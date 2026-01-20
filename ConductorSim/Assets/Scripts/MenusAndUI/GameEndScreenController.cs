using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScreenController : MonoBehaviour
{
    [SerializeField] GameObject gameEndScreenContainer;
    [SerializeField] TextMeshProUGUI statisticsTMP;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameEndScreenContainer.SetActive(false);
    }

    public void ShowEndScreen(string gameStatistics)
    {
        Time.timeScale = 0; // Pauste the game

        statisticsTMP.text = gameStatistics;
        gameEndScreenContainer.SetActive(true);
    }

    public void GoToTitleScreen(bool showCredits = false)
    {
        GameManager.showCreditsOnTitle = showCredits;
        SceneManager.LoadScene("TitleScreen");
    }
}
