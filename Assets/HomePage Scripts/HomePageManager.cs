using System.Collections;
using UnityEngine;
using TMPro;
using SocketIOClient;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class HomePageManager : MonoBehaviour
{
    private SocketManager socketManager;
    //public Button LudoBtn;
    public Button BackButton; // Add a reference to the Back button

    [SerializeField]
    private GameObject Selectscreen;
    [SerializeField]
    private GameObject LeaderBoard;
    [SerializeField]
    private GameObject ProfilePanel;

    [SerializeField]
    private GameObject currentPanel;
    [SerializeField]
    private GameObject HomePanel;
    [SerializeField]
    private GameObject EditPanel;

    [SerializeField]
    private GameObject LudoLoadingPanel;

    [SerializeField]
    private GameObject  ClassicLudoLoadingPanel;

    [SerializeField]
    private GameObject CricketLoadingPanel;

    [SerializeField]
    private GameObject MindMorgaLoadingPanel;

    public GameObject HomeMark;
    public GameObject WalletMark;
    public GameObject LeaderBoardMark;

    [SerializeField]
    GameFetcherScript gameFetcher;

    //private const string baseUrl = "http://localhost:3000/";
    private const string baseUrl = "https://sockets.fivlog.space";
    public TMP_Text userCountText;

    void Start()
    {
        // Get the SocketManager component
        socketManager = FindObjectOfType<SocketManager>();
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }

        HomeMark.SetActive(true);
        WalletMark.SetActive(false);
        LeaderBoard.SetActive(false);
        //// Set up button listeners
        //LudoBtn.onClick.AddListener(OnFastLudo);
        BackButton.onClick.AddListener(OnBackButtonClicked); // Setup Back button listener
        StartCoroutine(GetUserCount());
    }

    // This is your main "Fast Ludo" button handler
    public void OnFastLudo()
    {
        //SendGameIdToServer("cm2u4y04g0000307b4zpisl76");
        SceneManager.LoadScene("LudoLoadingScene");
        
    }

    public void OnFastLudo2Player()
    {
        SceneManager.LoadScene("LudoLoadingSceneFor2Player");

    }

    // Show Select Screen Panel
    public void OnFastLudoBtn()
    {
        ShowPanel(LudoLoadingPanel);
        if (gameFetcher != null)
        {
            StartCoroutine(gameFetcher.GetGameData("FAST_LUDO"));
        }
        else
        {
            Debug.LogError("GameFetcherScript reference is missing!");
        }
    }

    // Show Profile Panel
    public void OnProfile()
    {
        ShowPanel(ProfilePanel);
        Debug.LogWarning("Profile Panel is active");
    }

    // Show Leaderboard Panel
    public void OnLeaderBoard()
    {
        ShowPanel(LeaderBoard);
    }

    public void OnEditProfile()
    {
        ShowPanel(EditPanel);
    }

    // A utility function to show a specific panel
    private void ShowPanel(GameObject panel)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false); // Hide the previous panel
        }
        panel.SetActive(true); // Show the new panel
        currentPanel = panel; // Update currentPanel reference
    }

    // Handle the back button click event
    public void OnBackButtonClicked()
    {
       HomePanel.SetActive(true);
        HomeMark.SetActive(true);
        ProfilePanel.SetActive(false);
        Selectscreen.SetActive(false);
        LeaderBoard.SetActive(false);
    }

    // Example placeholder methods for other game buttons
    public void OnCricket()
    {
        ShowPanel(CricketLoadingPanel);
        if (gameFetcher != null)
        {
            StartCoroutine(gameFetcher.GetGameData("CRICKET"));
        }
        else
        {
            Debug.LogError("GameFetcherScript reference is missing!");
        }
    }

    public void OnClassicLudo()
    {
        ShowPanel(ClassicLudoLoadingPanel);
        if (gameFetcher != null)
        {
            StartCoroutine(gameFetcher.GetGameData("LUDO"));
        }
        else
        {
            Debug.LogError("GameFetcherScript reference is missing!");
        }
    }

    public void OnMindMorga()
    {
        ShowPanel(MindMorgaLoadingPanel);
        if (gameFetcher != null)
        {
            StartCoroutine(gameFetcher.GetGameData("MEMORYGAME"));
        }
        else
        {
            Debug.LogError("GameFetcherScript reference is missing!");
        }
    }

    public void OnRummyClicked()
    {
        // SendGameIdToServer("Rummy");
        SceneManager.LoadScene("RummyLoading");
    }

    public void OnSnakeAndLadderClicked()
    {
        // SendGameIdToServer("Rummy");
        SceneManager.LoadScene("SnakeAndLadderLoading");
    }

    private void SendGameIdToServer(string gameId)
    {
        if (socketManager != null && socketManager.isConnected)
        {
            socketManager.socket.Emit("INIT_GAME", gameId);
            Debug.Log("Sent game ID: " + gameId);
            SceneManager.LoadScene("LudoLoadingScene"); // Change to your desired loading scene
        }
        else
        {
            Debug.LogWarning("Socket is not connected. Cannot send game ID.");
        }
    }
    private IEnumerator GetUserCount()
    {
        Debug.Log("Getting user count");
        UnityWebRequest request = UnityWebRequest.Get(baseUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error fetching user count: {request.error}");
            yield break;
        }

        string responseText = request.downloadHandler.text;
        Debug.Log($"Backend Response: {responseText}");

        UserCountResponse response = JsonUtility.FromJson<UserCountResponse>(responseText);

        // Update the UI text with the user count
        userCountText.text = $" {response.count}";
    }

    [System.Serializable]
    public class UserCountResponse
    {
        public int count;
    }
}
            
                
                

