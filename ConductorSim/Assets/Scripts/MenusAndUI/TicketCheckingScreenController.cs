using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    // External elements
    [SerializeField] PlayerController player;
    [SerializeField] GameObject TicketCheckingScreenContainer;
    [SerializeField] GameObject Ticket;

    // Ticket data
    public static TicketData ticketData;

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

    public void ShowTicket() { Ticket.SetActive(true); }

    public void PullTicketData(Passenger passenger)
    {
        ticketData = passenger.ticketData;
    }
}
