using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
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
            if(pauseMenuContainer.activeSelf) { HideUIElement(pauseMenuContainer); }
            else { ShowUIElement(pauseMenuContainer); } 
        }
    }

    public void ShowUIElement(GameObject UIElement) { UIElement.SetActive(true); }

    public void HideUIElement(GameObject UIElement) { UIElement.SetActive(false); }

    public void SaveAndQuit()
    {
        print("save and quit");
        SceneManager.LoadScene("TitleScreen");
    }
}
