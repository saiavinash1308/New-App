//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;
//using TMPro;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using System.Collections.Generic;

//public class GameFetcherScript : MonoBehaviour
//{
//    public GameObject gameDataPrefab;
//    public Transform contentPanel1;
//    public Transform contentPanel2;
//    public Transform contentPanel3;

//    public GameObject popupUI; // Reference to the popup UI (caution message)
//    public TMP_Text popupMessageText; // Text component for the caution message
//    public Button confirmButton; // Button to confirm deduction
//    public Button cancelButton; // Button to cancel

//    private const string baseUrl = "http://localhost:3001/api/game/fetchGame/";
//    private const string deductUrl = "http://localhost:3001/api/game/deductAmount/";

//    private string authToken = "YourAuthTokenHere"; // Replace with your actual auth token

//    [System.Serializable]
//    public class Game
//    {
//        public string gameId;
//        public string gameType;
//        public int maxPlayers;
//        public int entryFee;
//        public int prizePool;
//        public string currency;
//        public bool isActive;
//    }

//    [System.Serializable]
//    public class GameResponse
//    {
//        public List<Game> games;
//    }

//    void Start()
//    {
//        StartCoroutine(GetGameData("LUDO"));

//        // Set up button listeners
//        confirmButton.onClick.AddListener(OnConfirmDeduction);
//        cancelButton.onClick.AddListener(OnCancelDeduction);

//        // Initially hide the popup UI
//        popupUI.SetActive(false);
//    }

//    private IEnumerator GetGameData(string gameType)
//    {
//        string url = $"{baseUrl}{gameType}";

//        Debug.Log($"Sending GET request to: {url}");

//        UnityWebRequest request = UnityWebRequest.Get(url);
//        yield return request.SendWebRequest();

//        if (request.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError($"Error fetching games: {request.error}");
//            yield break;
//        }

//        string rawResponse = request.downloadHandler.text;
//        Debug.Log($"Backend Response: {rawResponse}");

//        if (string.IsNullOrEmpty(rawResponse))
//        {
//            Debug.LogWarning("Empty response from game fetcher API.");
//            yield break;
//        }

//        GameResponse response = JsonUtility.FromJson<GameResponse>(rawResponse);

//        if (response.games == null || response.games.Count == 0)
//        {
//            Debug.Log("No games available.");
//            yield break;
//        }

//        Debug.Log("Parsed Game Data:");
//        foreach (var game in response.games)
//        {
//            Debug.Log($"Game Name: {game.gameType}, Game Type: {game.gameType}, Max Players: {game.maxPlayers}, Prize Pool: {game.prizePool} {game.currency}");
//        }

//        PopulatePanel(contentPanel1, response.games, "all");
//        PopulatePanel(contentPanel2, response.games, "2");
//        PopulatePanel(contentPanel3, response.games, "4");

//        Canvas.ForceUpdateCanvases();
//    }

//    private void PopulatePanel(Transform contentPanel, List<Game> games, string playerType)
//    {
//        foreach (Transform child in contentPanel)
//        {
//            Destroy(child.gameObject);
//        }

//        List<Game> filteredGames = new List<Game>();

//        if (playerType == "all")
//        {
//            filteredGames = games;
//        }
//        else if (playerType == "2")
//        {
//            filteredGames = games.FindAll(game => game.maxPlayers == 2);
//        }
//        else if (playerType == "4")
//        {
//            filteredGames = games.FindAll(game => game.maxPlayers == 4);
//        }

//        foreach (var game in filteredGames)
//        {
//            GameObject gameDataUI = Instantiate(gameDataPrefab, contentPanel);

//            TMP_Text[] textComponents = gameDataUI.GetComponentsInChildren<TMP_Text>();

//            textComponents[0].text = game.gameType;
//            textComponents[1].text = $"Max Players: {game.maxPlayers}";
//            textComponents[2].text = $"Prize Pool: {game.prizePool} {game.currency}";

//            Button button = gameDataUI.GetComponentInChildren<Button>();
//            button.onClick.AddListener(() => OnGameButtonClicked(game));
//        }
//    }

