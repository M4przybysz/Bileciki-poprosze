using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum FatiguePhase { Rested, Tired, VeryTired, Exhausted, PassingOut }

public class PlayerFatigue : MonoBehaviour
{
    // ====================================================================
    // Fatigue
    // ====================================================================

    [Header("Fatigue")]
    [Range(0f, 100f)]
    public float fatigue = 0f;

    [SerializeField] float baseFatigueGain = 1f;
    [SerializeField] float sprintFatigueMultiplier = 1.15f;

    public FatiguePhase currentPhase;

    bool isSprinting;
    bool isPassingOut;

    FatiguePhase previousPhase;

    public bool IsExhausted => fatigue >= 100f;

    public bool IsPassingOut => isPassingOut;

    public bool IsImmobilized => isPassingOut || fatigue >= 100f;

    // --- nowe pola do efektów z jedzenia (minimalne zmiany) ---
    float fatigueGainMultiplier = 1f; // 1 = normalne narastanie, mniejsze = wolniej
    bool preventFatigue = false;
    Coroutine preventFatigueCoroutine;
    Coroutine capacityCoroutine;

    // ====================================================================
    // Dialogue 
    // ====================================================================

    [Header("Dialogue")]
    [TextArea][SerializeField] string[] tiredLines;
    [TextArea][SerializeField] string[] veryTiredLines;
    [TextArea][SerializeField] string[] exhaustedLines;
    [TextArea][SerializeField] string[] passingOutLines;

    // ====================================================================
    // Audio 
    // ====================================================================

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip yawnClip;

    // ====================================================================
    // Screen effects 
    // ====================================================================

    [Header("Screen Effects")]
    [SerializeField] Volume postProcessVolume;
    [SerializeField] Image blackScreen;
    [SerializeField] [Range(0f, 1f)] float maxDarkness = 0.8f;
    [SerializeField] float blackFadeSpeed = 2f;
    Vignette vignette;

    // ====================================================================
    // Unity
    // ====================================================================

    void Start()
    {
        if (postProcessVolume != null)
            postProcessVolume.profile.TryGet(out vignette);

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
        else
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        UpdatePhaseImmediate();
        previousPhase = currentPhase;

        if (blackScreen != null) SetBlackAlpha(Mathf.Lerp(0f, maxDarkness, fatigue / 100f));
    }

    void Update()
    {
        if (isPassingOut) return;

        UpdateFatigue();
        UpdatePhase();
        UpdateBlackScreen();
    }

    // ====================================================================
    // Fatigue logic
    // ====================================================================

    void UpdateFatigue()
    {
        if (preventFatigue) return;

        float gain = baseFatigueGain * fatigueGainMultiplier * Time.deltaTime;
        if (isSprinting) gain *= sprintFatigueMultiplier;

        fatigue = Mathf.Clamp(fatigue + gain, 0f, 100f);
    }

