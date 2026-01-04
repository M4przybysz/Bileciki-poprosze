using UnityEngine;

public class TrainCar : MonoBehaviour
{
    [SerializeField] Transform seatsContainer;
    public int carNumber;
    public PassengerSeat[] passengerSeats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get all seats in a train car
        passengerSeats = new PassengerSeat[seatsContainer.childCount];
        string debugInfo = $"Car {carNumber} seats:\n";

        for(int i = 0; i < seatsContainer.childCount; i++)
        {
            passengerSeats[i] = seatsContainer.GetChild(i).GetComponent<PassengerSeat>();
            debugInfo += $"\t Seat {i + 1}: {passengerSeats[i]} [{passengerSeats[i].seatNumber}, {passengerSeats[i].isTaken}]\n";
        }
        print(debugInfo);
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================

    public PassengerSeat FindSeat(int seatNumber)
    {
        foreach(PassengerSeat seat in passengerSeats)
        {
            if(seat.seatNumber == seatNumber) { return seat; }
        }

        print($"Seat number {seatNumber} not found in car number {carNumber}");
        return null;
    }
}