//    private void OnGameButtonClicked(Game game)
//    {
//        Debug.Log($"Button clicked for game: {game.gameType} (ID: {game.gameId})");

//        // Show the caution popup
//        popupMessageText.text = $"Are you sure you want to play {game.gameType}? Amount will be deducted: {game.entryFee} {game.currency}.";
//        popupUI.SetActive(true);

//        // Store the game data for later confirmation
//        selectedGame = game;
//    }

//    private Game selectedGame;

//    private void OnConfirmDeduction()
//    {
//        StartCoroutine(SendDeductionRequest(selectedGame));
//    }

//    private void OnCancelDeduction()
//    {
//        // Close the popup without deducting any amount
//        popupUI.SetActive(false);
//    }

//    private IEnumerator SendDeductionRequest(Game game)
//    {
//        string url = deductUrl;

//        // Create a JSON object to send in the request
//        var deductionData = new
//        {
//            gameId = game.gameId,
//            entryFee = game.entryFee,
//            authToken = authToken
//        };

//        string jsonBody = JsonUtility.ToJson(deductionData);
//        UnityWebRequest request = new UnityWebRequest(url, "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");

//        yield return request.SendWebRequest();

//        if (request.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError($"Error sending deduction request: {request.error}");
//            yield break;
//        }

//        Debug.Log("Deduction request successful. Proceeding to load the scene...");

//        // Close the popup UI
//        popupUI.SetActive(false);

//        // Load the scene based on the game type and max players
//        string sceneToLoad = "";

//        if (game.gameType == "LUDO")
//        {
//            if (game.maxPlayers == 2)
//            {
//                sceneToLoad = "fastludoFor2Player";
//            }
//            else if (game.maxPlayers == 4)
//            {
//                sceneToLoad = "fastludo";
//            }
//        }
//        else if (game.gameType == "CRICKET")
//        {
//            if (game.maxPlayers == 2)
//            {
//                sceneToLoad = "Cricket2PlayerScene";
//            }
//            else if (game.maxPlayers == 4)
//            {
//                sceneToLoad = "Cricket4PlayerScene";
//            }
//        }

