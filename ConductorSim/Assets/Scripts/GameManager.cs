using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager Instance;

    // Generl variables
    public static bool doesSaveExist {get; private set;}

    //========================================================================
    // Game data
    //========================================================================
    
    // [Insert game data vriables]

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
        // [Insert game data vriables]
    }

    public static void CheckSaveFile()
    {
        doesSaveExist = File.Exists(Application.persistentDataPath + "/savefile.json");
    }

    public static void SaveGameData()
    {
        SaveData data = new();

        // [save game data]

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

            // [Load saved data]
        }
        else
        {
            SetDefaultData();
        }
    }

    static void SetDefaultData()
    {
        // [Set deafult data]
    }
}