    void UpdatePhase()
    {
        FatiguePhase newPhase;
        if (fatigue < 50f) newPhase = FatiguePhase.Rested;
        else if (fatigue < 70f) newPhase = FatiguePhase.Tired;
        else if (fatigue < 85f) newPhase = FatiguePhase.VeryTired;
        else if (fatigue < 95f) newPhase = FatiguePhase.Exhausted;
        else newPhase = FatiguePhase.PassingOut;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            OnPhaseChanged(newPhase);
        }
    }

    void UpdatePhaseImmediate()
    {
        if (fatigue < 50f) currentPhase = FatiguePhase.Rested;
        else if (fatigue < 70f) currentPhase = FatiguePhase.Tired;
        else if (fatigue < 85f) currentPhase = FatiguePhase.VeryTired;
        else if (fatigue < 95f) currentPhase = FatiguePhase.Exhausted;
        else currentPhase = FatiguePhase.PassingOut;
    }

    // ====================================================================
    // Public API - minimalne metody używane przez sklep
    // ====================================================================
    public void ClearEffects()
    {
        if (preventFatigueCoroutine != null) { StopCoroutine(preventFatigueCoroutine); preventFatigueCoroutine = null; }
        if (capacityCoroutine != null) { StopCoroutine(capacityCoroutine); capacityCoroutine = null; }

        preventFatigue = false;
        fatigueGainMultiplier = 1f;

        // Zresetuj zmęczenie do zera — od tej chwili znów będzie normalnie narastać
        fatigue = 0f;

        // Zaktualizuj fazę i wywołaj reakcje jeśli potrzeba (np. vignette)
        UpdatePhaseImmediate();
        OnPhaseChanged(currentPhase);

        Debug.Log("PlayerFatigue: efekty usunięte, zmęczenie zresetowane do 0.");
    }

    public void PreventFatigueFor(float seconds)
    {
        if (preventFatigueCoroutine != null) StopCoroutine(preventFatigueCoroutine);
        preventFatigueCoroutine = StartCoroutine(PreventFatigueRoutine(seconds));
    }

    IEnumerator PreventFatigueRoutine(float seconds)
    {
        preventFatigue = true;
        yield return new WaitForSeconds(seconds);
        preventFatigue = false;
        preventFatigueCoroutine = null;
    }

    public void ApplyFatigueCapacityBonusPercent(float percent, float duration)
    {
        if (capacityCoroutine != null) StopCoroutine(capacityCoroutine);
        capacityCoroutine = StartCoroutine(CapacityRoutine(percent, duration));
    }

    IEnumerator CapacityRoutine(float percent, float duration)
    {
        float previous = fatigueGainMultiplier;
        fatigueGainMultiplier = Mathf.Clamp01(fatigueGainMultiplier * (1f - percent));
        yield return new WaitForSeconds(duration);
        fatigueGainMultiplier = previous;
        capacityCoroutine = null;
    }

    // ====================================================================
    // Phase reactions 
    // ====================================================================

    void OnPhaseChanged(FatiguePhase phase)
    {
        switch (phase)
        {
            case FatiguePhase.Tired:
                TriggerDialogue(tiredLines);
                break;

            case FatiguePhase.VeryTired:
                TriggerDialogue(veryTiredLines);
                PlayYawn();
                break;

            case FatiguePhase.Exhausted:
                TriggerDialogue(exhaustedLines);
                PlayYawn();
                SetVignette(0.25f);
                break;

            case FatiguePhase.PassingOut:
                TriggerDialogue(passingOutLines);
                StartCoroutine(PassOutRoutine());
                break;

            case FatiguePhase.Rested:
                SetVignette(0f);
                break;
        }
    }

    // ====================================================================
    // Passing out 
    // ====================================================================

    IEnumerator PassOutRoutine()
    {
        isPassingOut = true;

        PlayPassOut();
        SetVignette(0.4f);

        if (blackScreen != null)
            yield return FadeBlackScreen(1f, 0.5f);
        else
            yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(2f);

        if (blackScreen != null)
            yield return FadeBlackScreen(Mathf.Lerp(0f, maxDarkness, 90f / 100f), 0.5f);
        else
            yield return new WaitForSeconds(0.5f);

        fatigue = 90f;
        isPassingOut = false;
    }

    // ====================================================================
    // Helpers 
    // ====================================================================

    void TriggerDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;
        string line = lines[Random.Range(0, lines.Length)];
        Debug.Log("[Fatigue] " + line);
    }

    void PlayYawn()
    {
        if (audioSource == null) { Debug.LogWarning("PlayerFatigue: brak AudioSource — nie odtworzono yawnClip."); return; }
        if (yawnClip == null) { Debug.LogWarning("PlayerFatigue: yawnClip nie ustawiony."); return; }
        audioSource.PlayOneShot(yawnClip);
    }

    void PlayPassOut()
    {
        if (audioSource == null) { Debug.Log("PlayerFatigue: PlayPassOut wywołane, lecz nie ma AudioSource."); return; }
        Debug.Log("PlayerFatigue: pass-out audio usunięte z kodu (brak clipa).");
    }

    void SetVignette(float intensity)
    {
        if (vignette == null) return;
        vignette.intensity.Override(intensity);
    }

    IEnumerator FadeBlackScreen(float targetAlpha, float duration)
    {
        if (blackScreen == null) yield break;

        Color start = blackScreen.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Lerp(start.a, targetAlpha, time / duration);
            SetBlackAlpha(a);
            yield return null;
        }

        SetBlackAlpha(targetAlpha);
    }

    void UpdateBlackScreen()
    {
        if (blackScreen == null) return;

        float target = Mathf.Lerp(0f, maxDarkness, fatigue / 100f);
        float currentA = blackScreen.color.a;
        float nextA = Mathf.MoveTowards(currentA, target, Time.deltaTime * blackFadeSpeed);
        SetBlackAlpha(nextA);
    }

    void SetBlackAlpha(float a)
    {
        if (blackScreen == null) return;
        Color c = blackScreen.color;
        c.a = Mathf.Clamp01(a);
        blackScreen.color = c;
    }

    // ====================================================================
    // Public API (pozostałe)
    // ====================================================================
    public void SetSprinting(bool sprinting) { isSprinting = sprinting; }

    public float GetSpeedModifier()
    {
        switch (currentPhase)
        {
            case FatiguePhase.Tired: return 0.95f;
            case FatiguePhase.VeryTired: return 0.9f;
            case FatiguePhase.Exhausted: return 0.85f;
            case FatiguePhase.PassingOut: return 0.8f;
            default: return 1f;
        }
    }

    public void RestoreFatigue(float amount)
    {
        fatigue = Mathf.Clamp(fatigue - amount, 0f, 100f);
    }
}