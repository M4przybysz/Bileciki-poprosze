using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;

    // General variables
    public static bool doesSaveExist {get; private set;}

    // Constant game data
    public const int startingInGameYear = 2006;
    public static DateTime startingInGameDate = new(startingInGameYear, 1, 18);
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetPlayerDataToSave()
    {
        if(GameObject.Find("Player").TryGetComponent<PlayerController>(out var player))
        {
            playerPosition = player.transform.position;
            playerWallet = player.GetWallet();   
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
        }
        else
        {
            SetDefaultData();
        }
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
