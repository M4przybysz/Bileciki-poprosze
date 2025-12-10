using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;

    // General variables
    public static bool doesSaveExist {get; private set;}

    //========================================================================
    // Game data
    //========================================================================
    
    public static float SFXVolume;
    public static float musicVolume;

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
    }

    public static void CheckSaveFile()
    {
        doesSaveExist = File.Exists(Application.persistentDataPath + "/savefile.json");
    }

    public static void SaveGameData()
    {
        SaveData data = new();

        data.SFXVolume = SFXVolume;
        data.musicVolume = musicVolume;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            SFXVolume = data.SFXVolume;
            musicVolume = data.musicVolume;
        }
        else
        {
            SetDefaultData();
        }
    }

    static void SetDefaultData()
    {
        SFXVolume = 0.5f;
        musicVolume = 0.5f;
    }
}
