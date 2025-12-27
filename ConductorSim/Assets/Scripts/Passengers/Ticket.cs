using UnityEngine;

public class Ticket : DragAndDrop
{
    // All of the buttons
    [SerializeField] GameObject kasaWydaniaButton, klasaButton, przejazdButton, liczbaOsobButton, taryfaButton, waznyWTamButton, 
        waznyDoTamButton, waznyWPowrotButton, waznyDoPowrotButton, stacjaOdButton, stacjaDoButton, przezButton, seriaButton, numerButton, 
        seriaINumerBotton, StacjeButton, PTUButton, CenaButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
