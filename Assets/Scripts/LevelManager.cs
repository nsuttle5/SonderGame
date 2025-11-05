using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Level Items")]
    [SerializeField] private List<ItemPickup> requiredItems = new List<ItemPickup>();
    [SerializeField] private bool autoFindItems = true;

    [Header("Timer Settings")]
    [SerializeField] private float levelTime = 60f;
    [SerializeField] private bool timerEnabled = true;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool autoFindTimerText = true;

    private float currentTime;
    private int itemsRemaining;
    private bool levelComplete = false;
    private bool levelFailed = false;

    void Start()
    {
        // Auto-find timer text if enabled
        if (autoFindTimerText && timerText == null)
        {
            timerText = FindFirstObjectByType<TextMeshProUGUI>();
            if (timerText != null)
            {
                Debug.Log("Timer text automatically found!");
            }
        }

        // Auto-find all ItemPickup objects in the scene if enabled
        if (autoFindItems)
        {
            requiredItems.Clear();
            ItemPickup[] foundItems = FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
            requiredItems.AddRange(foundItems);
        }

        itemsRemaining = requiredItems.Count;
        currentTime = levelTime;

        Debug.Log($"Level started with {itemsRemaining} items to collect and {levelTime} seconds!");
    }

    void Update()
    {
        if (levelComplete || levelFailed) return;

        // Update timer
        if (timerEnabled)
        {
            currentTime -= Time.deltaTime;

            // Update timer UI
            if (timerText != null)
            {
                timerText.text = GetTimerFormatted();
            }

            // Check if time ran out
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                LevelFailed();
            }
        }

        // Check if all items are collected
        CheckItemsCollected();
    }

    private void CheckItemsCollected()
    {
        // Remove null items (items that have been destroyed/picked up)
        requiredItems.RemoveAll(item => item == null);
        
        int currentItemCount = requiredItems.Count;

        // Check if item count changed
        if (currentItemCount != itemsRemaining)
        {
            itemsRemaining = currentItemCount;
            Debug.Log($"Item collected! {itemsRemaining} items remaining.");

            // Check if all items collected
            if (itemsRemaining == 0)
            {
                LevelComplete();
            }
        }
    }

    private void LevelComplete()
    {
        levelComplete = true;
        Debug.Log($"Level Complete! Time remaining: {currentTime:F2} seconds");
        
        // TODO: Trigger win condition, load next level, show UI, etc.
    }

    private void LevelFailed()
    {
        levelFailed = true;
        Debug.Log("Level Failed! Time ran out!");
        
        // TODO: Trigger lose condition, restart level, show UI, etc.
    }

    // Public methods for UI or other systems to access
    public float GetTimeRemaining()
    {
        return currentTime;
    }

    public int GetItemsRemaining()
    {
        return itemsRemaining;
    }

    public bool IsLevelComplete()
    {
        return levelComplete;
    }

    public bool IsLevelFailed()
    {
        return levelFailed;
    }

    public string GetTimerFormatted()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
