using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================
    // External elements
    [SerializeField] TicketCheckingScreenController TicketCheckingScreen; 
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    
    // Unity components
    Rigidbody2D playerRigidbody;
    private PlayerFatigue fatigueScript; 
    
    // Passenger interaction variables
    Passenger targetPassenger = null; 

    // Movement consts
    const float sprintSpeedModifier = 1.5f;
    const float defaultSpeedModifier = 1f;

    // Movement variables
    Vector2 input;
    float speed = 3f;
    float speedModifier = defaultSpeedModifier;

    // Action limiters
    public bool isInConversation = false;
    public bool isGamePaused = false;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>(); // Get rigidbody
        fatigueScript = GetComponent<PlayerFatigue>(); // Get player fatigue script
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        // poprawne ustawianie pr�dko�ci Rigidbody2D
        if (playerRigidbody != null)
            playerRigidbody.linearVelocity = input * speed * speedModifier;
    }

    //=====================================================================================================
    // Input handling
    //=====================================================================================================
    void HandleInputs()
    {
        if(!isInConversation && !isGamePaused)
        {
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

            // Starting conversation 
            if (Input.GetKeyDown(KeyCode.F) && targetPassenger != null)
            {
                StartConverstation();
            }   
        }
    }

    //=====================================================================================================
    // Unity events
    //=====================================================================================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PassengerTrigger"))
        {
            targetPassenger = collision.transform.parent.GetComponent<Passenger>();
        }

        if(collision.CompareTag("PlayerSpriteTrigger"))
        {
            playerSprite.sortingOrder = -2;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PassengerTrigger") && targetPassenger == collision.transform.parent.GetComponent<Passenger>()) 
        { 
            targetPassenger = null; 
        }

        if(collision.CompareTag("PlayerSpriteTrigger"))
        {
            playerSprite.sortingOrder = 2;
        }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void StartConverstation()
    {
        print("Starting converstation with " + targetPassenger.FirstName);
        TicketCheckingScreen.ShowTicketCheckingScreen();
        TicketCheckingScreen.PullTicketData(targetPassenger);
    }
}
