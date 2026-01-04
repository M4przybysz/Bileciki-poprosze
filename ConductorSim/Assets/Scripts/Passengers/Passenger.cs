using System;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerType {Special, Normal, Problematic};
public enum PassengerCharacter {Nice, Rude, Talkative, Quiet};
public enum PassengerGender {M, F};

public class Passenger : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================

    // Lists of first and last names
    static string[] femaleFirstNames = {"Maja", "Zofia", "Zuzanna", "Hanna", "Laura", "Julia", "Oliwia", "Pola", "Alicja", "Maria", "Aleksandra", "Barbara", "Magda", "Wanda", "Natalia", "Wiktoria", "Adrianna", "Joanna", "Klaudia", "Olga", "Jagoda", "Karolina", "Paulina", "Patrycja", "Katarzyna", "Dominika", "Martyna", "Marta", "Kinga", "Lena", "Małgorzata", "Mariola", "Anna", "Monika", "Gabriela", "Justyna", "Daria", "Angelika", "Izabela", "Emilia", "Sylwia", "Ewelina", "Nikola", "Ewa", "Sara", "Marcelina", "Aneta", "Anita", "Beata", "Dorota", "Dagmara", "Urszula", "Iwona", "Żaneta", "Malwina", "Wioletta", "Jolanta", "Łucja ", "Sonia", "Mariola", "Matylda", "Renata", "Antonina", "Anastazja", "Róża", "Krystyna", "Jadwiga", "Rozalia", "Teresa", "Inga", "Danuta", "Pamela", "Blanka", "Lucyna", "Melania", "Wanessa", "Grażyna"};
    static string[] femaleLastNames = {"Abramczyk", "Adamiec", "Aleksandrowicz", "Andrzejczak", "Bakuła", "Bęben", "Białecka", "Boguszewska", "Borowska", "Brodka", "Brzezińska", "Ceglińska", "Chmiel", "Cicha", "Cieśla", "Czech", "Czekalska", "Ćwir", "Dąbrowska", "Dobosz", "Dobrzyńska", "Domagała", "Duda", "Fikus", "Frątczak", "Gałecka", "Gawron", "Gołąb", "Godlewska", "Grabarz", "Grochowska", "Jabłońska", "Jagiełło", "Janowska", "Jaskólska", "Jaworska", "Kaczmarek", "Kiełbasa", "Klimczuk", "Kmiecik", "Kołodziej", "Kot", "Kowalska", "Krawczyk", "Kucharczyk", "Kwiatkowska", "Lewandowska", "Lipska", "Łęcka", "Majewska", "Majchrzak", "Małek", "Mazur", "Młynarczyk", "Niewiarowska", "Nowak", "Ochocka", "Okulska", "Orlińska", "Ozorek", "Paliwoda", "Paprocka", "Piwowarczyk", "Pazura", "Piątek", "Piechota", "Puchała", "Rogalska", "Różyczka", "Sarna", "Sikora", "Sławuta", "Stasiuk", "Stępień", "Suta", "Sołtys", "Śliwińska", "Tomczak", "Tyszka", "Walczak", "Wasik", "Wilk", "Wójcik", "Wróblewska", "Zawada", "Zięba", "Żmuda", "Żukowska", "Niedźwiedzka", "Łagoda", "Cychowska"};
    static string[] maleFirstNames = {"Jakub", "Mateusz", "Kacper", "Michał", "Maciej", "Sebastian", "Patryk", "Dawid", "Daniel", "Kamil", "Piotr", "Szymon", "Paweł", "Bartosz", "Bartłomiej", "Damian", "Dominik", "Adrian", "Marcin", "Grzegorz", "Łukasz", "Krzysztof", "Tomasz", "Filip", "Adam", "Karol", "Mikołaj", "Krystian", "Hubert", "Konrad", "Wojciech", "Rafał", "Jan", "Przemysław", "Oskar", "Wiktor", "Arkadiusz", "Aleksander", "Artur", "Robert", "Radosław", "Marek", "Eryk", "Marcel", "Norbert", "Andrzej", "Mariusz", "Maksymilian", "Jacek", "Miłosz", "Dariusz", "Cezary", "Igor", "Błażej", "Gabriel", "Alan", "Stanisław", "Nikodem", "Gracjan", "Albert", "Antoni", "Fabian", "Tobiasz", "Sławomir", "Tymoteusz", "Franciszek", "Kajetan", "Remigiusz", "Kornel", "Julian", "Dorian", "Cyprian", "Witold", "Oliwier", "Beniamin", "Samuel", "Józef", "Tadeusz", "Gerard"};
    static string[] maleLastNames = {"Abramczyk", "Adamiec", "Aleksandrowicz", "Andrzejczak", "Bakuła", "Bober", "Bęben", "Białecki", "Boguszewski", "Borowski", "Brzeziński", "Cegliński", "Chmiel", "Cichy", "Cieśla", "Czech", "Czekalski", "Dąbrowski", "Dobosz", "Dobrzyński", "Domagała", "Fikus", "Frątczak", "Gałecki", "Gawron", "Gołąb", "Grabarz", "Grochowski", "Jabłoński", "Jagiełło", "Janowski", "Jaskólski", "Jaworski", "Kaczmarek", "Klimczuk", "Kmicic", "Kołodziej", "Kot", "Kowalski", "Krawczyk", "Kucharczyk", "Kwiatkowski", "Lewandowski", "Lipski", "Majewski", "Majchrzak", "Małek", "Mazur", "Młynarczyk", "Nowak", "Ochocki", "Okulski", "Orliński", "Ozorek", "Paliwoda", "Paprocki", "Piwowarczyk", "Pazura", "Piątek", "Piechota", "Puchała", "Raskolnikow", "Rogalski", "Rzecki", "Sarna", "Sienkiewicz", "Sikora", "Sławuta", "Stasiuk", "Stępień", "Suta", "Sołtys", "Śliwiński", "Tomczak", "Tyszka", "Walczak", "Wasik", "Wilk", "Wójcik", "Wróblewski", "Zawada", "Zięba", "Żmuda", "Żukowski", "Karpiński", "Wokulski", "Przybył", "Grys", "Kałasa",};

    // Train reference
    static Train train;

    // Default date
    DateTime defaulDateTime = new DateTime(2006, 1, 18);

    // General variables
    [SerializeField] bool doNotGeneratePassenger = false;
    [SerializeField] PassengerData customPassengerData;

    // Passneger's personal info
    PassengerType type;
    PassengerCharacter character;
    PassengerGender gender;
    string firstName, lastName, pesel;
    DateTime dateOfBirth;
    int age;

    // Passenger's documents
    public TicketData ticketData = new TicketData();
    
    //=====================================================================================================
    // Variable encapsulation
    //=====================================================================================================
    public PassengerType Type
    {
        get { return type; }
        private set { type = value; }
    }

    public PassengerCharacter Character
    {
        get { return character; }
        private set { character = value; }
    }

    public PassengerGender Gender
    {
        get { return gender; }
        private set { gender = value; }
    }

    public string FirstName 
    {
        get { return firstName; } 
        private set { firstName = value; }
    }

    public string LastName 
    {
        get { return lastName; } 
        private set { lastName = value; }
    }

    public DateTime DateOfBirth
    {
        get { return dateOfBirth.Date; }
        private set { dateOfBirth = value; }
    }

    public string PESEL
    {
        get { return pesel; }
        private set { pesel = value; }
    }

    public int Age
    {
        get { return age; }
        private set { age = Math.Clamp(value, 4, 100); }
    }

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        train = GameObject.Find("Train").GetComponent<Train>();

        if (doNotGeneratePassenger) { SetCustomPassenger(); } 
        else { RandomizePassengerProfile(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //=====================================================================================================
    // Unity methods
    //=====================================================================================================
    void OnDestroy()
    {
        print($"Passenger {firstName} {lastName} leaves the train, route: {ticketData.stacjaOd} -> {ticketData.stacjaDo}");
        if(train != null) { train.RemovePassenger(gameObject); }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void RandomizePassengerProfile()
    {
        // Randomize basic data
        if(UnityEngine.Random.Range(1, 11) > 2) { Type = PassengerType.Normal; }
        else { Type = PassengerType.Problematic; }
        
        Character = (PassengerCharacter)Enum.GetValues(typeof(PassengerCharacter)).GetValue(UnityEngine.Random.Range(0, 4));
        Gender = (PassengerGender)Enum.GetValues(typeof(PassengerGender)).GetValue(UnityEngine.Random.Range(0, 2));
        if (Gender == PassengerGender.F)
        {
            FirstName = femaleFirstNames[UnityEngine.Random.Range(0, femaleFirstNames.Length)];
            LastName = femaleLastNames[UnityEngine.Random.Range(0, femaleLastNames.Length)];
        }
        else
        {
            FirstName = maleFirstNames[UnityEngine.Random.Range(0, maleFirstNames.Length)];
            LastName = maleLastNames[UnityEngine.Random.Range(0, maleLastNames.Length)];
        }

        // Randomize date of birth and calculate age
        DateOfBirth = new DateTime(
            UnityEngine.Random.Range(1930, GameManager.startingInGameDate.Year - 12), 
            UnityEngine.Random.Range(1, 13),
            UnityEngine.Random.Range(1, 29));  

        Age = GameManager.startingInGameDate.Year - DateOfBirth.Year;
        if (dateOfBirth.Date > GameManager.startingInGameDate.AddYears(-Age)) { Age -= 1; } 

        // Calculate PESEL
        PESEL = GeneratePesel(DateOfBirth, Gender);

        PrintProfile();

        GenerateTicket();
    }

    void SetCustomPassenger()
    {
        // Set basic data
        Type = PassengerType.Special;
        Character = customPassengerData.character;
        Gender = customPassengerData.gender;
        FirstName = customPassengerData.firstName;
        LastName = customPassengerData.lastName;

        // Set date of birth
        DateOfBirth = new DateTime(
            Math.Clamp(customPassengerData.yearOfBirth, 1930, GameManager.startingInGameDate.Year), 
            Math.Clamp(customPassengerData.monthOfBirth, 1, 12), 
            Math.Clamp(customPassengerData.dayOfBirth, 1, 28));

        // Calculate age
        Age = GameManager.startingInGameDate.Year - DateOfBirth.Year;
        if (dateOfBirth.Date > GameManager.startingInGameDate.AddYears(-Age)) { Age -= 1; }

        // Calculate PESEL
        PESEL = GeneratePesel(DateOfBirth, Gender);

        PrintProfile();

        GenerateTicket();
    }

    string GeneratePesel(DateTime date, PassengerGender gender)
    {
        // Set year, month and day of birth
        string peselYear = (date.Year % 100).ToString();
        peselYear = (peselYear.Length == 1) ? "0" + peselYear : peselYear;
        string peselMonth = (date.Month + 60).ToString();
        string peselDay = date.Day.ToString();
        peselDay = (peselDay.Length == 1) ? "0" + peselDay : peselDay;

        // Generate random filler number from 000 to 999
        string peselRandomNumber = UnityEngine.Random.Range(0, 1000).ToString();
        peselRandomNumber = (peselRandomNumber.Length == 1) ? "00" + peselRandomNumber : (peselRandomNumber.Length == 2) ? "0" + peselRandomNumber : peselRandomNumber;
        
        // Generate gender number
        int[] peselGenderF = {0, 2, 4, 6, 8}, peselGenderM = {1, 3, 5, 7, 9};
        string peselGender = (gender == PassengerGender.M) ? peselGenderM[UnityEngine.Random.Range(0, 5)].ToString() : peselGenderF[UnityEngine.Random.Range(0, 5)].ToString(); 

        // Combine all data + random control number (1-9)
        string pesel = peselYear + peselMonth + peselDay + peselRandomNumber + peselGender + UnityEngine.Random.Range(1, 10).ToString();
        
        return pesel;
    }

    void PrintProfile()
    {
        print($"Created new Passenger:\n  - Type: {Type};\n  - Character: {Character};\n  - Gender: {Gender};\n  - Name: {FirstName} {LastName};\n  - Date of birth: {DateOfBirth.Date};\n  - Age: {Age};\n  - PESEL: {PESEL}");
    }

    void GenerateTicket()
    {
        // Constant values
        ticketData.przejazd = "PRZEJAZD TAM\nPOC P";
        ticketData.waznyWPowrot = "--.--.----";
        ticketData.waznyDoPowrot = "--.--.----";
        ticketData.liczbaOsob = "1";

        // Randomize stations
        int from = train.currentStationNumber, 
            to = UnityEngine.Random.Range(from + 1, TicketData.numberOfStartStations + 1), 
            through = UnityEngine.Random.Range(from, to); 
        
        ticketData.stacjaOd = TicketData.stations[from]; // Start staion
        ticketData.stacjaDo = TicketData.stations[to]; // End station
        ticketData.stacjaPrzez = TicketData.stations[through]; // Through station

        ticketData.stacje = (to - from).ToString();

        // Set travel date
        DateTime date = (GameManager.currentDateTime.Year != 1) ? GameManager.currentDateTime : defaulDateTime;
        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            // Set the travel date as today and expiration date 1 day later
            ticketData.waznyWTam = date.ToString("dd.MM.yyyy");
            date = date.AddDays(1);
            ticketData.waznyDoTam = date.ToString("dd.MM.yyyy");
        }
        else
        {
            // Set today as expiration date
            ticketData.waznyDoTam = date.ToString("dd.MM.yyyy");
            date = date.AddDays(-1);
            ticketData.waznyWTam = date.ToString("dd.MM.yyyy");
        }

        // Set ticket office
        date = date.AddDays(UnityEngine.Random.Range(-6, 1));
        ticketData.kasaWydania = TicketData.kasyWydania[ticketData.stacjaOd] + date.ToString("dd.MM.yyyy");

        // Sarch for a free seat 
        bool isSeatFound = false;
        int loopBreaker = 0;
        do
        {
            // Randomize class
            ticketData.klasa = UnityEngine.Random.Range(1, 3).ToString();

            // Calculate ticket car number and seat number
            if(ticketData.klasa == "1")
            {
                ticketData.carNumber = UnityEngine.Random.Range(1, 3); // 2 cars for the first class
                ticketData.seatNumber = UnityEngine.Random.Range(1, 7) + 10 * UnityEngine.Random.Range(0, 5);
            }
            else
            {
                ticketData.carNumber = UnityEngine.Random.Range(4, 7); // 3 cars for the second class
                ticketData.seatNumber = UnityEngine.Random.Range(1, 9) + 10 * UnityEngine.Random.Range(0, 5);
            }

            // Check if seat is not taken and take it if it's free
            PassengerSeat chosenSeat = train.FindTrainCar(ticketData.carNumber).FindSeat(ticketData.seatNumber);
            if(chosenSeat != null && !chosenSeat.isTaken) 
            { 
                transform.position = chosenSeat.seatPosition;
                chosenSeat.isTaken = true;
                isSeatFound = true; 
            }

            loopBreaker += 1;
        } 
        while(!isSeatFound && loopBreaker < 50);

        if(loopBreaker >= 50) { Destroy(gameObject); } // Cancel passenger's life subscription if they can't find a seat

        // Randomize tariff
        if (ticketData.klasa == "2")
        {
            ticketData.taryfa = TicketData.tariffCodes[UnityEngine.Random.Range(0, 5)];
            while(Age < TicketData.tariffMinAge[ticketData.taryfa] || Age > TicketData.tariffMaxAge[ticketData.taryfa])
            {
                ticketData.taryfa = TicketData.tariffCodes[UnityEngine.Random.Range(0, 5)];
            }
        }
        else
        {
            if(Age >= 60) { ticketData.taryfa = "E"; }
            else { ticketData.taryfa = "N"; }
        }

        // Calcluate ticket price
        if(ticketData.klasa == "1")
        {
            float price = int.Parse(ticketData.stacje) * TicketData.priceForClass1;
            if(ticketData.taryfa == "E") { price *= TicketData.tariffPriceModifier[ticketData.taryfa]; }
            
            ticketData.PTU = Math.Round(price * TicketData.PTUPriceModifier, 2).ToString() + " zł";
            price *= 1 + TicketData.PTUPriceModifier;

            ticketData.cena = Math.Round(price, 2).ToString() + " zł";
        }
        else
        {
            float price = int.Parse(ticketData.stacje) * TicketData.priceForClass2 * TicketData.tariffPriceModifier[ticketData.taryfa];

            ticketData.PTU = Math.Round(price * TicketData.PTUPriceModifier, 2).ToString() + " zł";
            price *= 1 + TicketData.PTUPriceModifier;

            ticketData.cena = Math.Round(price, 2).ToString() + " zł";
        }

        // Create a ticket series and number
        ticketData.seria = TicketData.ticketSeries[UnityEngine.Random.Range(0, 3)];
        ticketData.numer = "0" + ticketData.carNumber.ToString() + ticketData.seatNumber.ToString();

        string controlNumber = UnityEngine.Random.Range(0, 100000).ToString();
        while (controlNumber.Length < 5) { controlNumber = "0" + controlNumber; }

        ticketData.numer += controlNumber;
        ticketData.seriaINumer = ticketData.seria + ticketData.numer;
    }

    //=====================================================================================================
    // Custom classes
    //=====================================================================================================
    [Serializable] class PassengerData
    {
        public PassengerCharacter character; 
        public PassengerGender gender;
        public string firstName;
        public string lastName;
        [Range(1930, GameManager.startingInGameYear)] public int yearOfBirth; 
        [Range(1, 12)]public int monthOfBirth; 
        [Range(1, 28)]public int dayOfBirth;
    }
}

// Class responsible for storing the data of each ticket and static information used to generate tickets
public class TicketData 
{
    // Reference values
    public const int numberOfStartStations = 9;
    public const float priceForClass2 = 5.5f, priceForClass1 = 9.25f, PTUPriceModifier = 0.07f;
    public static readonly string[] stations = {"Rzeszów Główny", "Stalowa Wola Rozwadów", "Lublin Główny", "Warszawa Centralna", 
        "Łowicz Główny", "Włocławek", "Toruń Główny", "Bydgoszcz Główna", "Piła Główna", "Kołobrzeg"};
    public static Dictionary<string, string> kasyWydania = new Dictionary<string, string>
    {
        ["Rzeszów Główny"] = "Rzeszów\n\n12345789\n",
        ["Stalowa Wola Rozwadów"] = "Stalowa Wola\n\n192837465\n",
        ["Lublin Główny"] = "Lublin\n\n98754321\n",
        ["Warszawa Centralna"] = "Warszawa\n\n91827345\n",
        ["Łowicz Główny"] = "Łowicz\n\n101010101\n",
        ["Włocławek"] = "Włocławek\n\n111111111\n",
        ["Toruń Główny"] = "Toruń\n\n123454321\n",
        ["Bydgoszcz Główna"] = "Bydgoszcz\n\n98756789\n",
        ["Piła Główna"] = "Piła\n\n546372819\n",
    };
    public static readonly string[] ticketSeries = {"A", "B", "C"};
    public static readonly string[] tariffCodes = {"N", "S", "D", "E", "Z"};
    public readonly static Dictionary<string, float> tariffPriceModifier = new Dictionary<string, float>
        { ["N"] = 1f, ["S"] = 0.49f, ["D"] = 0.63f, ["E"] = 0.7f, ["Z"] = 0.22f, };
    public readonly static Dictionary<string, int> tariffMinAge = new Dictionary<string, int>
        { ["N"] = 0, ["S"] = 19, ["D"] = 4, ["E"] = 60, ["Z"] = 18, };
    public readonly static Dictionary<string, int> tariffMaxAge = new Dictionary<string, int>
        { ["N"] = 1000, ["S"] = 26, ["D"] = 18, ["E"] = 1000, ["Z"] = 63, };

    // Data varaibles
    public int carNumber, seatNumber;
    public string kasaWydania, klasa, przejazd, liczbaOsob, taryfa, waznyWTam, waznyDoTam, waznyWPowrot, 
        waznyDoPowrot, stacjaOd, stacjaDo, stacjaPrzez, seria, numer, seriaINumer, stacje, PTU, cena;
}