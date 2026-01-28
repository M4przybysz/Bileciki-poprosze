using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================
    // External elements
    [SerializeField] UIController uiController;
    [SerializeField] GameObject UseBedConfirm;
    [SerializeField] Train train;
    [SerializeField] TicketCheckingScreenController TicketCheckingScreen; 
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] AudioSource playerFootsteps;
    
    // Unity components
    Rigidbody2D playerRigidbody;
    private PlayerFatigue fatigueScript; 
    
    // Passenger and round interaction variables
    Passenger targetPassenger = null; 
    bool isByBed = false;

    // Movement consts
    const float sprintSpeedModifier = 1.5f;
    const float defaultSpeedModifier = 1f;

    // Movement variables
    Vector2 input;
    float speed = 2.25f;
    float speedModifier = defaultSpeedModifier;

    // Action limiters
    public bool isInConversation = false;
    public bool isGamePaused = false;

    // Player's wallet
    public const float salary = 84f;
    float wallet;

    // dodane pola do buffu pr�dko�ci (zmiana: zarz�dzanie bez korutyn)
    float speedBuffMultiplier = 1f;
    bool speedBuffActive = false;
    float speedBuffEndTimeUnscaled = 0f;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>(); // Get rigidbody
        fatigueScript = GetComponent<PlayerFatigue>(); // Get player fatigue script

        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        // Zarz�dzanie buffem pr�dko�ci (u�ywamy czasu nie-skalowanego, wi�c dzia�a w pauzie)
        if (speedBuffActive && Time.unscaledTime >= speedBuffEndTimeUnscaled)
        {
            speedBuffMultiplier = 1f;
            speedBuffActive = false;
        }
    }

    void FixedUpdate()
    {
        if (fatigueScript != null && fatigueScript.IsImmobilized)
        {
            if (playerRigidbody != null)
                playerRigidbody.linearVelocity = Vector2.zero;
            return;
        }

        // Uwaga: speedBuffMultiplier wp�ywa multiplicatively na ko�cow� pr�dko��
        playerRigidbody.linearVelocity = input * speed * speedModifier * speedBuffMultiplier;
    }

    //=====================================================================================================
    // Input handling
    //=====================================================================================================
    void HandleInputs()
    {
        if(!isInConversation && !isGamePaused)
        {
            if (fatigueScript != null && fatigueScript.IsImmobilized)
            {
                input = Vector2.zero;
                speedModifier = 0f;
                fatigueScript.SetSprinting(false);
                return;
            }

            // Movement inputs
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            // Set animation based on movement
            playerAnimator.SetFloat("inputX", input.x);
            playerAnimator.SetFloat("inputY", input.y);

            // Sprint while holding shift
            bool sprinting = Input.GetKey(KeyCode.LeftShift);
            fatigueScript.SetSprinting(sprinting);

            // Adjust sprint and fatigue modifiers
            if (sprinting) { speedModifier = sprintSpeedModifier * fatigueScript.GetSpeedModifier(); }
            else {speedModifier = defaultSpeedModifier * fatigueScript.GetSpeedModifier(); }

            // Player Footsteps
            if (input.x != 0f || input.y != 0f)
            {
                if (!playerFootsteps.isPlaying)
                {
                    playerFootsteps.Play();
                }
            }
            else
            {
                playerFootsteps.Stop();
            }

            // Starting conversation 
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isByBed && train.trainState == "ride") { uiController.ShowUIElement(UseBedConfirm); }
                else if (targetPassenger != null) { StartConverstation(); }
            }   
        }
    }

    //=====================================================================================================
    // Unity events
    //=====================================================================================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bed")) { isByBed = true; }

        if(collision.CompareTag("PassengerTrigger"))
        {
            targetPassenger = collision.transform.parent.GetComponent<Passenger>();
        }

        if(collision.CompareTag("PlayerSpriteTrigger")) { playerSprite.sortingOrder = -2; }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Bed")) { isByBed = false; }

        if(collision.CompareTag("PassengerTrigger") && targetPassenger == collision.transform.parent.GetComponent<Passenger>()) 
        { 
            targetPassenger = null; 
        }

        if(collision.CompareTag("PlayerSpriteTrigger")) { playerSprite.sortingOrder = 2; }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void StartConverstation()
    {
        print("Starting converstation with " + targetPassenger.FirstName);
        TicketCheckingScreen.ShowTicketCheckingScreen(targetPassenger);
    }

    public void AddMoneyToWallet(float moneyToAdd) { wallet += moneyToAdd; }

    public void SetWallet(float moneyToSet) { wallet = moneyToSet; }
    public float GetWallet() { return wallet; }
    
    void LoadPlayerData()
    {
        if(GameManager.Instance != null)
        {
            transform.position = GameManager.playerPosition;
            wallet = GameManager.playerWallet;
        }
    }

    // publiczna metoda wywo�ywana przez sklep
    // percent akceptuje warto�ci w formacie u�amkowym (0.25) lub procentowym (25).
    // duration w sekundach.
    public void ApplySpeedBuffPercent(float percent, float duration)
    {
        // obs�u� przypadek gdy projektant poda 25 zamiast 0.25
        if (percent > 2f) percent = percent / 100f;

        // ogranicz rozs�dnie multiplier (np. max +200%)
        percent = Mathf.Clamp(percent, -0.9f, 2f); // -90% do +200%

        float multiplier = 1f + percent;

        // ustaw buff
        speedBuffMultiplier = multiplier;
        speedBuffActive = true;
        speedBuffEndTimeUnscaled = Time.unscaledTime + Mathf.Max(0.01f, duration);

        Debug.Log($"PlayerController: speed buff applied x{multiplier} for {duration}s (unscaled).");
    }
}
