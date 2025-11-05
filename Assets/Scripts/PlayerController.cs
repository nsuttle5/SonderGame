using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Only allow one direction at a time
        if (moveY != 0)
        {
            moveX = 0; // Prioritize vertical movement
        }

        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
    }
}
