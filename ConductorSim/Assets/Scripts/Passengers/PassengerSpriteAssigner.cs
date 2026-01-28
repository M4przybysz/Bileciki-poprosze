using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Passenger))]
public class PassengerSpriteAssigner : MonoBehaviour
{
    [Header("Renderer docelowy")]
    [SerializeField] SpriteRenderer targetRenderer;

    [Header("Domyślne sprite'y wg płci (dorosły)")]
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

    [Header("Sprite'y specjalne dla charakterów")]
    [SerializeField] List<CharacterSpriteSet> characterSets = new();

    [Header("Sprite'y dla 'Problematic'")]
    [SerializeField] Sprite[] problematicMaleSprites;
    [SerializeField] Sprite[] problematicFemaleSprites;

    [Header("Opcje")]
    [SerializeField] bool randomizeVariant = true;

    Passenger passenger;
    bool initialFlip = false;

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
        if (targetRenderer != null)
            initialFlip = targetRenderer.flipX;
        else
            initialFlip = spriteFacesRightByDefault;
    }

    void Start()
    {
        try { passenger.ProfileReady += OnProfileReady; } catch { }
        StartCoroutine(AssignWhenReadyCoroutine());
    }

    void OnDestroy()
    {
        try { passenger.ProfileReady -= OnProfileReady; } catch { }
    }

    IEnumerator AssignWhenReadyCoroutine()
    {
        int attempts = 0;
        while ((passenger == null || string.IsNullOrEmpty(passenger.FirstName)) && attempts < 10)
        {
            attempts++;
            yield return null;
        }
        AssignFor(passenger);
    }

    void OnProfileReady() => AssignFor(passenger);

    public void AssignFor(Passenger p)
    {
        if (p == null) return;

        // Spróbuj przypisać renderer dynamicznie jeśli nie ustawiono w inspektorze
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<SpriteRenderer>();

        if (targetRenderer == null)
        {
            Debug.LogWarning($"PassengerSpriteAssigner: missing targetRenderer on {name}", this);
            return;
        }

        Sprite chosen = PickSpriteFor(p);
        if (chosen == null) return;

        targetRenderer.sprite = chosen;
        ApplyOrientationFromSeat();
    }

    // =========================================================
    // ORIENTACJA NA PODSTAWIE PassengerSeat.seatNumber
    // =========================================================
    [SerializeField] bool spriteFacesRightByDefault = false;
    
void ApplyOrientationFromSeat()
{
    try
    {

        PassengerSeat seat = GetComponentInParent<PassengerSeat>();
        if (seat == null)
        {
            // fallback: znajdź najbliższe miejsce w scenie
            seat = FindNearestSeat();
            if (seat == null)
            {
                Debug.LogWarning($"PassengerSpriteAssigner: no PassengerSeat parent for {name}", this);
                return;
            }
            else
            {
                Debug.Log($"PassengerSpriteAssigner: using nearest seat {seat.name} (#{seat.seatNumber}) for {name}", this);
            }
        }

        if (targetRenderer == null)
        {
            Debug.LogWarning($"PassengerSpriteAssigner: targetRenderer became null for {name}", this);
            return;
        }

        bool currentFlip = targetRenderer.flipX;
        Debug.Log($"ApplyOrientationFromSeat: {name} seat={seat.seatNumber} currentFlip={currentFlip}", this);

        if (seat.seatNumber == 0)
        {
            Debug.LogWarning($"PassengerSeat has seatNumber==0 for {seat.name}; check naming (Seat1..)", seat);
        }

        if (seat.seatNumber % 2 == 0)
            targetRenderer.flipX = !currentFlip;
        else
            targetRenderer.flipX = currentFlip;
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"ApplyOrientationFromSeat error on {name}: {ex}", this);
    }
}

    PassengerSeat FindNearestSeat()
    {
        PassengerSeat[] seats = FindObjectsByType<PassengerSeat>(FindObjectsSortMode.None);
        if (seats == null || seats.Length == 0) return null;

        PassengerSeat best = null;
        float bestDist = float.MaxValue;
        Vector3 myPos = transform.position;

        foreach (var s in seats)
        {
            // prefer non-taken seats
            float d = Vector3.SqrMagnitude(s.transform.position - myPos);
            if (d < bestDist)
            {
                bestDist = d;
                best = s;
            }
        }

        return best;
    }



    // =========================================================

    Sprite PickSpriteFor(Passenger p)
    {
        AgeCategory cat = GetAgeCategory(p);

        if (cat == AgeCategory.Pensioner)
            return PickFromArray(p.Gender == PassengerGender.M ? pensionerMaleSprites : pensionerFemaleSprites)
                ?? PickDefault(p);

        if (cat == AgeCategory.Child)
            return PickFromArray(p.Gender == PassengerGender.M ? childMaleSprites : childFemaleSprites)
                ?? PickDefault(p);

        if (cat == AgeCategory.Senior)
            return PickFromArray(p.Gender == PassengerGender.M ? seniorMaleSprites : seniorFemaleSprites)
                ?? PickDefault(p);

        if (p.Type == PassengerType.Problematic)
            return PickFromArray(p.Gender == PassengerGender.M ? problematicMaleSprites : problematicFemaleSprites)
                ?? PickDefault(p);

        var set = characterSets.FirstOrDefault(c => c.character == p.Character);
        if (set != null)
            return PickFromArray(p.Gender == PassengerGender.M ? set.maleSprites : set.femaleSprites)
                ?? PickDefault(p);

        return PickDefault(p);
    }

    Sprite PickDefault(Passenger p) =>
        PickFromArray(p.Gender == PassengerGender.M ? defaultMaleSprites : defaultFemaleSprites);

    AgeCategory GetAgeCategory(Passenger p)
    {
        if (p.pensionerIDData != null) return AgeCategory.Pensioner;
        if (p.Age >= 65) return AgeCategory.Pensioner;
        if (p.Age <= 14) return AgeCategory.Child;
        if (p.Age >= 50) return AgeCategory.Senior;
        return AgeCategory.Adult;
    }

    Sprite PickFromArray(Sprite[] arr)
    {
        if (arr == null || arr.Length == 0) return null;
        return randomizeVariant ? arr[Random.Range(0, arr.Length)] : arr[0];
    }
}
