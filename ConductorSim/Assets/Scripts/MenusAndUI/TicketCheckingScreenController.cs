using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    public GameObject TicketCheckingScreenContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideUIElement(TicketCheckingScreenContainer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUIElement(GameObject UIElement) { UIElement.SetActive(true); }

    public void HideUIElement(GameObject UIElement) { UIElement.SetActive(false); }
}
