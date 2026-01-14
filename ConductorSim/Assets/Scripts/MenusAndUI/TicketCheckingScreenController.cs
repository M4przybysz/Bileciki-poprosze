using TMPro;
using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    // External elements
    [SerializeField] Train train;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject ticketCheckingScreenContainer;
    [SerializeField] TextMeshProUGUI NPCTextBox;
    [SerializeField] GameObject ticket, schoolID, universityID, personalID, armyID, pensionerID;
    [SerializeField] GameObject GetTicketsButton, GetDocumentsButton, GoodDocumentsButton, BadDocumentsButton, GoodbyeButton;

    // Passenger and their documents data
    static Passenger targetPassenger;
    public static TicketData ticketData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideDocuments();
        HideTicketCheckingScreen();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ShowTicketCheckingScreen(Passenger passenger) 
    { 
        ticketCheckingScreenContainer.SetActive(true); 
        player.isInConversation = true;

        PullPassengerData(passenger);
        
        if(targetPassenger.isChecked) { GoodbyeButton.SetActive(true); }
        else { GetTicketsButton.SetActive(true); }
    }

    public void HideTicketCheckingScreen() 
    {
        GoodbyeButton.SetActive(false);
        HideDocuments();
        ticketCheckingScreenContainer.SetActive(false); 
        player.isInConversation = false;
    }

    public void ShowTicket() 
    { 
        GetTicketsButton.SetActive(false);
        ticket.SetActive(true); 

        if(targetPassenger.armyIDData != null || targetPassenger.pensionerIDData != null || targetPassenger.schoolIDData != null || targetPassenger.universityIDData != null)
        {
            GetDocumentsButton.SetActive(true);
        }
        else
        {
            GoodDocumentsButton.SetActive(true);
            BadDocumentsButton.SetActive(true);
        }
    }

    public void ShowAllPossessedDocuments()
    {
        GetDocumentsButton.SetActive(false);

        GoodDocumentsButton.SetActive(true);
        BadDocumentsButton.SetActive(true);

        if(targetPassenger.armyIDData != null) { ShowDocument(armyID); }
        if(targetPassenger.pensionerIDData != null) { ShowDocument(pensionerID); ShowDocument(personalID); }
        if(targetPassenger.schoolIDData != null) { ShowDocument(schoolID); }
        if(targetPassenger.universityIDData != null) { ShowDocument(universityID); }
    }

    static public void ShowDocument(GameObject document) { document.SetActive(true); }

    void PullPassengerData(Passenger passenger)
    {
        targetPassenger = passenger;
        ticketData = targetPassenger.ticketData;

        ticket.GetComponent<Ticket>().SetTicketText(targetPassenger.ticketData);
        schoolID.GetComponent<SchoolID>().SetSchoolIDText(targetPassenger.schoolIDData);
        universityID.GetComponent<UniversityID>().SetUniversityIDText(targetPassenger.universityIDData);
        personalID.GetComponent<PersonalID>().SetPersonalIDText(targetPassenger.personalIDData);
        armyID.GetComponent<ArmyID>().SetArmyIDText(targetPassenger.armyIDData);
        pensionerID.GetComponent<PensionerID>().SetPensionerIDText(targetPassenger.pensionerIDData);
    }

    void HideDocuments()
    {
        ticket.GetComponent<Ticket>().ResetPosition();
        schoolID.GetComponent<SchoolID>().ResetPosition();
        universityID.GetComponent<UniversityID>().ResetPosition();
        personalID.GetComponent<PersonalID>().ResetPosition();
        armyID.GetComponent<ArmyID>().ResetPosition();
        pensionerID.GetComponent<PensionerID>().ResetPosition();

        ticket.SetActive(false);
        schoolID.SetActive(false);
        universityID.SetActive(false);
        personalID.SetActive(false); 
        armyID.SetActive(false);
        pensionerID.SetActive(false);
    }

    public void DocumentsAreFine()
    {
        GoodDocumentsButton.SetActive(false);
        BadDocumentsButton.SetActive(false);

        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Problematic) { train.mistakesCounter += 1; }

        GoodbyeButton.SetActive(true);
    }

    public void DocumentsAreFake()
    {
        GoodDocumentsButton.SetActive(false);
        BadDocumentsButton.SetActive(false);

        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Normal) { train.mistakesCounter += 1; }

        GoodbyeButton.SetActive(true);
    }
}
