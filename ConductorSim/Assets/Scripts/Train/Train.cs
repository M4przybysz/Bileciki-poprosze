using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Train : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================
    
    // Serialized elements
    [SerializeField] Whistle whistle;
    [SerializeField] PlayerController player;
    [SerializeField] GameEndScreenController gameEndScreen;
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
    public const int timeScale = 3; // How many times faster does in-game time flow (default is x3)

    // Train elements
    public static TrainCar[] passengerCars;
    public static List<GameObject> passengersList = new List<GameObject>();

    // Route and rounds variables
    public string trainState {get; private set;} // either "stop" or "ride"
    public int currentStationNumber { get; private set; }
    public string currentStationName { get; private set; }
    public string nextStationName { get; private set; }

    // Timer
    public float targetTime { get; private set; }

    // Passenger info
    public int passengersCounter = 0;
    public int checkedPassengersCounter = 0;
    public int mistakesCounter = 0;
    public int uncheckedFakersCounter = 0;

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

        if(GameManager.loadTrain) 
        { 
            trainState = GameManager.trainState;
            currentStationName = GameManager.currentStationName;
            nextStationName = GameManager.nextStationName;
            currentStationNumber = GameManager.currentStationNumber;
            targetTime = GameManager.targetTime;

            passengersCounter = GameManager.trainCounters[0];
            checkedPassengersCounter  = GameManager.trainCounters[1];
            mistakesCounter = GameManager.trainCounters[2];
            uncheckedFakersCounter = GameManager.trainCounters[3];
        }
        else
        {
            // Set up start of the route
            trainState = "stop";
            currentStationNumber = startStationNumber;
            currentStationName = stationNames[currentStationNumber];
            nextStationName = stationNames[currentStationNumber + 1];

            // Set first staton stop timer
            targetTime = minutesPerStationStop[currentStationNumber] * 60;

            // Spawn first passengers
            SpawnPassengers(UnityEngine.Random.Range(5, 16));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStationNumber < endStationNumber && !player.isGamePaused)
        {
            targetTime -= Time.deltaTime * timeScale;
            // print(targetTime);

            if(targetTime <= 0)
            {
                if(trainState == "stop") // End of station stop time
                {
                    whistle.PlayRideWhistle();
                    targetTime = minutesPerRide[currentStationNumber] * 60;
                    trainState = "ride";
                    print($"Starting ride from {currentStationName} to {nextStationName}. Passneger count: {passengersList.Count}");
                }
                else if(trainState == "ride") // End of ride time
                {
                    whistle.PlayStationWhistle();
                    print($"Stopped on station {currentStationName}");

                    currentStationNumber += 1;
                    currentStationName = stationNames[currentStationNumber];
                    CheckPassengerLeave();

                    if(currentStationNumber == endStationNumber) 
                    { 
                        nextStationName = "Koniec trasy"; 
                        PrintStatistics();
                    }
                    else 
                    { 
                        nextStationName = stationNames[currentStationNumber + 1]; 
                        targetTime = minutesPerStationStop[currentStationNumber] * 60;
                        SpawnPassengers(UnityEngine.Random.Range(5, 16));
                    }

                    trainState = "stop";
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
                    passengersCounter += 1;
                }   
        }
        else { print($"Reached passenger limit: {maxPassengersNumber} \nLast passenger: {passengersList[passengersList.Count - 1].GetComponent<Passenger>().FirstName}"); }
    }

    public void RemovePassenger(GameObject passenger)
    {
        passengersList.Remove(passenger);
    }

    void CheckPassengerLeave()
    {
        foreach(GameObject passenger in passengersList)
        {
            if(passenger.GetComponent<Passenger>().ticketData.stacjaDo == currentStationName)
            {
                Destroy(passenger);
            }
        }
    }

    public void SkipRide()
    {
        targetTime = 10; // Skip time to the end of the raid
    }

    void PrintStatistics()
    {
        // Get salary, pay rent and pay for mistakes
        int mistakesCost = (uncheckedFakersCounter + mistakesCounter) * 8;
        player.AddMoneyToWallet(PlayerController.salary - 40 - mistakesCost);

        print($"Statystyki:\n  - Ilość pasażerów: {passengersCounter}\n  - Ilość niesprawdzonych pasażerów: {passengersCounter - checkedPassengersCounter}\n  - Ilość niesprawdzonych oszustów: {uncheckedFakersCounter}\n  - Ilość sprawdzonych pasażerów: {checkedPassengersCounter}\n  - Ilość poprawnie sprawdzonych pasażerów: {checkedPassengersCounter - mistakesCounter}\n  - Ilość błędnie sprawdzonych pasażerów: {mistakesCounter}\n\n  - Wypłata: {PlayerController.salary} zł\n  - Opłata za mieszkanie w wagonie: 40 zł\n  - Wypłata potrącona za błędy i niezłapanych oszustów: {mistakesCost} zł\n  - Zawartość portfela na koniec dnia: {player.GetWallet()} zł");
        
        string stats = $"Podsumowanie dzisiejszego dnia:\n  - Ilość pasażerów: {passengersCounter}\n  - Ilość niesprawdzonych pasażerów: {passengersCounter - checkedPassengersCounter}\n  - Ilość niesprawdzonych oszustów: {uncheckedFakersCounter}\n  - Ilość sprawdzonych pasażerów: {checkedPassengersCounter}\n  - Ilość poprawnie sprawdzonych pasażerów: {checkedPassengersCounter - mistakesCounter}\n  - Ilość błędnie sprawdzonych pasażerów: {mistakesCounter}\n\n  - Wypłata: {PlayerController.salary} zł\n  - Opłata za mieszkanie w wagonie: 40 zł\n  - Wypłata potrącona za błędy i niezłapanych oszustów: {mistakesCost} zł\n  - Zawartość portfela na koniec dnia: {player.GetWallet()} zł";
        gameEndScreen.ShowEndScreen(stats);
    }
}