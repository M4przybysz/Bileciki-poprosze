using System;
using UnityEngine;

public enum PassengerType {Special, Normal, Problematic};
public enum PassengerCharacter {Nice, Rude, Talkative, Quiet, TimeWaster};
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
    // [instert documents]
    
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
        private set { age = value; }
    }

    //=====================================================================================================
    // Awake, Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        if (doNotGeneratePassenger) { SetCustomPassenger(); } 
        else
        {
            RandomizePassengerProfile();
            PrintProfile(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void RandomizePassengerProfile()
    {
        // Randomize basic data
        Type = (PassengerType)Enum.GetValues(typeof(PassengerType)).GetValue(UnityEngine.Random.Range(1, 3));
        Character = (PassengerCharacter)Enum.GetValues(typeof(PassengerCharacter)).GetValue(UnityEngine.Random.Range(0, 5));
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

        // Randomize date of birth
        DateOfBirth = new DateTime(
            UnityEngine.Random.Range(1930, GameManager.startingInGameDate.Year), 
            UnityEngine.Random.Range(1, 13),
            UnityEngine.Random.Range(1, 29));

        // Calculate age
        Age = GameManager.startingInGameDate.Year - DateOfBirth.Year;
        if (dateOfBirth.Date > GameManager.startingInGameDate.AddYears(-Age)) { Age -= 1; }

        // Calculate PESEL
        PESEL = GeneratePesel(DateOfBirth, Gender);
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