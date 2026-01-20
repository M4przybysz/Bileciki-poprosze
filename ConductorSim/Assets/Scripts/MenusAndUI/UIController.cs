using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] TextMeshProUGUI WatchHandTMP;

    float minutesCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WatchHandTMP.text = GameManager.currentDateTime.ToString("HH:mm\n------\ndd.MM\nyyyy");
    }

    // Update is called once per frame
    void Update()
    {
        minutesCounter += Time.deltaTime * Train.timeScale;

        if(minutesCounter >= 60)
        {
            GameManager.currentDateTime = GameManager.currentDateTime.AddMinutes(minutesCounter / 60);
            WatchHandTMP.text = GameManager.currentDateTime.ToString("HH:mm\n------\ndd.MM\nyyyy");
            minutesCounter %= 60;
        }
    }

    public void ShowUIElement(GameObject UIElement) { 
        if(!player.isGamePaused) { UIElement.SetActive(true); } 
    }

    public void HideUIElement(GameObject UIElement) { 
        if(!player.isGamePaused) { UIElement.SetActive(false); }  
    }

    public void SkipTime(float time) { minutesCounter += time; }
}