//        if (!string.IsNullOrEmpty(sceneToLoad))
//        {
//            SceneManager.LoadScene(sceneToLoad);
//        }
//        else
//        {
//            Debug.LogWarning($"No scene configured for game: {game.gameType} with {game.maxPlayers} players.");
//        }
//    }
//}
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameFetcherScript : MonoBehaviour
{
    public GameObject gameDataPrefab;
    public Transform contentPanel1;
    public Transform contentPanel2;
    public Transform contentPanel3;

    public GameObject popupUI; // Reference to the popup UI (caution message)
    public TMP_Text popupMessageText; // Text component for the caution message
    public Button confirmButton; // Button to confirm deduction
    public Button cancelButton; // Button to cancel

    private const string baseUrl = "https://server.fivlog.space/api/game/fetchGame/";

    private string authToken = "YourAuthTokenHere"; // Replace with your actual auth token
    [SerializeField]
    private SocketManager socketManager;
    [System.Serializable]
    public class Game
    {
        public string gameId;
        public string gameType;
        public int maxPlayers;
        public int entryFee;
        public int prizePool;
        public string currency;
        public bool isActive;
    }

    [System.Serializable]
    public class GameResponse
    {
        public List<Game> games;
    }
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

        //StartCoroutine(GetGameData("FAST_LUDO"));

        // Set up button listeners
        confirmButton.onClick.AddListener(OnConfirmDeduction);
        cancelButton.onClick.AddListener(OnCancelDeduction);

        // Initially hide the popup UI
        popupUI.SetActive(false);
    }

    internal IEnumerator GetGameData(string gameType)
    {
        string url = $"{baseUrl}{gameType}";

        Debug.Log($"Sending GET request to: {url}");

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error fetching games: {request.error}");
            yield break;
        }

        string rawResponse = request.downloadHandler.text;
        Debug.Log($"Backend Response: {rawResponse}");

        if (string.IsNullOrEmpty(rawResponse))
        {
            Debug.LogWarning("Empty response from game fetcher API.");
            yield break;
        }

        GameResponse response = JsonUtility.FromJson<GameResponse>(rawResponse);

        if (response.games == null || response.games.Count == 0)
        {
            Debug.Log("No games available.");
            yield break;
        }

        Debug.Log("Parsed Game Data:");
        foreach (var game in response.games)
        {
            Debug.Log($"Game Name: {game.gameType}, Game Type: {game.gameType}, Max Players: {game.maxPlayers}, Prize Pool: {game.prizePool} {game.currency}");
        }

        PopulatePanel(contentPanel1, response.games, "all");
        PopulatePanel(contentPanel2, response.games, "2");
        PopulatePanel(contentPanel3, response.games, "4");

        Canvas.ForceUpdateCanvases();
    }

    private void PopulatePanel(Transform contentPanel, List<Game> games, string playerType)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        List<Game> filteredGames = new List<Game>();

        if (playerType == "all")
        {
            filteredGames = games;
        }
        else if (playerType == "2")
        {
            filteredGames = games.FindAll(game => game.maxPlayers == 2);
        }
        else if (playerType == "4")
        {
            filteredGames = games.FindAll(game => game.maxPlayers == 4);
        }

        foreach (var game in filteredGames)
        {
            GameObject gameDataUI = Instantiate(gameDataPrefab, contentPanel);

            TMP_Text[] textComponents = gameDataUI.GetComponentsInChildren<TMP_Text>();

            textComponents[0].text = game.gameType;
            textComponents[1].text = $"Max Players: {game.maxPlayers}";
            textComponents[2].text = $" {game.prizePool} {game.currency}";

            Button button = gameDataUI.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnGameButtonClicked(game));
        }
    }

    private void OnGameButtonClicked(Game game)
    {
        Debug.Log($"Button clicked for game: {game.gameType} (ID: {game.gameId})");

        // Show the caution popup
        popupMessageText.text = $"Are you sure you want to play {game.gameType}? Amount will be deducted: {game.entryFee} {game.currency}.";
        popupUI.SetActive(true);

        // Store the game data for later confirmation
        selectedGame = game;
    }

    private Game selectedGame;

    private void OnConfirmDeduction()
    {
        // Close the popup UI
        popupUI.SetActive(false);
        Debug.Log($"Emitting INIT_GAME event with gameId: {selectedGame.gameId}");
        socketManager.EmitEvent("INIT_GAME", selectedGame.gameId);
        // Load the scene based on the game type and max players
        string sceneToLoad = "";

        if (selectedGame.gameType == "FAST_LUDO")
        {
            if (selectedGame.maxPlayers == 2)
            {
                sceneToLoad = "LudoLoadingSceneFor2Player";
            }
            else if (selectedGame.maxPlayers == 4)
            {
                sceneToLoad = "LudoLoadingScene";
            }
        }
        else if(selectedGame.gameType == "LUDO")
        {
            if (selectedGame.maxPlayers == 2)
            {
                sceneToLoad = "ClassicLudoLoadingSceneFor2Player";
            }
            else if (selectedGame.maxPlayers == 4)
            {
                sceneToLoad = "ClassicLudoLoadingScene";
            }
        }
        else if (selectedGame.gameType == "CRICKET")
        {
            if (selectedGame.maxPlayers == 2)
            {
                sceneToLoad = "CricketLoadingScene";         // SOCKET TEST SCENE
            }
        }

        else if (selectedGame.gameType == "MEMORYGAME")
        {
            if (selectedGame.maxPlayers == 2)
            {
                sceneToLoad = "MindMorgaLoadingScene";         // SOCKET TEST SCENE
            }
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning($"No scene configured for game: {selectedGame.gameType} with {selectedGame.maxPlayers} players.");
        }
    }

    private void OnCancelDeduction()
    {
        // Close the popup without deducting any amount
        popupUI.SetActive(false);
    }
}
