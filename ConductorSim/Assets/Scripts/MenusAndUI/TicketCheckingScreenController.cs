using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TicketCheckingScreenController : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================

    // Dialogue variables and constants
    const float textSpeed = 0.025f; // Time between each char of the dialogue string
    bool isTicketChecking = false; // Is conductor currently checking ticket and/or documents?
    float timeToNextDialogue = 15;

    IEnumerator lastPrinting = null;
    static readonly List<List<string>> startingDialogues = new List<List<string>>()
    {
        /* Nice */ new() {"Dzień dobry!", "Dzień dobry, proszę bardzo.", "Dzień dobry, już wyciągam.", "Dzień dobry Panu, proszę.", "Dzień dobry, to mój bilet.", "Dzień dobry, tu jest mój bilet."},
        /* Rude */ new() {"...", "Proszę się pospieszyć.", "Tylko szybko.", "No nie wiem, czy taki dobry.", "Może dla Pana."},
        /* Talkative */ new() {"Dzień dobry, dzień dobry! Piękna dziś pogoda, czyż nie?", "Dzień dobry! Pan to dzisiaj lewą nogą wstał? Ha, ha.", "Dobry, dobry, oczywiście.", "Tak, tak, już podaję. Niech Pan mówi od razu, jak coś będzie trzeba jeszcze, bo to brat kupował, a on mi nigdy nie powie, co i jak.", "Zawsze z tymi biletami… Tyle co człowiek wszedł! Już Panu wyciągam.", "Musi mi Pan dać chwilkę, bo tak mi siostra schowała, że nie wiem, czy znajdę… O, jest!", "A to zawsze Pan musi sprawdzać? Czasami mi nie sprawdzają. A czasami mówią, że mogę jechać bez biletu."},
        /* Quiet */ new() {"...", "Dobry.", "Mhm."}
    };

    static readonly List<List<string>> randomDialogues = new List<List<string>>()
    {
        /* Nice */ new() {"Czy coś jest nie tak?", "Wszystko w porządku?", "Czegoś brakuje?", "Czyżby czegoś brakowało?", "Czy wszystko się zgadza?"},
        /* Rude */ new() {"Długo jeszcze?", "Coś nie tak?", "Przecież wszystko się zgadza.", "Jakiś problem?", "Może Pan szybciej?", "Wolniej się nie da?"},
        /* Talkative */ new() {"Ale Pan się tu musi nudzić. Ja bym nie dała rady, tak cały dzień się gapić w papierki i sprawdzać.", "Wszyściutko jest, Pan się niczym nie przejmuję. Ja jestem uczciwym człowiekiem i nigdy w życiu nikogo nie oszukałam.", "Jak tam Panu idzie?", "Wszystko się zgadza?", "Tam wszystko jest, proszę Pana, Pan sobie zobaczy.", "Pan to ma dobrze, taką pracę to każdy by chciał. Tylko się chodzi w kółko po pociągu.", "Dużo Pan zarabia? Siostra chce być konduktorem. Albo konduktorką? Ale nie wie, czy są z tego pieniądze.", "Pan się nie spieszy, mam w rodzinie pięciu konduktorów. Wiem, jaka to jest ważna sprawa, żeby wszystkie bilety dobrze sprawdzić. Miga się w oczach po jakimś czasie.", "Proszę się nie spieszyć, dobrze być dokładnym.", "Niedługo wysiadam, może się Pan trochę pospieszyć?", "Tak się zimno gdzieś zrobiło ostatnio, a przecież słoneczko świeci… Z tą pogodą to nigdy nie wiadomo.", "Proszę Pana, a macie coś dobrego w tym Warsie? Podobno schabowy przepyszny, ale nie wiem, czy wierzyć koleżankom.", "Mam nadzieję, że szybko dojedziemy, bo już nie mogę wysiedzieć.", "Pójdę się trochę przejść po pociągu, jak Pan skończy sprawdzać.", "Strasznie niewygodne te fotele. Może Pan porozmawiać z szefem? Może coś z tym zrobi.", "Ale w tym pociągu duszno. Można coś z tym zrobić? Może okno Pan otworzy?", "Ale Pan się tu musi nudzić. Ja bym nie dał rady, tak cały dzień się gapić w papierki i sprawdzać.", "Wszyściutko jest, Pan się niczym nie przejmuję. Ja jestem uczciwym człowiekiem i nigdy w życiu nikogo nie oszukałem."},
        /* Quiet */ new() {"....."}
    };

    static readonly List<List<string>> goodDocumentsDialogues = new List<List<string>>()
    {
        /* Nice */ new() {"Do widzenia!", "Do widzenia.", "Dziękuję, do widzenia!", "Miłego dnia!", "Dziękuję!", "Dziękuję, miłego dnia.", "Dziękuję, Panu również."},
        /* Rude */ new() {"...", "Dłużej się nie dało?", "No wreszcie.", "Mhm."},
        /* Talkative */ new() {"No i świetnie! Szybko, sprawnie i bez problemu, tak właśnie trzeba pracować.", "Ja zawsze mam wszystko przy sobie, wie Pan? Nawet sprawdzać nie trzeba.", "Tak, tak, miłego dnia i do widzenia. Ja tu się zdrzemnę trochę chyba, to mnie prosze obudzić, jakby Pan chciał jeszcze raz sprawdzić.", "Dziękuję Panu. A niech mi Pan powie jeszcze, jest tu jakaś toaleta?", "Do widzenia, do widzenia. I miłej pracy panu życzę, bo z ludźmi nigdy nie wiadomo.", "No i z głowy, co? He, he. To teraz może do Warsu się przejdę po coś dobrego."},
        /* Quiet */ new() {"...", "Mhm.", "Do widzenia.", "Miłego."}
    };

    static readonly List<List<string>> badDocumentsDialogues = new List<List<string>>()
    {
        /* Nice */ new() {"Ojejku, to niemożliwe.", "Och, nie. Przepraszam Pana.", "Och, nie.", "Jejku, głupia sytuacja. Mam coś dopłacić?", "… Faktycznie! Bardzo przepraszam.", "Ma Pan rację, to moja wina. Dopłacić coś za to?"},
        /* Rude */ new() {"To są jakieś żarty?", "Chyba sobie Pan żartujesz.", "Pan sobie żartuje.", "Słucham?", "Niemożliwe.", "Niech Pan sobie nie robi jaj.", "Wy konduktorzy to zawsze z igły widły robicie."},
        /* Talkative */ new() {"Na pewno? Przecież sprawdzałam.", "Jest Pan pewien?", "Może to jakiś błąd?", "Ale co Pan? Przecież tu wszyściutko jest w porządku.", "Chyba się Panu coś pomyliło.", "Ale to niemożliwe, wszystko jest, jak być powinno.", "Słucham? Czy Pan sugeruje, że próbuję go oszukać?", "Pan próbuje mnie oszukać. Napiszę na Pana skargę.", "A może Pan sprawdzić jeszcze raz?", "Niemożliwe, wszystko załatwiane dziś rano, nawet Pani z okienka na dworcu mi pomogła.", "No co Pan mówi? To niemożliwe.", "Na pewno? Przecież sprawdzałem."},
        /* Quiet */ new() {"..", "Okej.", "Mhm.", "… Cholerka."},
    };

    // External elements
    [SerializeField] Train train;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject ticketCheckingScreenContainer;
    [SerializeField] TextMeshProUGUI NPCTextBox;
    [SerializeField] GameObject ticket, schoolID, universityID, personalID, armyID, pensionerID;
    [SerializeField] GameObject GetTicketsButton, GetDocumentsButton, GoodDocumentsButton, BadDocumentsButton, GoodbyeButton;

    // Passenger and documents sounds
    [SerializeField] AudioSource passengerAudioSource;
    [SerializeField] AudioSource documentsAudioSource;
    [SerializeField] AudioClip[] passengerSounds; // 0-18 == male (0-4 == talkative, 5-9 == nice, 10-15 == rude, 16-18 == quiet), 19-39 == female (18-25 == talkative, 26-31 == nice, 32-35 == rude, 36-39 == quiet)
    [SerializeField] AudioClip[] documentsSounds; 

    // Passenger and their documents data
    static Passenger targetPassenger;
    public static TicketData ticketData;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideAllDocuments();
        HideTicketCheckingScreen();
        NPCTextBox.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(isTicketChecking)
        {
            timeToNextDialogue -= Time.deltaTime;

            if(timeToNextDialogue <= 0)
            {
                switch(targetPassenger.Character)
                {
                    case PassengerCharacter.Talkative:
                    {
                        if(targetPassenger.Gender == PassengerGender.M) { PrintLine(randomDialogues[(int)PassengerCharacter.Talkative][Random.Range(2, randomDialogues[(int)PassengerCharacter.Talkative].Count)]); }
                        else { PrintLine(randomDialogues[(int)PassengerCharacter.Talkative][Random.Range(0, randomDialogues[(int)PassengerCharacter.Talkative].Count - 2)]); }
                        break;
                    }
                    default: { PrintLine(randomDialogues[(int)targetPassenger.Character][Random.Range(0, randomDialogues[(int)targetPassenger.Character].Count)]); break; }
                }

                timeToNextDialogue = Random.Range(10, 30);
            }   
        }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================

    public void ShowTicketCheckingScreen(Passenger passenger) 
    { 
        ticketCheckingScreenContainer.SetActive(true); 
        player.isInConversation = true;

        PullPassengerData(passenger);

        // Random passenger voiceline on interaction
        PlayPassengerVoiceline();

        if (targetPassenger.isChecked) 
        {
            PrintLine("Tak?"); 
            GoodbyeButton.SetActive(true); 
        }
        else 
        {
            NPCTextBox.text = ""; 
            GetTicketsButton.SetActive(true); 
            timeToNextDialogue = Random.Range(10, 30);
        }
    }

    public void HideTicketCheckingScreen() 
    {
        if(lastPrinting != null) { StopCoroutine(lastPrinting); }
        NPCTextBox.text = "";

        GoodbyeButton.SetActive(false);
        HideAllDocuments();
        ticketCheckingScreenContainer.SetActive(false); 
        player.isInConversation = false;
    }

    public void ShowTicket() 
    { 
        isTicketChecking = true;
        switch(targetPassenger.Character)
        {
            case PassengerCharacter.Quiet:
            {
                int randInt = Random.Range(0, 100);

                if(randInt < 15) { PrintLine(startingDialogues[(int)PassengerCharacter.Quiet][2]); }
                else if(randInt < 30) { PrintLine(startingDialogues[(int)PassengerCharacter.Quiet][1]); }
                else { PrintLine(startingDialogues[(int)PassengerCharacter.Quiet][0]); }

                break;
            }
            default: { PrintLine(startingDialogues[(int)targetPassenger.Character][Random.Range(0, startingDialogues[(int)targetPassenger.Character].Count)]); break; }
        }

        GetTicketsButton.SetActive(false);
        ticket.SetActive(true); 
        documentsAudioSource.PlayOneShot(documentsSounds[Random.Range(0, documentsSounds.Length)]);

        if(targetPassenger.armyIDData.firstName != "" || targetPassenger.pensionerIDData.firstName != "" || targetPassenger.schoolIDData.name != "" || targetPassenger.universityIDData.name != "")
        {
            GetDocumentsButton.SetActive(true);
        }
        else
        {
            if(targetPassenger.Gender == PassengerGender.M) { BadDocumentsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bilet jest niepoprawny. Musi Pan zapłacić za normalny."; }
            else { BadDocumentsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bilet jest niepoprawny. Musi Pani zapłacić za normalny."; }
            
            GoodDocumentsButton.SetActive(true);
            BadDocumentsButton.SetActive(true);
        }
    }

    public void ShowAllPossessedDocuments()
    {
        GetDocumentsButton.SetActive(false);

        GoodDocumentsButton.SetActive(true);
        BadDocumentsButton.SetActive(true);

        if(targetPassenger.armyIDData.firstName != "") { ShowDocument(armyID); }
        if(targetPassenger.pensionerIDData.firstName != "") { ShowDocument(pensionerID); ShowDocument(personalID); }
        if(targetPassenger.schoolIDData.name != "") { ShowDocument(schoolID); }
        if(targetPassenger.universityIDData.name != "") { ShowDocument(universityID); }

        documentsAudioSource.PlayOneShot(documentsSounds[Random.Range(0, documentsSounds.Length)]);
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

    public void HideAllDocuments()
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

        documentsAudioSource.PlayOneShot(documentsSounds[Random.Range(0, documentsSounds.Length)]);
    }

    public void DocumentsAreFine()
    {
        GoodDocumentsButton.SetActive(false);
        BadDocumentsButton.SetActive(false);

        switch(targetPassenger.Character)
        {
            case PassengerCharacter.Quiet:
            {
                int randInt = Random.Range(0, 100);

                if(randInt < 10) { PrintLine(goodDocumentsDialogues[(int)PassengerCharacter.Quiet][3]); }
                else if(randInt < 20) { PrintLine(goodDocumentsDialogues[(int)PassengerCharacter.Quiet][2]); }
                else if(randInt < 30) { PrintLine(goodDocumentsDialogues[(int)PassengerCharacter.Quiet][1]); }
                else { PrintLine(goodDocumentsDialogues[(int)PassengerCharacter.Quiet][0]); }

                break;
            }
            default: { PrintLine(goodDocumentsDialogues[(int)targetPassenger.Character][Random.Range(0, goodDocumentsDialogues[(int)targetPassenger.Character].Count)]); break; }
        }

        isTicketChecking = false;
        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Problematic) { train.mistakesCounter += 1; }

        GoodbyeButton.SetActive(true);
    }

    public void DocumentsAreFake()
    {
        GoodDocumentsButton.SetActive(false);
        BadDocumentsButton.SetActive(false);

        switch(targetPassenger.Character)
        {
            case PassengerCharacter.Quiet:
            {
                int randInt = Random.Range(0, 100);

                if(randInt < 10) { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Quiet][3]); }
                else if(randInt < 25) { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Quiet][0]); }
                else if(randInt < 45) { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Quiet][2]); }
                else { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Quiet][1]); }

                break;
            }
            case PassengerCharacter.Talkative:
            {
                if(targetPassenger.Gender == PassengerGender.M) { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Talkative][Random.Range(1, badDocumentsDialogues[(int)PassengerCharacter.Talkative].Count)]); }
                else { PrintLine(badDocumentsDialogues[(int)PassengerCharacter.Talkative][Random.Range(0, badDocumentsDialogues[(int)PassengerCharacter.Talkative].Count - 1)]); }

                break;
            }
            default: { PrintLine(badDocumentsDialogues[(int)targetPassenger.Character][Random.Range(0, badDocumentsDialogues[(int)targetPassenger.Character].Count)]); break; }
        }

        isTicketChecking = false;
        targetPassenger.isChecked = true;
        train.checkedPassengersCounter += 1;
        if(targetPassenger.Type == PassengerType.Normal) { train.mistakesCounter += 1; }

        GoodbyeButton.SetActive(true);
    }

    void PrintLine(string line)
    {
        if(lastPrinting != null) { StopCoroutine(lastPrinting); }
        NPCTextBox.text = "";
        
        lastPrinting = TypeLine(line);
        StartCoroutine(lastPrinting);

        PlayPassengerVoiceline();
    }

    IEnumerator TypeLine(string line)
    {
        foreach(char c in line.ToCharArray())
        {
            NPCTextBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void PlayPassengerVoiceline()
    {
        switch (targetPassenger.Gender)
        {
            case PassengerGender.M:
            {
                switch (targetPassenger.Character)
                {
                    case PassengerCharacter.Talkative: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(0, 5)]); break; }
                    case PassengerCharacter.Nice: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(5, 10)]); break; }
                    case PassengerCharacter.Rude: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(10, 16)]); break; }
                    case PassengerCharacter.Quiet: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(16, 19)]); break; }
                    default: { Debug.LogWarning($"Unknown passenger character: {targetPassenger.Character}"); break; }
                }
                break;
            }
            case PassengerGender.F:
            {
                switch (targetPassenger.Character)
                {
                    case PassengerCharacter.Talkative: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(19, 26)]); break; }
                    case PassengerCharacter.Nice: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(26, 32)]); break; }
                    case PassengerCharacter.Rude: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(32, 36)]); break; }
                    case PassengerCharacter.Quiet: { passengerAudioSource.PlayOneShot(passengerSounds[Random.Range(36, 40)]); break; }
                    default: { Debug.LogWarning($"Unknown passenger character: {targetPassenger.Character}"); break; }
                }
                break;
            }
            default: { Debug.LogWarning($"Unknown passenger gender: {targetPassenger.Gender}"); break; }
        }
    }
}
