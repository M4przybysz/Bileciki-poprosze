using UnityEngine;

public enum FatiguePhase { Rested, Tired, VeryTired, Exhausted, PassingOut }

public class PlayerFatigue : MonoBehaviour
{
    [Header("Fatigue")]
    [Range(0f, 100f)]
    public float fatigue = 0f;

    [SerializeField] float baseFatigueGain = 1f;
    [SerializeField] float sprintFatigueMultiplier = 1.15f;

    public FatiguePhase currentPhase;

    bool isSprinting;

    void Update()
    {
        UpdateFatigue();
        UpdatePhase();
    }

    // ====================================================================
    // Fatigue logic
    // ====================================================================

    void UpdateFatigue()
    {
        float gain = baseFatigueGain * Time.deltaTime;

        if (isSprinting) { gain *= sprintFatigueMultiplier; }

        fatigue = Mathf.Clamp(fatigue + gain, 0f, 100f);
    }

    void UpdatePhase()
    {
        if (fatigue < 50f) { currentPhase = FatiguePhase.Rested; }
        else if (fatigue < 70f) { currentPhase = FatiguePhase.Tired; }
        else if (fatigue < 85f) { currentPhase = FatiguePhase.VeryTired; }
        else if (fatigue < 95f) { currentPhase = FatiguePhase.Exhausted; }
        else { currentPhase = FatiguePhase.PassingOut; }
    }

    // ====================================================================
    // Public API (for other systems)
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
