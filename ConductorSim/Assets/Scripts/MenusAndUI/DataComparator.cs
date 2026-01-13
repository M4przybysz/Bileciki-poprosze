using System;
using System.Linq;
using TMPro;
using UnityEngine;

public enum TargetBookInfo {None, TrainRoute, Class, Tariff, Date, Series, Number, PriceAndPTU};
public enum TicketInfoType {None, TicketOffice, SingleStation, NumberOfStations, Class, Tariff, Date, Series, Number, PTU, Price}

public class DataComparator : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================

    // External elements
    [SerializeField] TicketCheckingScreenController ticketCheckingScreen;
    [SerializeField] Train train;
    [SerializeField] GameObject ConductorTextBox;

    // Data references
    static readonly string[] possibleComparisonOutcomes = {"Co niby mam porównać?", "Jest w porządku.", "Nie zgadza się."};

    // Comparison variables
    TicketInfoType ticketInfoType = TicketInfoType.None;
    string ticketInfoString;

    // Textbox timer
    float textBoxTimer = 0; // Time in seconds
    const float maxTextBoxTime = 1.5f;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Update is called once per frame
    void Update()
    {
        if(textBoxTimer <= 0 && ConductorTextBox.activeSelf) { ConductorTextBox.SetActive(false); }
        else { textBoxTimer -= Time.deltaTime; }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void SayComparisonOutcome(string comparisonOutcome)
    {
        ConductorTextBox.GetComponentInChildren<TextMeshProUGUI>().text = comparisonOutcome;
        ConductorTextBox.SetActive(true);

        textBoxTimer = maxTextBoxTime;
    }

    public void SetTicketInfoString(TextMeshProUGUI ticketInfoTMP) { ticketInfoString = ticketInfoTMP.text; }

    public void SetTicketInfoType(string ticketInfo) { ticketInfoType = Enum.Parse<TicketInfoType>(ticketInfo); }

    public void SetTargetBookInfo(string bookInfo)
    {
        if(ticketInfoType == TicketInfoType.None) { SayComparisonOutcome(possibleComparisonOutcomes[0]); }
        else { CompareData(ticketInfoString, ticketInfoType, Enum.Parse<TargetBookInfo>(bookInfo)); }
    }

     void CompareData(string dataToCheck, TicketInfoType ticketInfo, TargetBookInfo bookInfo)
    {
        int outcome; // Documents are: 0 == idk, 1 == fine, 2 == not fine

        switch(bookInfo)
        {
            case TargetBookInfo.TrainRoute: { outcome = CheckStation(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.Class: { outcome = CheckClass(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.Tariff: { outcome = CheckTariff(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.Date: { outcome = CheckDate(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.Series: { outcome = CheckTicketSeries(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.Number: { outcome = CheckTicketNumber(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.PriceAndPTU: { outcome = CheckPriceAndPTU(dataToCheck, ticketInfo); break; }
            case TargetBookInfo.None: { outcome = 0; break; }
            default: { outcome = 0; break; }
        }

        ticketInfoString = "";
        ticketInfoType = TicketInfoType.None;

        SayComparisonOutcome(possibleComparisonOutcomes[outcome]);
    }

    static int CheckStation(string dataToCheck, TicketInfoType ticketInfo)
    {
        switch (ticketInfo)
        {
            case TicketInfoType.SingleStation:
            {
                if(TicketData.stations.Contains(dataToCheck)) { return 1; }
                else { return 2; }
            }
            case TicketInfoType.NumberOfStations:
            {
                TicketData ticket = TicketCheckingScreenController.ticketData;

                if((Array.FindIndex(TicketData.stations, station => station.Contains(ticket.stacjaDo)) - Array.FindIndex(TicketData.stations, station => station.Contains(ticket.stacjaOd))).ToString() == dataToCheck) { return 1; }
                else { return 2; }
            }
            case TicketInfoType.TicketOffice:
            {
                string officeCity = dataToCheck[..dataToCheck.IndexOf("\n")];
                bool isCityFound = false;

                foreach(string station in TicketData.stations)
                {
                    isCityFound = station.Contains(officeCity);
                    if(isCityFound) { break; }
                }

                if(isCityFound) { return 1; }
                else { return 2; }
            }
            default: { return 0; }
        }
    }

    static int CheckClass(string dataToCheck, TicketInfoType ticketInfo)
    {
        if(ticketInfo != TicketInfoType.Class) { return 0; }
        else
        {
            if(dataToCheck == "1" || dataToCheck == "2") { return 1; }
            else { return 2; }
        }
    }

    static int CheckTariff(string dataToCheck, TicketInfoType ticketInfo)
    {
        if(ticketInfo != TicketInfoType.Tariff) { return 0; }
        else
        {
            if(TicketData.tariffCodes.Contains(dataToCheck)) { return 1; }
            else { return 2; }
        }
    }

    static int CheckDate(string dataToCheck, TicketInfoType ticketInfo)
    {
        if(ticketInfo != TicketInfoType.Date) { return 0; }
        else
        {
            if(GameManager.currentDateTime.ToString("dd.MM.yyyy") == dataToCheck || GameManager.currentDateTime.AddDays(1).ToString("dd.MM.yyyy") == dataToCheck || GameManager.currentDateTime.AddDays(-1).ToString("dd.MM.yyyy") == dataToCheck) { return 1; }
            else { return 2; }
        }    
    }

    static int CheckTicketSeries(string dataToCheck, TicketInfoType ticketInfo)
    {
        if(ticketInfo != TicketInfoType.Series) { return 0; }
        else
        {
            if(TicketData.ticketSeries.Contains(dataToCheck)) { return 1; }
            else { return 2; }
        }
    }

    int CheckTicketNumber(string dataToCheck, TicketInfoType ticketInfo)
    {
        if(ticketInfo != TicketInfoType.Number) { return 0; }
        else
        {
            if(train.FindTrainCar(int.Parse(dataToCheck[..2])).FindSeat(int.Parse(dataToCheck.Substring(2, 2)))) { return 1; }
            else { return 2; }
        }
    }

    static int CheckPriceAndPTU(string dataToCheck, TicketInfoType ticketInfo)
    {
        switch (ticketInfo)
        {
            case TicketInfoType.PTU:
            {
                TicketData ticket = TicketCheckingScreenController.ticketData;
                float calculatedPTU;

                if(ticket.klasa == "1") { calculatedPTU = (float)Math.Round(int.Parse(ticket.stacje) * TicketData.priceForClass1 * TicketData.tariffPriceModifier[ticket.taryfa] * TicketData.PTUPriceModifier, 2); }
                else if(ticket.klasa == "2") { calculatedPTU = (float)Math.Round(int.Parse(ticket.stacje) * TicketData.priceForClass2 * TicketData.tariffPriceModifier[ticket.taryfa] * TicketData.PTUPriceModifier, 2); }
                else { calculatedPTU = 0; }

                if(calculatedPTU.ToString() + " zł" == dataToCheck) { return 1; }
                else { return 2; }
            }
            case TicketInfoType.Price:
            {
                TicketData ticket = TicketCheckingScreenController.ticketData;
                float calculatedPrice;

                if(ticket.klasa == "1") { calculatedPrice = (float)Math.Round(int.Parse(ticket.stacje) * TicketData.priceForClass1 * TicketData.tariffPriceModifier[ticket.taryfa] * (1 + TicketData.PTUPriceModifier), 2); }
                else if(ticket.klasa == "2") { calculatedPrice = (float)Math.Round(int.Parse(ticket.stacje) * TicketData.priceForClass2 * TicketData.tariffPriceModifier[ticket.taryfa] * (1 + TicketData.PTUPriceModifier), 2); }
                else { calculatedPrice = 0; }

                if(calculatedPrice.ToString() + " zł" == dataToCheck) { return 1; }
                else { return 2; }
            }
            default: { return 0; }
        }
    }
}
