using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float timeRemaining = 10f;
    [SerializeField]
    private bool timerRunning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time's up!");
                timerRunning = false;
            }
        }

    }
}
