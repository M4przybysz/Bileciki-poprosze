using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuCotroller : MonoBehaviour
{
    [SerializeField] GameObject newGameConfirm;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject options;
    [SerializeField] Button continueButton;
    [SerializeField] Button newGameButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide subpages
        HideUIElement(newGameConfirm);
        if(!GameManager.showCreditsOnTitle) { HideUIElement(credits); }
        else { ShowUIElement(credits); }
        HideUIElement(options);

        // Allow player to continue playing saved game
        if(GameManager.doesSaveExist) { continueButton.interactable = true; }

        // Unlock and show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowUIElement(GameObject UIElement) { UIElement.SetActive(true); }

    public void HideUIElement(GameObject UIElement) { UIElement.SetActive(false); }

    public void ContinueGame()
    {
        GameManager.loadTrainAndPassengers = true;
        SceneManager.LoadScene("GameScene");
    }

    public void NewGame()
    {
        GameManager.SetDefaultData();
        SceneManager.LoadScene("GameScene");     
    }

    public void NewGameConfirmation()
    {
        if(!GameManager.doesSaveExist) { NewGame(); } // Start new game without confirmation if there's no savefile
        else { ShowUIElement(newGameConfirm); } // Require confirmation if save file exists 
    }

    public void QuitGame()
    {
        GameManager.SaveGameData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
