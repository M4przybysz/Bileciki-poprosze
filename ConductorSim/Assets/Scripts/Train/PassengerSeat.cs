using UnityEngine;
using System.Text.RegularExpressions;

public class PassengerSeat : MonoBehaviour
{
    public int seatNumber;                 // LOGICZNY numer miejsca
    public Vector3 seatPosition { get; private set; }
    public bool isTaken = false;

    void Awake()
    {
        seatNumber = ResolveSeatNumber();
    }

    void Start()
    {
        seatPosition = transform.position;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = -100;
    }

    int ResolveSeatNumber()
    {
        // 1️⃣ Wyciągamy numer z nazwy SeatX
        Match match = Regex.Match(gameObject.name, @"\d+");
        if (!match.Success)
        {
            Debug.LogWarning($"Cannot resolve seat number from name: {name}", this);
            return 0;
        }

        int baseNumber = int.Parse(match.Value);

        // 2️⃣ Sprawdzamy typ wagonu (parenty)
        Transform t = transform;
        while (t != null)
        {
            string n = t.name;

            // Class1Car_v2 → miejsca 11–46
            if (n.StartsWith("Class1Car_v2"))
                return baseNumber + 10;

            // Class1Car_v1 → 1–46
            if (n.StartsWith("Class1Car_v1"))
                return baseNumber;

            // Class2Car / Class2Car(1) / Class2Car(2) → 1–48
            if (n.StartsWith("Class2Car"))
                return baseNumber;

            t = t.parent;
        }

        // fallback
        return baseNumber;
    }
}
