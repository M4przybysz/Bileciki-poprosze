using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuCotroller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Unlock and show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

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
