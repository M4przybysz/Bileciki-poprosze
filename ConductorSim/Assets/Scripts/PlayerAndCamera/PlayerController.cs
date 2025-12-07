using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRigidbody;
    Vector2 input;

    float speed = 3f;
    float speedModifier = DefaultSpeedModifier;
    const float DefaultSpeedModifier = 1f;
    const float SprintSpeedModifier = 1.5f;

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

    void HandleInputs()
    {
        // Movement inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        // Sprint while holding shift
        if(Input.GetKey(KeyCode.LeftShift)) { speedModifier = SprintSpeedModifier; }
        else { speedModifier = DefaultSpeedModifier; }
    }
}
