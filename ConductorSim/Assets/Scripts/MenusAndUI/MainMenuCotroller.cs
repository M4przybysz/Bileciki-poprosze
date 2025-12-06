using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuCotroller : MonoBehaviour
{
    [SerializeField] GameObject newGameConfirm;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject options;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideUIElement(newGameConfirm);
        HideUIElement(credits);
        HideUIElement(options);

        // Unlock and show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowUIElement(GameObject UIElement) { UIElement.SetActive(true); }

    public void HideUIElement(GameObject UIElement) { UIElement.SetActive(false); }

    public void ContinueGame()
    {
        print("continue game");
        SceneManager.LoadScene("GameScene");
    }

    public void NewGame()
    {
        print("new game");
        SceneManager.LoadScene("GameScene");     
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
