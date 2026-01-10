using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUIElement(GameObject UIElement) { 
        if(!player.isGamePaused) { UIElement.SetActive(true); } 
    }

    public void HideUIElement(GameObject UIElement) { 
        if(!player.isGamePaused) { UIElement.SetActive(false); }  
    }
}
