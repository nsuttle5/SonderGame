using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private float pickupTime = 2f;
    [SerializeField] private float decreaseDelay = 0.5f;
    [SerializeField] private float decreaseSpeed = 1f;

    [Header("Progress Bar Settings")]
    [SerializeField] private Vector3 barOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private float barWidth = 1f;
    [SerializeField] private float barHeight = 0.1f;
    [SerializeField] private Color barColor = Color.green;
    [SerializeField] private Color backgroundColor = Color.gray;

    private GameObject player;
    private float currentProgress = 0f;
    private bool isPlayerInRange = false;
    private float timeSinceLeftRange = 0f;

    private GameObject progressBarObj;
    private GameObject backgroundBarObj;

    void Start()
    {
        // Find the player (assumes player has "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogWarning("ItemPickup: No player found with 'Player' tag!");
        }

        CreateProgressBar();
    }

    void Update()
    {
        if (player == null) return;

        // Check if player is in range
        float distance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerInRange = distance <= pickupRadius;

        // Show/hide progress bar
        if (backgroundBarObj != null)
        {
            backgroundBarObj.SetActive(isPlayerInRange || currentProgress > 0f);
            progressBarObj.SetActive(isPlayerInRange || currentProgress > 0f);
        }

        // Handle pickup progress
        if (isPlayerInRange)
        {
            // Player is in range - increase progress
            timeSinceLeftRange = 0f;
            currentProgress += Time.deltaTime / pickupTime;

            // Check if pickup is complete
            if (currentProgress >= 1f)
            {
                PickupItem();
                return;
            }
        }
        else
        {
            // Player left the range
            if (currentProgress > 0f)
            {
                timeSinceLeftRange += Time.deltaTime;

                // Start decreasing after delay
                if (timeSinceLeftRange >= decreaseDelay)
                {
                    currentProgress -= Time.deltaTime * decreaseSpeed / pickupTime;
                    currentProgress = Mathf.Max(0f, currentProgress);
                }
            }
        }

        // Update progress bar fill
        UpdateProgressBar();
    }

    private void CreateProgressBar()
    {
        // Create background bar using a sprite renderer
        backgroundBarObj = new GameObject("ProgressBarBackground");
        backgroundBarObj.transform.SetParent(transform);
        backgroundBarObj.transform.localPosition = barOffset;
        
        SpriteRenderer bgRenderer = backgroundBarObj.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = CreateSquareSprite();
        bgRenderer.color = backgroundColor;
        bgRenderer.sortingOrder = 10;
        backgroundBarObj.transform.localScale = new Vector3(barWidth, barHeight, 1f);

        // Create progress bar (fill)
        progressBarObj = new GameObject("ProgressBarFill");
        progressBarObj.transform.SetParent(backgroundBarObj.transform);
        progressBarObj.transform.localPosition = new Vector3(-0.5f, 0, 0);
        
        SpriteRenderer fillRenderer = progressBarObj.AddComponent<SpriteRenderer>();
        fillRenderer.sprite = CreateSquareSprite();
        fillRenderer.color = barColor;
        fillRenderer.sortingOrder = 11;
        progressBarObj.transform.localScale = new Vector3(0f, 1f, 1f);

        backgroundBarObj.SetActive(false);
        progressBarObj.SetActive(false);
    }

    private Sprite CreateSquareSprite()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }

    private void UpdateProgressBar()
    {
        if (progressBarObj != null)
        {
            // Update the scale to show progress
            progressBarObj.transform.localScale = new Vector3(currentProgress, 1f, 1f);
            progressBarObj.transform.localPosition = new Vector3(-0.5f + (currentProgress * 0.5f), 0, 0);
        }
    }

    private void PickupItem()
    {
        Debug.Log($"Picked up item: {gameObject.name}");
        
        // TODO: Add to inventory here
        
        Destroy(gameObject);
    }

    // Visualize pickup radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
