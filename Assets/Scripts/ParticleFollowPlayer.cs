using UnityEngine;

public class ParticleFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator playerAnimator;

    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (playerSpriteRenderer != null && leftPosition != null && rightPosition != null)
        {
            // Check if player is running
            bool isRunning = false;
            if (playerAnimator != null)
            {
                isRunning = playerAnimator.GetBool("IsRunning");
            }

            // Control particle emission based on IsRunning
            if (particleSystem != null)
            {
                if (isRunning && !particleSystem.isPlaying)
                {
                    particleSystem.Play();
                }
                else if (!isRunning && particleSystem.isPlaying)
                {
                    particleSystem.Stop();
                }
            }

            // Move particle effect to the appropriate position based on sprite flip
            if (playerSpriteRenderer.flipX)
            {
                // Player is facing left
                transform.position = leftPosition.position;
                transform.localScale = new Vector3(1f, 1f, -1f);
            }
            else
            {
                // Player is facing right
                transform.position = rightPosition.position;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
