using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI stopwatchText;
    private float remainingTime;
    private bool isRunning;
    //public GM gameManager;
    [SerializeField]
    private SocketManager socketManager;


    private void Awake()
    {
        socketManager = FindObjectOfType<SocketManager>();
    }

    void Start()
    {
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }

        stopwatchText = GameObject.Find("StopwatchText").GetComponent<TextMeshProUGUI>();
        isRunning = false; // Timer starts manually
    }

    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateStopwatchText();
            }
            else
            {
                isRunning = false;
                remainingTime = 0;
                UpdateStopwatchText();
                OnTimerEnd();
            }
        }
    }

    void UpdateStopwatchText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

        stopwatchText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeAndStart(float timeInSeconds)
    {
        isRunning = false; // Stop any current timer
        remainingTime = timeInSeconds;
        UpdateStopwatchText(); // Update the UI immediately
        StartTimer();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    void OnTimerEnd()
    {
        Debug.Log("Timer has ended!");
        Time.timeScale = 0;
        if (socketManager != null && socketManager.isConnected)
        {
            if (socketManager.getMySocketId() == ClassicLudoGM.game.turnSocketId)
            {

                 socketManager.socket.Emit("TIME_UP");
            }
        }
        else
        {
            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
        }
        //gameManager.HandleWinner();
    }
}
