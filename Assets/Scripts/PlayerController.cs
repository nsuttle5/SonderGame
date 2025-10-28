using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    [Header("Rotation Settings")]
    [SerializeField] private bool rotateTowardsMovement = true;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 currentVelocity;

    void Start()
    {
        // Get or add Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Configure Rigidbody2D for top-down movement
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Get input from WASD or Arrow keys
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal movement
        movementInput.Normalize();

        // Handle rotation towards movement direction
        if (rotateTowardsMovement && movementInput.magnitude > 0.1f)
        {
            RotateTowardsMovement();
        }
    }

    void FixedUpdate()
    {
        // Apply movement with smooth acceleration/deceleration
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Calculate target velocity
        Vector2 targetVelocity = movementInput * moveSpeed;

        // Smoothly interpolate current velocity to target velocity
        if (movementInput.magnitude > 0.1f)
        {
            // Accelerate
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply velocity to Rigidbody2D
        rb.linearVelocity = currentVelocity;
    }

    private void RotateTowardsMovement()
    {
        // Calculate the angle based on movement direction
        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
        
        // Adjust angle for sprite orientation (assumes sprite faces right by default)
        angle -= 90f;

        // Create target rotation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Smoothly rotate towards target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Optional: Method to set move speed at runtime
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    // Optional: Get current movement direction
    public Vector2 GetMovementDirection()
    {
        return movementInput;
    }

    // Optional: Check if player is moving
    public bool IsMoving()
    {
        return currentVelocity.magnitude > 0.1f;
    }
}
