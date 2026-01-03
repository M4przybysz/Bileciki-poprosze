using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] Transform passengerCarsContainer;
    [SerializeField] Transform passengerContainer;
    [SerializeField] GameObject passengerPrefab;
    const int maxPassengersNumber = 174;
    public static TrainCar[] passengerCars;
    public static List<GameObject> passengersList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get all cars in the train
        passengerCars = new TrainCar[passengerCarsContainer.childCount];
        string debugInfo = $"Train cars:\n";

        for(int i = 0; i < passengerCarsContainer.childCount; i++)
        {
            passengerCars[i] = passengerCarsContainer.GetChild(i).GetComponent<TrainCar>();
            debugInfo += $"\t Car {i + 1}: {passengerCars[i]} [{passengerCars[i].carNumber}]\n";
        }
        print(debugInfo);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPassengers(Random.Range(5, 11));
            // print(passengers.Count);
        }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    public TrainCar FindTrainCar(int carNumber)
    {
        foreach(TrainCar car in passengerCars)
        {
            if(car.carNumber == carNumber) { return car; }
        }

        print($"Train car number {carNumber} not found");
        return null;
    }

    void SpawnPassengers(int numberOfPassengers)
    {
        if(passengersList.Count < maxPassengersNumber)
        {
            for(int i = 0; i < numberOfPassengers; i++)
                {
                    passengersList.Add(Instantiate(passengerPrefab, passengerContainer));
                }   
        }
        else { print($"Rached passenger limit: {maxPassengersNumber} \nLast passenger: {passengersList[passengersList.Count - 1].GetComponent<Passenger>().FirstName}"); }
    }

    public void RemovePassenger(GameObject passenger)
    {
        passengersList.Remove(passenger);
    }
}
