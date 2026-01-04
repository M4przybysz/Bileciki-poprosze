using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================
    
    // Serialized elements
    [SerializeField] Transform passengerCarsContainer;
    [SerializeField] Transform passengerContainer;
    [SerializeField] GameObject passengerPrefab;

    // Consts
    static readonly string[] stationNames = {"Rzeszów Główny", "Stalowa Wola Rozwadów", "Lublin Główny", "Warszawa Centralna", 
        "Łowicz Główny", "Włocławek", "Toruń Główny", "Bydgoszcz Główna", "Piła Główna", "Kołobrzeg"};
    static readonly int[] minutesPerStationStop = {10, 10, 15, 10, 10, 15, 10, 15, 15};
    static readonly int[] minutesPerRide = {54, 60, 123, 44, 41, 18, 32, 60, 150};
    const int maxPassengersNumber = 174;
    const int startStationNumber = 0;
    const int endStationNumber = 9;
    const int timeScale = 2; // How many times faster does in-game time flow

    // Train elements
    public static TrainCar[] passengerCars;
    public static List<GameObject> passengersList = new List<GameObject>();

    // Route and rounds variables
    string trainState; // either "stop" or "ride"
    public int currentStationNumber { get; private set; }
    string currentStationName;
    string nextStationName;

    // Timer
    float targetTime;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

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

        // Set up start of the route
        trainState = "stop";
        currentStationNumber = startStationNumber;
        currentStationName = stationNames[currentStationNumber];
        nextStationName = stationNames[currentStationNumber + 1];

        // Set first staton stop timer
        targetTime = minutesPerStationStop[currentStationNumber] * 60;

        // Spawn first passengers
        SpawnPassengers(Random.Range(5, 16));
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStationNumber < endStationNumber)
        {
            targetTime -= Time.deltaTime * timeScale;
            print(targetTime);

            if(targetTime <= 0)
            {
                if(trainState == "stop") // End of station stop time
                {
                    targetTime = minutesPerRide[currentStationNumber] * 60;
                    trainState = "ride";
                    print($"Starting ride from {currentStationName} to {nextStationName}");
                }
                else if(trainState == "ride") // End of ride time
                {
                    currentStationNumber += 1;
                    currentStationName = stationNames[currentStationNumber];

                    if(currentStationNumber == endStationNumber) { nextStationName = "Koniec trasy"; }
                    else 
                    { 
                        nextStationName = stationNames[currentStationNumber + 1]; 
                        SpawnPassengers(Random.Range(5, 16));

                        targetTime = minutesPerStationStop[currentStationNumber] * 60;
                    }

                    trainState = "stop";
                    print($"Stopping on station {currentStationName}");
                }
            }   
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
