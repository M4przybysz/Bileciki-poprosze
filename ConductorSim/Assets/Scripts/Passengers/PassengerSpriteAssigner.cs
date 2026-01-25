using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Przypisuje odpowiedni sprite do wygenerowanego pasa¿era na podstawie pola Gender/Character/Type oraz wieku.
/// Umieœæ komponent na prefabie pasa¿era (ten sam obiekt co `Passenger`) i w Inspektorze wype³nij bazê sprite'ów.
/// Dzia³a automatycznie po wygenerowaniu profilu pasa¿era; mo¿na te¿ wywo³aæ publicznie `AssignFor(Passenger)`.
/// </summary>
[RequireComponent(typeof(Passenger))]
public class PassengerSpriteAssigner : MonoBehaviour
{
    [Header("Renderer docelowy")]
    [Tooltip("Jeœli puste, spróbuje znaleŸæ pierwszy SpriteRenderer w dzieciach prefab'u.")]
    [SerializeField] SpriteRenderer targetRenderer;

    [Header("Domyœlne sprite'y wg p³ci (doros³y)")]
    [Tooltip("U¿ywane jeœli nie ma dopasowania dla charakteru/type")]
    [SerializeField] Sprite[] defaultMaleSprites;
    [SerializeField] Sprite[] defaultFemaleSprites;

    [Header("Sprite'y dla dziecka")]
    [SerializeField] Sprite[] childMaleSprites;
    [SerializeField] Sprite[] childFemaleSprites;

    [Header("Sprite'y dla starszego (senior)")]
    [SerializeField] Sprite[] seniorMaleSprites;
    [SerializeField] Sprite[] seniorFemaleSprites;

    [Header("Sprite'y dla emeryta/pensionera")]
    [SerializeField] Sprite[] pensionerMaleSprites;
    [SerializeField] Sprite[] pensionerFemaleSprites;

    [Header("Sprite'y specjalne dla charakterów (priorytetowe)")]
    [Tooltip("Dla ka¿dego charakteru mo¿esz podaæ zestaw sprite'ów (male/female).")]
    [SerializeField] List<CharacterSpriteSet> characterSets = new List<CharacterSpriteSet>();

    [Header("Sprite'y dla 'Problematic' (opcjonalnie)")]
    [SerializeField] Sprite[] problematicMaleSprites;
    [SerializeField] Sprite[] problematicFemaleSprites;

    [Header("Opcje")]
    [Tooltip("Jeœli true, losuje sprite z dostêpnych wariantów; jeœli false, bierze pierwszy.")]
    [SerializeField] bool randomizeVariant = true;

    Passenger passenger;

    [System.Serializable]
    public class CharacterSpriteSet
    {
        public PassengerCharacter character;
        public Sprite[] maleSprites;
        public Sprite[] femaleSprites;
    }

    enum AgeCategory { Child, Adult, Senior, Pensioner }

    void Awake()
    {
        passenger = GetComponent<Passenger>();
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<SpriteRenderer>();

        // jeœli Passenger istnieje i ma event ProfileReady, subskrybuj (jeœli jest)
        // subskrypcjê wykonujemy w Start() jako fallback, bo Passenger mo¿e jeszcze nie mieæ eventu ustawionego wczeœniej
    }

    void Start()
    {
        // Subskrybuj, jeœli Passenger udostêpnia event ProfileReady
        if (passenger != null)
        {
            try { passenger.ProfileReady += OnProfileReady; } catch { /* ignoruj jeœli event nie istnieje */ }
        }

        // czekamy krótko, ¿eby Passenger.Start() zd¹¿y³ wygenerowaæ dane
        StartCoroutine(AssignWhenReadyCoroutine());
    }

    void OnDestroy()
    {
        if (passenger != null)
        {
            try { passenger.ProfileReady -= OnProfileReady; } catch { /* ignoruj */ }
        }
    }

    IEnumerator AssignWhenReadyCoroutine()
    {
        // czekamy maksymalnie kilka klatek na inicjalizacjê Passenger
        int attempts = 0;
        while ((passenger == null || string.IsNullOrEmpty(passenger.FirstName)) && attempts < 10)
        {
            attempts++;
            yield return null;
        }

        AssignFor(passenger);
    }

