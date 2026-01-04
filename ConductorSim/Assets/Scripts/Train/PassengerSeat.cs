using UnityEngine;

public class PassengerSeat : MonoBehaviour
{
    public int seatNumber;
    public Vector3 seatPosition { get; private set; }
    public bool isTaken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seatPosition = transform.position;
        GetComponent<SpriteRenderer>().sortingOrder = -100;   
    }
}
