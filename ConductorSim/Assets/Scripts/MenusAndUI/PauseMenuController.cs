using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject menuContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            if(menuContainer.activeSelf) { HideUIElement(menuContainer); }
            else { ShowUIElement(menuContainer); } 
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