    void OnProfileReady()
    {
        // Profil pasa¿era gotowy — przypisz sprite
        AssignFor(passenger);
    }

    /// <summary>
    /// Publiczne API - przypisz sprite do konkretnego Passenger (np. gdy tworzysz dynamicznie).
    /// </summary>
    public void AssignFor(Passenger p)
    {
        if (p == null) return;

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<SpriteRenderer>();

        if (targetRenderer == null)
        {
            Debug.LogWarning("PassengerSpriteAssigner: brak SpriteRenderer do przypisania sprite'a.");
            return;
        }

        Sprite chosen = PickSpriteFor(p);
        if (chosen != null)
        {
            targetRenderer.sprite = chosen;
        }
        else
        {
            Debug.Log($"PassengerSpriteAssigner: nie znaleziono sprite'a dla {p.FirstName} {p.LastName} (gender={p.Gender}, char={p.Character}, type={p.Type}, age={p.Age}).");
        }
    }

    Sprite PickSpriteFor(Passenger p)
    {
        // najpierw kategoria wiekowa (pensioner override)
        AgeCategory cat = GetAgeCategory(p);

        // 1) Pensioner override
        if (cat == AgeCategory.Pensioner)
        {
            var arr = (p.Gender == PassengerGender.M) ? pensionerMaleSprites : pensionerFemaleSprites;
            var s = PickFromArray(arr);
            if (s != null) return s;
        }

        // 2) Child override
        if (cat == AgeCategory.Child)
        {
            var arr = (p.Gender == PassengerGender.M) ? childMaleSprites : childFemaleSprites;
            var s = PickFromArray(arr);
            if (s != null) return s;
        }

        // 3) Senior override
        if (cat == AgeCategory.Senior)
        {
            var arr = (p.Gender == PassengerGender.M) ? seniorMaleSprites : seniorFemaleSprites;
            var s = PickFromArray(arr);
            if (s != null) return s;
        }

        // 4) Problematic override (jeœli istnieje)
        if (p.Type == PassengerType.Problematic)
        {
            var arr = (p.Gender == PassengerGender.M) ? problematicMaleSprites : problematicFemaleSprites;
            var s = PickFromArray(arr);
            if (s != null) return s;
        }

        // 5) Character-specific
        var set = characterSets.FirstOrDefault(c => c.character == p.Character);
        if (set != null)
        {
            var arr = (p.Gender == PassengerGender.M) ? set.maleSprites : set.femaleSprites;
            var s = PickFromArray(arr);
            if (s != null) return s;
        }

        // 6) Default by gender (adult default)
        var def = (p.Gender == PassengerGender.M) ? defaultMaleSprites : defaultFemaleSprites;
        var chosen = PickFromArray(def);
        if (chosen != null) return chosen;

        // 7) Fallback: any sprite from any default
        chosen = PickFromArray(defaultMaleSprites) ?? PickFromArray(defaultFemaleSprites);
        return chosen;
    }

    AgeCategory GetAgeCategory(Passenger p)
    {
        if (p == null) return AgeCategory.Adult;

        // Pensioner: jeœli posiada pensionerIDData lub wiek >= 65
        if (p.pensionerIDData != null && !string.IsNullOrEmpty(p.pensionerIDData.firstName)) return AgeCategory.Pensioner;
        if (p.Age >= 65) return AgeCategory.Pensioner;

        // Child: wiek do 14 w³¹cznie
        if (p.Age <= 14) return AgeCategory.Child;

        // Senior: 50-64 jako "starszy"
        if (p.Age >= 50 && p.Age < 65) return AgeCategory.Senior;

        // Domyœlnie doros³y
        return AgeCategory.Adult;
    }

    Sprite PickFromArray(Sprite[] arr)
    {
        if (arr == null || arr.Length == 0) return null;
        if (randomizeVariant)
        {
            return arr[Random.Range(0, arr.Length)];
        }
        return arr[0];
    }

#if UNITY_EDITOR
    // Umo¿liwia rêczne odœwie¿enie w edytorze
    [ContextMenu("Assign Sprite Now")]
    void EditorAssignNow()
    {
        AssignFor(passenger ?? GetComponent<Passenger>());
    }
#endif
}