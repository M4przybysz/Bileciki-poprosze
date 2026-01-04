using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject pauseMenuContainer;
    [SerializeField] GameObject settingsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideUIElement(pauseMenuContainer);
        HideUIElement(settingsMenu);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void HandleInputs()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(settingsMenu.activeSelf) { HideUIElement(settingsMenu); }
            else if(pauseMenuContainer.activeSelf) { HideUIElement(pauseMenuContainer); }
            else { ShowUIElement(pauseMenuContainer); } 
        }
    }

    public void ShowUIElement(GameObject UIElement) { 
        if(UIElement == pauseMenuContainer) 
        { 
            Time.timeScale = 0;
            player.isGamePaused = true; 
        }
        UIElement.SetActive(true); 
    }

    public void HideUIElement(GameObject UIElement) { 
        if(UIElement == pauseMenuContainer) 
        { 
            Time.timeScale = 1;
            player.isGamePaused = false; 
        }
        UIElement.SetActive(false); 
    }

    public void SaveAndQuit()
    {
        GameManager.SaveGameData();
        SceneManager.LoadScene("TitleScreen");
    }
}
