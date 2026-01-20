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
    public static bool loadTrainAndPassengers = false;

    // Constant game data
    public const int startingInGameYear = 2006;
    public static DateTime startingInGameDate = new(startingInGameYear, 1, 18, 9, 6, 0);
    public static float startingPlayerWallet = 84f;
    public static Vector3 startingPlayerPosition = new(52.5f, 0, 0);

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
    public static int passengerIndex = 0;
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
                Passenger targetPassenger = Train.passengersList[i].GetComponent<Passenger>();

                passnegerSaveDatas[i] = new()
                {
                    isChecked = targetPassenger.isChecked,
                    type = targetPassenger.Type,
                    character = targetPassenger.Character,
                    gender = targetPassenger.Gender,
                    firstName = targetPassenger.FirstName,
                    lastName = targetPassenger.LastName,
                    pesel = targetPassenger.PESEL,
                    dateOfBirth = targetPassenger.DateOfBirth.ToString("dd.MM.yyyy"),
                    age = targetPassenger.Age,
                    ticketData = targetPassenger.ticketData,
                    personalIDData = targetPassenger.personalIDData ?? null,
                    schoolIDData = targetPassenger.schoolIDData ?? null,
                    universityIDData = targetPassenger.universityIDData ?? null,
                    armyIDData = targetPassenger.armyIDData ?? null,
                    pensionerIDData = targetPassenger.pensionerIDData ?? null
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
            passengerIndex = 0;
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
        public int age;

        // Ticket
        public TicketData ticketData;

        // Documents
        public PersonalIDData personalIDData = null;
        public SchoolIDData schoolIDData = null;
        public UniversityIDData universityIDData = null;
        public ArmyIDData armyIDData = null;
        public PensionerIDData pensionerIDData = null;
    }
