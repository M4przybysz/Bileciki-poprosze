using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //=====================================================================================================
    // Variables and constants
    //=====================================================================================================
    // External elements
    [SerializeField] TicketCheckingScreenController TicketCheckingScreen; 
    
    // Unity components
    Rigidbody2D playerRigidbody;
    
    // Passenger interaction variables
    Passenger targetPassenger = null; 

    // Movement variables
    Vector2 input;
    float speed = 3f;
    float speedModifier = DefaultSpeedModifier;
    const float DefaultSpeedModifier = 1f;
    const float SprintSpeedModifier = 1.5f;

    //=====================================================================================================
    // Start and Update
    //=====================================================================================================

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        playerRigidbody.linearVelocity = input * speed * speedModifier;
    }

    //=====================================================================================================
    // Input handling
    //=====================================================================================================
    void HandleInputs()
    {
        // Movement inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        // Sprint while holding shift
        if(Input.GetKey(KeyCode.LeftShift)) { speedModifier = SprintSpeedModifier; }
        else { speedModifier = DefaultSpeedModifier; }

        if(Input.GetKeyDown(KeyCode.F) && targetPassenger != null)
        {
            StartConverstation();
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
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PassengerTrigger") && targetPassenger == collision.transform.parent.GetComponent<Passenger>()) 
        { 
            targetPassenger = null; 
        }
    }

    //=====================================================================================================
    // Custom methods
    //=====================================================================================================
    void StartConverstation()
    {
        print("Starting converstation with " + targetPassenger.FirstName);
        TicketCheckingScreen.ShowUIElement(TicketCheckingScreen.TicketCheckingScreenContainer);
    }
}
