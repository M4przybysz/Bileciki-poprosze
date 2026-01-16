using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;

    // General variables
    public static bool doesSaveExist {get; private set;}
    public static bool showCreditsOnTitle = false;
    public static bool loadTrain = false;

    // Constant game data
    public const int startingInGameYear = 2006;
    public static DateTime startingInGameDate = new(startingInGameYear, 1, 18, 9, 6, 0);
    public static float startingPlayerWallet = 84f;
    public static Vector3 startingPlayerPosition = new(50, 0, 0);

    //========================================================================
    // Game save data
    //========================================================================

    // Settings
    public static float SFXVolume;
    public static float musicVolume;

    // General
    public static DateTime currentDateTime;

    // Player
    public static Vector3 playerPosition;
    public static float playerWallet; 

    // Train
    public static string trainState, currentStationName, nextStationName;
    public static int currentStationNumber;
    public static float targetTime;
    public static int[] trainCounters = {0, 0, 0, 0};

    // Passengers
    public static PassnegerSaveData[] passnegerSaveDatas = {};

    //========================================================================
    // Awake, Start and Update
    //========================================================================
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance == null)
        {
            LoadGameData();
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetPlayerDataToSave()
    {
        if(GameObject.Find("Player").TryGetComponent<PlayerController>(out var player))
        {
            playerPosition = player.transform.position;
            playerWallet = player.GetWallet();   
        }
    }

    public static void SetTrainDataToSave()
    {
        if(GameObject.Find("Train").TryGetComponent<Train>(out var train))
        {
            trainState = train.trainState;
            currentStationName = train.currentStationName;
            nextStationName = train.nextStationName;
            currentStationNumber = train.currentStationNumber;
            targetTime = train.targetTime;
            trainCounters = new int[4] {train.passengersCounter, train.checkedPassengersCounter, train.mistakesCounter, train.uncheckedFakersCounter};

            passnegerSaveDatas = new PassnegerSaveData[Train.passengersList.Count];

            for(int i = 0; i < passnegerSaveDatas.Length; i++)
            {
                passnegerSaveDatas[i] = new()
                {
                    isChecked = Train.passengersList[i].GetComponent<Passenger>().isChecked,
                    type = Train.passengersList[i].GetComponent<Passenger>().Type,
                    character = Train.passengersList[i].GetComponent<Passenger>().Character,
                    gender = Train.passengersList[i].GetComponent<Passenger>().Gender,
                    firstName = Train.passengersList[i].GetComponent<Passenger>().FirstName,
                    lastName = Train.passengersList[i].GetComponent<Passenger>().LastName,
                    pesel = Train.passengersList[i].GetComponent<Passenger>().PESEL,
                    dateOfBirth = Train.passengersList[i].GetComponent<Passenger>().DateOfBirth.ToString("dd.MM.yyyy"),
                    ticketData = Train.passengersList[i].GetComponent<Passenger>().ticketData,
                    personalIDData = Train.passengersList[i].GetComponent<Passenger>().personalIDData ?? null,
                    schoolIDData = Train.passengersList[i].GetComponent<Passenger>().schoolIDData ?? null,
                    universityIDData = Train.passengersList[i].GetComponent<Passenger>().universityIDData ?? null,
                    armyIDData = Train.passengersList[i].GetComponent<Passenger>().armyIDData ?? null,
                    pensionerIDData = Train.passengersList[i].GetComponent<Passenger>().pensionerIDData ?? null
                };
            }
        }
    }

    //========================================================================
    // Managing save data
    //========================================================================
    class SaveData
    {
        // Settings
        public float SFXVolume;
        public float musicVolume;

        // General
        public long currentDateTime;

        // Player
        public Vector3 playerPosition;
        public float playerWallet;

        // Train
        public string trainState, currentStationName, nextStationName;
        public int currentStationNumber;
        public float targetTime;
        public int[] trainCounters;

        // Passengers
        public PassnegerSaveData[] passnegerSaveDatas;
    }

    public static void SaveGameData()
    {
        SaveData data = new(); // Create new save data 

        // Set save data
        data.SFXVolume = SFXVolume;
        data.musicVolume = musicVolume;
        data.currentDateTime = currentDateTime.ToBinary();

        data.playerPosition = playerPosition;
        data.playerWallet = playerWallet;

        data.trainState = trainState;
        data.currentStationName = currentStationName;
        data.nextStationName = nextStationName;
        data.currentStationNumber = currentStationNumber;
        data.targetTime = targetTime;
        data.trainCounters = trainCounters;

        data.passnegerSaveDatas = passnegerSaveDatas;

        // Write json save file
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            doesSaveExist = true; // Confirm save existence

            // Read json savefile
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Load save data
            SFXVolume = data.SFXVolume;
            musicVolume = data.musicVolume;
            currentDateTime = DateTime.FromBinary(data.currentDateTime);

            playerPosition = data.playerPosition;
            playerWallet = data.playerWallet;

            trainState = data.trainState;
            currentStationName = data.currentStationName;
            nextStationName = data.nextStationName;
            currentStationNumber = data.currentStationNumber;
            targetTime = data.targetTime;
            trainCounters = data.trainCounters;

            passnegerSaveDatas = data.passnegerSaveDatas;
        }
        else { SetDefaultData(); }
    }

    public static void SetDefaultData()
    {
        // SFXVolume = 0.5f;
        // musicVolume = 0.5f;
        currentDateTime = startingInGameDate;
        playerPosition = startingPlayerPosition;
        playerWallet = startingPlayerWallet;
    }
}

[Serializable] public class PassnegerSaveData
    {
        // Passenger personal info
        public bool isChecked;
        public PassengerType type;
        public PassengerCharacter character;
        public PassengerGender gender;
        public string firstName, lastName, pesel, dateOfBirth;

        // Ticket
        public TicketData ticketData;

        // Documents
        public PersonalIDData personalIDData = null;
        public SchoolIDData schoolIDData = null;
        public UniversityIDData universityIDData = null;
        public ArmyIDData armyIDData = null;
        public PensionerIDData pensionerIDData = null;
    }
