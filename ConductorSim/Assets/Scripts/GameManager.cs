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
    public static DateTime startingInGameDate = new DateTime(startingInGameYear, 1, 18);

    //========================================================================
    // Game save data
    //========================================================================
    public static float SFXVolume;
    public static float musicVolume;
    public static DateTime currentDateTime;

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

    //========================================================================
    // Managing save data
    //========================================================================
    class SaveData
    {
        public float SFXVolume;
        public float musicVolume;
        public DateTime currentDateTime;
    }

    public static void SaveGameData()
    {
        SaveData data = new();

        // Set save data
        data.SFXVolume = SFXVolume;
        data.musicVolume = musicVolume;
        data.currentDateTime = currentDateTime;

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
            currentDateTime = data.currentDateTime;
        }
        else
        {
            SetDefaultData();
        }
    }

    public static void SetDefaultData()
    {
        SFXVolume = 0.5f;
        musicVolume = 0.5f;
        currentDateTime = startingInGameDate;
    }
}
