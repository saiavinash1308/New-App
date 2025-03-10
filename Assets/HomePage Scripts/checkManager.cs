using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro Input Fields

public class checkManager : MonoBehaviour
{
    public static checkManager Instance;
    public TextMeshProUGUI statusMessageText;
    public GameObject Blocker;
    private bool isInternetChecked = false; // Flag to stop checking once the internet is available

    // Start is called before the first frame update
    void Awake()
    {
        // Implement Singleton pattern to prevent duplicates
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    void Start()
    {
        CheckInternetConnection();
    }

    // Update is called once per frame
    void Update()
    {
        // Only continue checking if the internet is not already available
        if (!isInternetChecked)
        {
            CheckInternetConnection();
        }
    }

    private void CheckInternetConnection()
    {
        // Check for internet connectivity
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("No internet connection");
            ShowStatusMessage("Please Connect to the internet");
        }
        else
        {
            Debug.LogWarning("Internet connection available");
            ShowStatusMessage(""); // Clear the message
            Blocker.gameObject.SetActive(false);
            isInternetChecked = true; // Stop further checks once the internet is available
        }
    }

    private void ShowStatusMessage(string message)
    {
        statusMessageText.text = message;
        Blocker.gameObject.SetActive(true);
    }
}
