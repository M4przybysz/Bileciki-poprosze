using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemiesText;

    private bool gameEnded = false;

    void Start()
    {
        endScreenPanel.SetActive(false);
    }

    public void ShowEndScreen(int score, float time, int enemiesKilled)
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f; // pauza gry

        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + time.ToString("F1") + " s";
        enemiesText.text = "Enemies defeated: " + enemiesKilled;

        endScreenPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
