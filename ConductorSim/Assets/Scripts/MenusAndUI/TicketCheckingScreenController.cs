using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    // External elements
    [SerializeField] Train train;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject ticketCheckingScreenContainer;
    [SerializeField] GameObject ticket, schoolID, universityID, personalID, armyID, pensionerID;
    [SerializeField] GameObject[] documentButtons;

    // Passenger and their documents data
    static Passenger targetPassenger;
    public static TicketData ticketData;
    public static SchoolIDData schoolIDData;
    public static UniversityIDData universityIDData;
    public static PersonalIDData personalIDData;
    public static ArmyIDData armyIDData;
    public static PensionerIDData pensionerIDData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideTicketCheckingScreen();
        HideDocuments();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTicketCheckingScreen() 
    { 
        ticketCheckingScreenContainer.SetActive(true); 
        player.isInConversation = true;
    }

    public void HideTicketCheckingScreen() 
    {
        

        HideDocuments();
        ticketCheckingScreenContainer.SetActive(false); 
        player.isInConversation = false;
    }

    public void ShowTicket() { ticket.SetActive(true); }
    public void ShowDocument(GameObject document) { document.SetActive(true); }

    public void PullPassengerData(Passenger passenger)
    {
        targetPassenger = passenger;

        ticketData = passenger.ticketData;
        schoolIDData = passenger.schoolIDData;
        universityIDData = passenger.universityIDData;
        personalIDData = passenger.personalIDData;
        armyIDData = passenger.armyIDData;
        pensionerIDData = passenger.pensionerIDData;

        ticket.GetComponent<Ticket>().SetTicketText(ticketData);
        schoolID.GetComponent<SchoolID>().SetSchoolID(schoolIDData);
        universityID.GetComponent<UniversityID>().SetUniversityID(universityIDData);
        personalID.GetComponent<PersonalID>().SetPersonalID(personalIDData);
        armyID.GetComponent<ArmyID>().SetArmyID(armyIDData);
        pensionerID.GetComponent<PensionerID>().SetPensionerID(pensionerIDData);
    }

    void HideDocuments()
    {
        ticket.SetActive(false);
        schoolID.SetActive(false);
        universityID.SetActive(false);
        personalID.SetActive(false); 
        armyID.SetActive(false);
        pensionerID.SetActive(false);
    }

    public void DocumentsAreFine()
    {
        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Problematic) { train.mistakesCounter += 1; }
    }

    public void DocumentsAreFake()
    {
        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Normal) { train.mistakesCounter += 1; }
    }
}
