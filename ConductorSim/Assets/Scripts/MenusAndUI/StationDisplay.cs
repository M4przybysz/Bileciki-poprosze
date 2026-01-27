using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class StationDisplay : MonoBehaviour
{
    [Header("èrÛd≥o danych")]
    [Tooltip("Jeúli nie ustawione, skrypt sprÛbuje znaleüÊ obiekt Train w scenie.")]
    [SerializeField] Train train;

    [Header("UI")]
    [Tooltip("TextMeshPro tylko dla aktualnej stacji (wyúwietla nazwÍ stacji).")]
    [SerializeField] TextMeshProUGUI currentStationText;

    [Header("Opcje wyúwietlania")]
    [Tooltip("Czas (w sekundach) przez jaki pokazuje siÍ kaøda z informacji (aktualna/nastÍpna).")]
    [SerializeField] float displayDuration = 3f;
    [Tooltip("Jeøeli prawda, zaczyna od aktualnej stacji, w przeciwnym razie od nastÍpnej.")]
    [SerializeField] bool startWithCurrent = true;

    [Header("Prefixy")]
    [Tooltip("Prefix wyúwietlany przed nazwπ aktualnej stacji.")]
    [SerializeField] string currentPrefix = "Aktualna stacja: ";
    [Tooltip("Prefix wyúwietlany przed nazwπ nastÍpnej stacji.")]
    [SerializeField] string nextPrefix = "NastÍpna stacja: ";

    // cache ostatnich wartoúci aby aktualizowaÊ UI tylko przy zmianie
    string lastCurrent = "";
    string lastNext = "";

    Coroutine alternationCoroutine;

    void Start()
    {
        if (train == null)
        {
            var tObj = GameObject.Find("Train");
            if (tObj != null) train = tObj.GetComponent<Train>();
        }

        ForceUpdate();

        if (currentStationText != null)
        {
            if (alternationCoroutine != null) StopCoroutine(alternationCoroutine);
            alternationCoroutine = StartCoroutine(AlternateDisplayRoutine());
        }
    }

    void OnDisable()
    {
        if (alternationCoroutine != null) { StopCoroutine(alternationCoroutine); alternationCoroutine = null; }
    }

    void OnDestroy()
    {
        if (alternationCoroutine != null) { StopCoroutine(alternationCoroutine); alternationCoroutine = null; }
    }

    System.Collections.IEnumerator AlternateDisplayRoutine()
    {
        // krÛtkie zabezpieczenie przed zerowym czasem
        float dur = Mathf.Max(0.1f, displayDuration);

        bool showCurrent = startWithCurrent;

        while (true)
        {
            if (train == null || currentStationText == null)
            {
                yield return null;
                continue;
            }

            // Pobierz aktualne wartoúci bezpoúrednio przed wyúwietleniem
            string current = train.currentStationName ?? "";
            string next = train.nextStationName ?? "";

            if (showCurrent)
            {
                // Jeøeli nastπpi≥a zmiana stacji od ostatniego wyúwietlenia, od razu odúwieø cache
                if (current != lastCurrent)
                {
                    lastCurrent = current;
                }

                currentStationText.text = string.IsNullOrEmpty(current) ? "" : currentPrefix + current;
            }
            else
            {
                if (next != lastNext)
                {
                    lastNext = next;
                }

                currentStationText.text = string.IsNullOrEmpty(next) ? "" : nextPrefix + next;
            }

            // Czekaj korzystajπc z czasu skalowanego (standardowe UI) ó unscaled moøna uøyÊ jeúli wymagane
            float timer = 0f;
            while (timer < dur)
            {
                timer += Time.deltaTime;
                // Jeøeli stacja zmieni≥a siÍ w trakcie odliczania i pokazujemy aktualnπ, zaktualizuj tekst natychmiast
                if (showCurrent && train.currentStationName != lastCurrent)
                {
                    lastCurrent = train.currentStationName ?? "";
                    currentStationText.text = string.IsNullOrEmpty(lastCurrent) ? "" : currentPrefix + lastCurrent;
                }
                else if (!showCurrent && train.nextStationName != lastNext)
                {
                    lastNext = train.nextStationName ?? "";
                    currentStationText.text = string.IsNullOrEmpty(lastNext) ? "" : nextPrefix + lastNext;
                }

                yield return null;
            }

            showCurrent = !showCurrent;
        }
    }

    // Odúwieøa natychmiast (np. przy inicjalizacji UI)
    public void ForceUpdate()
    {
        if (train == null || currentStationText == null) return;

        lastCurrent = train.currentStationName ?? "";
        lastNext = train.nextStationName ?? "";

        // Ustaw tekst na to, od czego zaczynamy ó z prefixem
        currentStationText.text = startWithCurrent
            ? (string.IsNullOrEmpty(lastCurrent) ? "" : currentPrefix + lastCurrent)
            : (string.IsNullOrEmpty(lastNext) ? "" : nextPrefix + lastNext);
    }
}