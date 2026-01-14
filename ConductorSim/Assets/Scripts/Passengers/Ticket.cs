using TMPro;
using UnityEngine;

public class Ticket : DragAndDrop
{
    // All of the buttons
    [SerializeField] GameObject kasaWydaniaButton, klasaButton, przejazdButton, liczbaOsobButton, taryfaButton, waznyWTamButton, 
        waznyDoTamButton, waznyWPowrotButton, waznyDoPowrotButton, stacjaOdButton, stacjaDoButton, stacjaPrzezButton, seriaButton, numerButton, 
        seriaINumerBotton, StacjeButton, PTUButton, CenaButton;

    static readonly Vector2 startPosition = new(0, -170);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start() { base.Start(); }

    public void SetTicketText(TicketData ticketData)
    {
        kasaWydaniaButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.kasaWydania;
        klasaButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.klasa;
        przejazdButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.przejazd;
        liczbaOsobButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.liczbaOsob;
        taryfaButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.taryfa;
        waznyWTamButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.waznyWTam;
        waznyDoTamButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.waznyDoTam;
        waznyWPowrotButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.waznyWPowrot;
        waznyDoPowrotButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.waznyDoPowrot;
        stacjaOdButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.stacjaOd;
        stacjaDoButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.stacjaDo;
        stacjaPrzezButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.stacjaPrzez;
        seriaButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.seria;
        numerButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.numer;
        seriaINumerBotton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.seriaINumer;
        StacjeButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.stacje;
        PTUButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.PTU;
        CenaButton.GetComponentInChildren<TextMeshProUGUI>().text = ticketData.cena;
    }

    public void ResetPosition() { GetComponent<RectTransform>().anchoredPosition = startPosition; }
}
