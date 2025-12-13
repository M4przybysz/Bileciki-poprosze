using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    public GameObject TicketCheckingScreenContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideTicketCheckingScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTicketCheckingScreen() { 
        TicketCheckingScreenContainer.SetActive(true); 
        player.isInConversation = true;
    }

    public void HideTicketCheckingScreen() { 
        TicketCheckingScreenContainer.SetActive(false); 
        player.isInConversation = false;
    }
}
