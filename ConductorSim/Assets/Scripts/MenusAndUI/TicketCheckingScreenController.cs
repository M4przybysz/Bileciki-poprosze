using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    // External elements
    [SerializeField] PlayerController player;
    [SerializeField] GameObject ticketCheckingScreenContainer;
    [SerializeField] GameObject ticket;

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
        ticketCheckingScreenContainer.SetActive(true); 
        player.isInConversation = true;
    }

    public void HideTicketCheckingScreen() { 
        ticketCheckingScreenContainer.SetActive(false); 
        player.isInConversation = false;
    }

    public void ShowTicket() { ticket.SetActive(true); }

    public void PullTicketData(Passenger passenger)
    {
        ticketData = passenger.ticketData;
        ticket.GetComponent<Ticket>().setTicketText(ticketData);
    }
}
