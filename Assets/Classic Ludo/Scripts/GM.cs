using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SocketManager;

public class GM : MonoBehaviour
{
    public static GM game { get; private set; }
    //public static GM game;
    public GameObject PopUp;
    public Button Back;
    public Button Quit;
    public Button Cancle;
    public RD rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = false;
    public List<PPt> playeronpathpointList = new List<PPt>();
    public bool canDiceRoll = true;
    //public bool transferDice = false;
    //public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;

    public RD[] manageRolingDice;
    //public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    public AudioSource centerPathAudioSource; // Add this reference

    public float countdownTime = 15f; // Set the countdown time (10 seconds)
    private float currentCountdown; // To track the remaining time
    private Coroutine countdownCoroutine; // To handle the countdown coroutine
    //public Image countdownImage; // Reference to the circular countdown Image
    public Image blueTimerImage;
    public Image redTimerImage;
    public Image greenTimerImage;
    public Image yellowTimerImage;


    // Declare individual arrows
    public GameObject bluearrow;
    public GameObject redarrow;
    public GameObject greenarrow;
    public GameObject yellowarrow;

    public GameObject blueblocker;
    public GameObject redblocker;
    public GameObject greenblocker;
    public GameObject yellowblocker;

    public Text bluePlayerIdText;
    public Text redPlayerIdText;
    public Text greenPlayerIdText;
    public Text yellowPlayerIdText;

    public Text prizePool;

    User currentUser;
    public User[] players;

    private static readonly string[] playerColors = { "Blue", "Red", "Green", "Yellow" };


    private const string TOGGLE_PREF_KEY = "ButtonTogglerState";

    public static string currentTurnSocketId;

    public string turnSocketId;

    public string token;

    [SerializeField] private Transform rollingDiceTarget; // The single Transform for dynamically positioning the dice

    public GameObject[] dice; // Array to store the dice GameObjects
    public Transform diceTargetPositions; // Target positions for dice

    public Text winnerText;
    public GameObject scorePopupPanel;

    public GameObject[] RedLife;
    public Image[] RedheartImages;
    public int RedplayerLives = 3;

    public GameObject[] YellowLife;
    public Image[] YellowheartImages;
    public int YellowplayerLives = 3;

    public GameObject[] BlueLife;
    public Image[] BlueheartImages;
    public int BlueplayerLives = 3;

    public GameObject[] GreenLife;
    public Image[] GreenheartImages;
    public int GreenplayerLives = 3;

    private Text livesText;
    public GameObject LifeOverPopup;
    public Text gameOverText;

    public CountdownTimer countdownTimer;
    public float twoPlayerTime = 180f; // 3 minutes for 2 players
    public float fourPlayerTime = 300f; // 5 minutes for 4 players

    public Text blueScoreText;
    public Text redScoreText;
    public Text greenScoreText;
    public Text yellowScoreText;

    private int blueScore = 0;
    private int redScore = 0;
    private int greenScore = 0;
    private int yellowScore = 0;

    public GameObject winPanel;
    public GameObject losePanel;


    //public UIManager uiManager;
    [SerializeField]
    private SocketManager socketManager;
   

    private void Awake()
    {
        Debug.Log("GM Instance Created");
        game = this;
        ads = GetComponent<AudioSource>();
        socketManager = FindObjectOfType<SocketManager>();


    }

    private void Start()
    {
 

        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }
        Debug.Log("Turn Socket ID: " + currentTurnSocketId);

        StartCountdown();
        ResizePlayerPieces(); // Ensure initial resizing
                              // Load the saved toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;  // Default to 'on' if no value is saved

        SetAudioVolume(isOn);
        
    }

    private RD[] GetActiveDice()
    {
        // Dynamically adjust the dice array based on the number of players
        if (players.Length == 2)
        {
            return new RD[] { manageRolingDice[1], manageRolingDice[3] }; // Only two dice for 2 players
        }
        else if (players.Length == 4)
        {
            return new RD[] { manageRolingDice[0], manageRolingDice[1], manageRolingDice[2], manageRolingDice[3] }; // Three dice for 3 players
        }
        return manageRolingDice; // For 4 players, use all 4 dice
    }


    public void UpdateArrowVisibility(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= players.Length)
        {
            Debug.LogError("Invalid playerIndex: " + playerIndex);
            return;
        }

        RD[] activeDice = GetActiveDice();
        SetAllInactive();

        if (players.Length == 2)
        {
            // For 2 players, assign dice and arrows to Red (Index 1) and Yellow (Index 3)
            if (playerIndex == 0)
            {
                SetActiveState(redarrow, blueblocker, redblocker, greenblocker, yellowblocker);
                rolingDice = activeDice[0]; // Red's dice
            }
            else if (playerIndex == 1)
            {
                SetActiveState(yellowarrow, blueblocker, redblocker, greenblocker, yellowblocker);
                rolingDice = activeDice[1]; // Yellow's dice
            }
        }
        else if (players.Length == 4)
        {
            // For 4 players, assign arrows normally
            switch (playerIndex)
            {
                case 0:
                    SetActiveState(bluearrow, blueblocker, redblocker, greenblocker, yellowblocker);
                    rolingDice = activeDice[0]; // Blue's dice
                    break;
                case 1:
                    SetActiveState(redarrow, blueblocker, redblocker, greenblocker, yellowblocker);
                    rolingDice = activeDice[1]; // Red's dice
                    break;
                case 2:
                    SetActiveState(greenarrow, blueblocker, redblocker, greenblocker, yellowblocker);
                    rolingDice = activeDice[2]; // Green's dice
                    break;
                case 3:
                    SetActiveState(yellowarrow, blueblocker, redblocker, greenblocker, yellowblocker);
                    rolingDice = activeDice[3]; // Yellow's dice
                    break;
            }
        }
    }



    private void SetActiveState(GameObject arrow, GameObject blueBlocker, GameObject redBlocker, GameObject greenBlocker, GameObject yellowBlocker)
    {
        arrow.SetActive(true);
        blueBlocker.SetActive(false);
        redBlocker.SetActive(true);
        greenBlocker.SetActive(true);
        yellowBlocker.SetActive(true);
    }

    private void SetAllInactive()
    {
        bluearrow.SetActive(false);
        redarrow.SetActive(false);
        greenarrow.SetActive(false);
        yellowarrow.SetActive(false);

        blueblocker.SetActive(true);
        redblocker.SetActive(true);
        greenblocker.SetActive(true);
        yellowblocker.SetActive(true);
    }




    public void InitializePlayers(User[] users)
    {
        if (users.Length < 2 || users.Length > 4)
        {
            Debug.LogError("Invalid number of players. The game supports 2 to 4 players.");
            return;
        }

        players = users;
        UpdatePlayerUI();
        InitializeDicePositions();

        if (countdownTimer == null)
        {
            Debug.LogError("CountdownTimer reference is not assigned in GM!");
            return;
        }

        // Determine and set the countdown time
        float countdownTime = DetermineCountdownTime();
        countdownTimer.SetTimeAndStart(countdownTime);
    }

    private float DetermineCountdownTime()
    {
        if (players.Length == 2)
        {
            Debug.Log("Setting timer for 2 players.");
            return twoPlayerTime;
        }
        else if (players.Length == 4)
        {
            Debug.Log("Setting timer for 4 players.");
            return fourPlayerTime;
        }
        else
        {
            Debug.LogWarning("Unexpected number of players. Using default time.");
            return fourPlayerTime; // Default to 4-player time if undefined
        }
    }

    private void InitializeDicePositions()
    {
        if (players == null || players.Length == 0)
        {
            Debug.LogWarning("Players array is empty. Cannot initialize dice positions.");
            return;
        }

        if (diceTargetPositions == null)
        {
            Debug.LogError("Target position for dice is not set.");
            return;
        }

        string mySocketId = socketManager != null ? socketManager.getMySocketId() : null;

        if (string.IsNullOrEmpty(mySocketId))
        {
            Debug.LogWarning("SocketManager or mySocketId is not properly initialized.");
            return;
        }

        if (players.Length == 2)
        {
            // Handle 2-player mode
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].socketId == mySocketId)
                {
                    int diceIndex = (i == 0) ? 0 : 2; // Use dice at index [1] for player 1 and [3] for player 2
                    if (diceIndex < dice.Length && dice[diceIndex] != null)
                    {
                        dice[diceIndex].transform.position = diceTargetPositions.position;
                        dice[diceIndex].transform.rotation = diceTargetPositions.rotation;
                        Debug.Log($"Initialized dice for player {i} (2-player mode) at target position.");
                        BoxCollider2D diceCollider = dice[diceIndex].GetComponent<BoxCollider2D>();
                        if (diceCollider != null)
                        {
                            diceCollider.enabled = true;
                            Debug.Log($"Enabled collider for dice {diceIndex} for player {i}.");
                        }

                    }
                    else
                    {
                        Debug.LogWarning($"Dice not set for player {i} (2-player mode).");
                    }
                }
            }
        }
        else if (players.Length == 4)
        {
            // Handle 4-player mode
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].socketId == mySocketId)
                {
                    if (i < dice.Length && dice[i] != null)
                    {
                        dice[i].transform.position = diceTargetPositions.position;
                        dice[i].transform.rotation = diceTargetPositions.rotation;
                        Debug.Log($"Initialized dice for player {i} (4-player mode) at target position.");

                    }
                    else
                    {
                        Debug.LogWarning($"Dice not set for player {i} (4-player mode).");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Unexpected number of players. Dice initialization skipped.");
        }
    }


    private void UpdatePlayerUI()
    {
        if (players == null || players.Length < 2)
        {
            Debug.LogError("Invalid number of players. The game requires at least 2 players.");
            return;
        }

        // Reset all text fields
        bluePlayerIdText.text = "";
        redPlayerIdText.text = "";
        greenPlayerIdText.text = "";
        yellowPlayerIdText.text = "";
        prizePool.text = "";

        if (players.Length == 2)
        {
            // Assign first player to Red and second player to Yellow
            prizePool.text = socketManager.getPrizePool().ToString();
            redPlayerIdText.text = players[0].username;
            yellowPlayerIdText.text = players[1].username;
        }
        else if (players.Length == 4)
        {
            // Assign players to their respective colors for 4 players
            prizePool.text = socketManager.getPrizePool().ToString();
            bluePlayerIdText.text = players[0].username;
            redPlayerIdText.text = players[1].username;
            greenPlayerIdText.text = players[2].username;
            yellowPlayerIdText.text = players[3].username;
        }
    }





    public void HandlePlayerTurn(string socketId)
    {
        StartCoroutine(HandleTurnCoroutine(socketId));
    }

    private IEnumerator HandleTurnCoroutine(string socketId)
    {
        // Find the player whose socketId matches the passed value
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].socketId == socketId)
            {
                // Update arrow visibility with a delay (for smoothness)
                UpdateArrowVisibility(i);
                yield return new WaitForSeconds(0.5f); // Delay for a smoother transition

                // Update current dice with a delay
                UpdateCurrentDice(i);
                yield return new WaitForSeconds(0.5f); // Delay for a smoother transition

                // Start countdown if necessary (you can add more steps here)
                 StartCountdown();

                break;
            }
        }

        // Set the turn-related flags
        turnSocketId = socketId;
        canDiceRoll = true;
        canPlayermove = false;

        // Resize player pieces with a small delay to make it visually smoother
        ResizePlayerPieces();
        yield return new WaitForSeconds(0.5f); // Optional delay

    }


    private void UpdateCurrentDice(int playerIndex)
    {
        rolingDice = players.Length == 2 ? (playerIndex == 0 ? manageRolingDice[1] : manageRolingDice[3]) : manageRolingDice[playerIndex];
        UpdateDiceOpacity();
    }

    public void UpdateRollingDicePosition(string socketId)
    {
        if (players == null || players.Length == 0)
        {
            Debug.LogError("Players array is null or empty!");
            return;
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].socketId == socketId)
            {
                if (manageRolingDice == null || manageRolingDice.Length <= i)
                {
                    Debug.LogError($"manageRolingDice is null or does not have enough elements! Index: {i}");
                    return;
                }

                RD currentDice = manageRolingDice[i];
                if (currentDice == null)
                {
                    Debug.LogError($"manageRolingDice[{i}] is null!");
                    return;
                }

                if (rollingDiceTarget == null)
                {
                    Debug.LogError("diceTransform is null!");
                    return;
                }

                currentDice.transform.position = rollingDiceTarget.position;
                currentDice.transform.rotation = rollingDiceTarget.rotation;
                Debug.Log($"Updated dice position for player {i} (socketId: {socketId}).");
                return;
            }
        }

        Debug.LogWarning($"No matching player found for socketId: {socketId}");
    }



    public void SetAudioVolume(bool isOn)
    {
        float volume = isOn ? 1.0f : 0.0f;

        if (ads != null)
        {
            ads.volume = volume;
        }

        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.volume = volume;
        }
    }


    public void AddPathPoint(PPt pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PPt pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }


    private void UpdateDiceOpacity()
    {
        for (int i = 0; i < manageRolingDice.Length; i++)
        {
            SpriteRenderer diceRenderer = manageRolingDice[i].transform.Find("NSH").GetComponent<SpriteRenderer>();

            SpriteRenderer newObjectRenderer = manageRolingDice[i].transform.Find("DB").GetComponent<SpriteRenderer>();

            if (diceRenderer != null)
            {
                Color diceColor = diceRenderer.color;
                Color newObjectColor = newObjectRenderer.color;

                if (manageRolingDice[i] == rolingDice)
                {
                    diceColor.a = 1f;
                    newObjectColor.a = 1f;
                }
                else
                {
                    diceColor.a = 0.5f;
                    newObjectColor.a = 0.1f;
                }

                diceRenderer.color = diceColor;
                newObjectRenderer.color = newObjectColor;
            }
            else
            {
                Debug.LogWarning(manageRolingDice[i].name + " does not have a SpriteRenderer on its NumberSpriteHolder or the new GameObject.");
            }
        }
    }

    public bool AllPlayersInHome()
    {
        List<PP> pieces = GetPlayerPiecesForCurrentDice();

        foreach (PP piece in pieces)
        {
            if (piece.isready)
            {
                return false;
            }
        }
        return true;
    }

    
    public bool IsPlayerOut(RD dice)
    {
        if (dice == manageRolingDice[0]) return blueOutPlayers > 0;
        if (dice == manageRolingDice[1]) return redOutPlayers > 0;
        if (dice == manageRolingDice[2]) return greenOutPlayers > 0;
        if (dice == manageRolingDice[3]) return yellowOutPlayers > 0;
        return false;
    }

    public void StartCountdown()
    {
        ResetCountdown();

        blueTimerImage.gameObject.SetActive(false);
        redTimerImage.gameObject.SetActive(false);
        greenTimerImage.gameObject.SetActive(false);
        yellowTimerImage.gameObject.SetActive(false);

        if (rolingDice == manageRolingDice[0])
        {
            blueTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(blueTimerImage));
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(redTimerImage));
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(greenTimerImage));
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(yellowTimerImage));
        }
    }



    public void ResetCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }


    private IEnumerator CountdownCoroutine(Image currentTimerImage)
    {
        float startTime = Time.time;
        float endTime = startTime + countdownTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            currentTimerImage.fillAmount = timeRemaining / countdownTime;

            yield return null;
        }

        currentTimerImage.fillAmount = 0;
        currentTimerImage.gameObject.SetActive(false);

        if (rolingDice == manageRolingDice[0])
        {
            HandleTurnTimeout("Blue");  // Red player's time is up
        }
        else if (rolingDice == manageRolingDice[1])
        {
            HandleTurnTimeout("Red");  // Yellow player's time is up
        }
        else if (rolingDice == manageRolingDice[2])
        {
            HandleTurnTimeout("Green");  // Yellow player's time is up
        }
        else if (rolingDice == manageRolingDice[3])
        {
            HandleTurnTimeout("Yellow");  // Yellow player's time is up
        }
    }


    private void HandleTurnTimeout(string playerColor)
    {
        Debug.Log(playerColor + "'s time is up!");
        LoseLife(playerColor);  // Deduct a life for the respective player
        //transferDice = true;    // Transfer the dice to the next player
        canDiceRoll = false;    // Stop the dice roll for the current player

        if (socketManager != null && socketManager.isConnected)
        {


            if (socketManager.socket.Id == GM.game.turnSocketId)
            {
                socketManager.socket.Emit("TURN_UPDATED");
                Debug.Log($"TURN_UPDATED emitted for"+ turnSocketId);
            }
            
        }
        else
        {
            Debug.LogWarning("SocketManager is not connected. Cannot emit TURN_UPDATED.");
        }
    }





    private void LoseLife(string playerColor)
    {
        if (playerColor == "Red" && RedplayerLives > 0)
        {
            RedplayerLives--;
            UpdateLivesUI("Red");
        }
        else if (playerColor == "Blue" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Blue");
        }
        else if (playerColor == "Green" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Green");
        }
        else if (playerColor == "Yellow" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Yellow");
        }

        // If a player runs out of lives, handle game over
        if (RedplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (BlueplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (GreenplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (YellowplayerLives <= 0)
        {
            HandleGameOver();
        }
    }

    private void UpdateLivesUI(string playerColor)
    {
        if (playerColor == "Red")
        {
            for (int i = 0; i < RedheartImages.Length; i++)
            {
                RedheartImages[i].enabled = i < RedplayerLives;
            }
        }
        else if (playerColor == "Blue")
        {
            for (int i = 0; i < BlueheartImages.Length; i++)
            {
                BlueheartImages[i].enabled = i < BlueplayerLives;
            }
        }
        else if (playerColor == "Green")
        {
            for (int i = 0; i < GreenheartImages.Length; i++)
            {
                GreenheartImages[i].enabled = i < GreenplayerLives;
            }
        }
        else if (playerColor == "Yellow")
        {
            for (int i = 0; i < YellowheartImages.Length; i++)
            {
                YellowheartImages[i].enabled = i < YellowplayerLives;
            }
        }
    }

    internal void HandleGameOver()
    {
        if (gameOverText != null)
        {
            LifeOverPopup.SetActive(true);  // Show the Game Over popup
            gameOverText.text = "You have missed the maximum number of turns!";  // Set the Game Over message
        }

        // Stop player movement
        canPlayermove = false;

        // Stop dice rolling
        canDiceRoll = false;

        // Disable all player pieces to stop any further interactions
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        // Disable all dice
        foreach (RD dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        // Optionally, stop any other gameplay routines, like bot behavior or countdowns
        StopAllCoroutines();  // This stops all ongoing coroutines, including bot routines or timers

        // If you want to freeze the game completely, you can also pause the game time
        Time.timeScale = 0;  // Pauses the game completely
    }


    public void OnContinueBtn()
    {
        SceneManager.LoadScene("Home");
    }

    internal void HandleGameWinner(string winnerId)
    {
        if (winnerText != null)
        {
            scorePopupPanel.SetActive(true); // Show the Game Over popup
            winnerText.text = $"🎉 Congratulations Winner!\nPlayer ID: {winnerId}"; // Display winner ID
            Debug.LogWarning("Congratulations Winner! " + winnerId);
        }

        // Stop player movement and dice rolling
        canPlayermove = false;
        canDiceRoll = false;

        // Disable all player pieces to stop any further interactions
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        // Disable all dice
        foreach (RD dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        // Stop all ongoing coroutines
        StopAllCoroutines();
        Time.timeScale = 0; // Pause the game completely

        // Show appropriate panels for the players
        foreach (User player in players)
        {
            if (player.socketId == winnerId)
            {
                // Show the "Win" panel for the winner
                ShowWinPanel();
            }
            else
            {
                // Show the "Lose" panel for the other players
                ShowLosePanel();
            }
        }
    }

    // Show the "Win" panel
    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
        losePanel.SetActive(false);
    }

    // Show the "Lose" panel
    private void ShowLosePanel()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(true);
    }


    //public void IncrementCenterCount(string playerColor)
    //{
    //    switch (playerColor)
    //    {
    //        case "Blue":
    //            blueFinishedPlayers++;
    //            if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
    //            break;
    //        case "Red":
    //            redFinishedPlayers++;
    //            if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
    //            break;
    //        case "Green":
    //            greenFinishedPlayers++;
    //            if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
    //            break;
    //        case "Yellow":
    //            yellowFinishedPlayers++;
    //            if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
    //            break;
    //    }
    //}

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    //public void CheckWinCondition()
    //{
    //    if (players.Length < 2) return;

    //    if (HasPlayerWon("Blue"))
    //    {
    //        Debug.Log("Blue Player Wins!");
    //        EndGame();
    //    }
    //    else if (HasPlayerWon("Red"))
    //    {
    //        Debug.Log("Red Player Wins!");
    //        EndGame();
    //    }
    //    else if (HasPlayerWon("Green"))
    //    {
    //        Debug.Log("Green Player Wins!");
    //        EndGame();
    //    }
    //    else if (HasPlayerWon("Yellow"))
    //    {
    //        Debug.Log("Yellow Player Wins!");
    //        EndGame();
    //    }
    //}

    private void EndGame()
    {
        foreach (PP piece in FindObjectsOfType<PP>())
        {
            piece.enabled = false;
        }

        foreach (RD dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;
    }

    //private void LoadWinnerScene(string winner)
    //{
    //    PlayerPrefs.SetString("Winner", winner);
    //    SceneManager.LoadScene("Winner");
    //}

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }
    public List<PP> GetPlayerPiecesForCurrentDice()
    {
        List<PP> playerPieces = new List<PP>();

        foreach (PP piece in FindObjectsOfType<PP>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }

    void ResizePlayerPieces()
    {
        // Get the active color
        string activeColor = "";
        if (rolingDice == manageRolingDice[0]) activeColor = "Blue1";
        else if (rolingDice == manageRolingDice[1]) activeColor = "Red1";
        else if (rolingDice == manageRolingDice[2]) activeColor = "Green1";
        else if (rolingDice == manageRolingDice[3]) activeColor = "Yellow1";

        // Resize pieces based on the active color
        foreach (string color in new[] { "Blue1", "Red1", "Green1", "Yellow1" })
        {
            float scale;
            float height;
            float depth;

            GameObject[] pieces = GameObject.FindGameObjectsWithTag(color);

            foreach (GameObject piece in pieces)
            {
                if (color == "Blue1")
                {
                    BluePP blueScript = piece.GetComponent<BluePP>();
                    if (blueScript != null) blueScript.enabled = (color == activeColor);
                }
                else if (color == "Red1")
                {
                    RedPP redScript = piece.GetComponent<RedPP>();
                    if (redScript != null) redScript.enabled = (color == activeColor);
                }
                else if (color == "Green1")
                {
                    GreenPP greenScript = piece.GetComponent<GreenPP>();
                    if (greenScript != null) greenScript.enabled = (color == activeColor);
                }
                else if (color == "Yellow1")
                {
                    YellowPP yellowScript = piece.GetComponent<YellowPP>();
                    if (yellowScript != null) yellowScript.enabled = (color == activeColor);
                }
                PlayerPieces playerPiece = piece.GetComponent<PlayerPieces>();

                // Determine if the piece is in a safe zone
                bool isInSafeZone = playerPiece != null && playerPiece.IsInSafeZone;

                // If the piece is in a safe zone, make it smaller
                if (isInSafeZone)
                {
                    scale = 0.5f;   // Smaller scale when in the safe zone
                    height = 0.5f;
                    depth = 0.5f;
                }
                else
                {
                    // If the piece is not in the safe zone, use the standard scaling
                    scale = color == activeColor ? 0.6f : 0.6f;
                    height = color == activeColor ? 0.6f : 0.6f;
                    depth = color == activeColor ? 0.5f : 0.5f;
                }

                // Resize the piece
                piece.transform.localScale = new Vector3(scale, height, depth);

                // Update sorting order based on the y-position to prevent hiding
                SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // The higher the y-position, the lower the sorting order
                    spriteRenderer.sortingOrder = Mathf.RoundToInt(-piece.transform.position.y * 100);
                    // Increase sorting order if the piece matches the active color
                    if (color == activeColor)
                    {
                        spriteRenderer.sortingOrder += 10;
                    }
                }
            }
        }
    }


    public void MovePiece(MoveUpdate moveUpdate)
    {
        Debug.LogWarning("Moving Piece");
        Debug.LogWarning($"PlayerID: {moveUpdate.playerId}, PieceID: {moveUpdate.pieceId}, Points: {string.Join(", ", moveUpdate.points)}");
        ApplyPoints(moveUpdate.playerId, moveUpdate.points);
        StartCoroutine(UpdatePlayerPieceCoroutine(moveUpdate.playerId, moveUpdate.pieceId));

    }

    private void ApplyPoints(string playerId, List<int> points)
    {
        if (points == null || points.Count == 0)
        {
            Debug.LogWarning($"No points to display for player {playerId}.");
            return;
        }

        // Ensure we have as many players as points
        if (players.Length < points.Count)
        {
            Debug.LogError($"Mismatch: {points.Count} points but only {players.Length} players.");
            return;
        }

        // Assign points to players based on their order in the `players` array
        for (int i = 0; i < points.Count; i++)
        {
            if (i < players.Length)
            {
                // Assign points to the correct player's text
                string currentPlayerId = players[i].socketId;
                string color = getColor(currentPlayerId);
                string pointText = $" {points[i]}";

                switch (color)
                {
                    case "Blue":
                        blueScoreText.text = pointText;
                        break;

                    case "Red":
                        redScoreText.text = pointText;
                        break;

                    case "Green":
                        greenScoreText.text = pointText;
                        break;

                    case "Yellow":
                        yellowScoreText.text = pointText;
                        break;

                    default:
                        Debug.LogWarning($"Unknown color for player {currentPlayerId}. Points not assigned.");
                        break;
                }

                Debug.Log($"Assigned {points[i]} points to {color} player (Player {i + 1}).");
            }
        }
    }




    public void KillPiece(KillUpdate killUpdate)
    {
        Debug.LogWarning("Moving Piece");
        StartCoroutine(UpdateKilledPlayerPiece(killUpdate.payload.playerId, killUpdate.payload.pieceId));
    }

    public IEnumerator UpdateKilledPlayerPiece(string playerId, int pieceId)
    {
        string color = getColor(playerId);
        token = color + (1).ToString();
        string tokenId = color + (pieceId).ToString();

        GameObject[] pieces = GameObject.FindGameObjectsWithTag(token);

        foreach (GameObject piece in pieces)
        {
            Debug.LogWarning("This is current piece check:" + piece);
            PP playerPiece = piece.GetComponent<PP>();
            Debug.Log("This is token: " + tokenId + "This is pieceName: " + piece.name + " This is comparision: " + (piece.name == tokenId));

            if (playerPiece != null && piece.name == tokenId)
            {
                Debug.Log("Found piece for player: " + token + " tokenId: " + tokenId);

                if (token == "Blue1")
                {
                    Debug.Log("Im here");
                    PPt bluePiece = piece.GetComponent<PPt>();
                    if (bluePiece != null)
                    {
                        Debug.Log("Im Done");
                        bluePiece.ResetToBasePoint(playerPiece); // Call the Blue piece's OnMouseDown
                        if (socketManager != null && socketManager.isConnected)
                        {
                            if (socketManager.getMySocketId() == playerId)
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                        }
                    }
                }
                else if (token == "Yellow1")
                {
                    PPt yellowPiece = piece.GetComponent<PPt>();
                    if (yellowPiece != null)
                    {
                        yellowPiece.ResetToBasePoint(playerPiece); // Call the Yellow piece's OnMouseDown
                        if (socketManager != null && socketManager.isConnected)
                        {
                            if (socketManager.getMySocketId() == playerId)
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                        }
                    }
                }
                else if (token == "Red1")
                {
                    PPt redPiece = piece.GetComponent<PPt>();
                    if (redPiece != null)
                    {
                        redPiece.ResetToBasePoint(playerPiece); // Call the Red piece's OnMouseDown
                        if (socketManager.getMySocketId() == playerId)
                        {
                            if (socketManager.getMySocketId() == playerId)
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                        }
                    }
                }
                else if (token == "Green1")
                {
                    PPt greenPiece = piece.GetComponent<PPt>();
                    if (greenPiece != null)
                    {
                        greenPiece.ResetToBasePoint(playerPiece); // Call the Green piece's OnMouseDown
                        if (socketManager != null && socketManager.isConnected)
                        {
                            if (socketManager.getMySocketId() == playerId)
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Unknown color or piece doesn't have the OnMouseDown() method!");
                }
                yield return null;
            }
        }
    }

    private string getColorByIndex(int i)
    {
        switch (i)
        {
            case 0: return "Blue";
            case 1: return "Red";
            case 2: return "Green";
            case 3: return "Yellow";
            default: return "Blue";  // Default to "Blue" if index is out of range
        }
    }

    private string getColor(string playerId)
    {
        if (players == null || players.Length == 0)
        {
            Debug.LogWarning("Players array is empty or not initialized.");
            return "Blue"; // Return default color if no players
        }

        // Handle the two-player case
        if (players.Length == 2)
        {
            // Assign the first player to Red and the second player to Yellow
            if (players[0].socketId == playerId)
            {
                Debug.LogWarning($"Found matching player! Player {playerId} assigned to Red.");
                return "Red";  // First player gets Red
            }
            else if (players[1].socketId == playerId)
            {
                Debug.LogWarning($"Found matching player! Player {playerId} assigned to Yellow.");
                return "Yellow";  // Second player gets Yellow
            }
        }
        // Handle the four-player case
        else if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log($"Checking player {i}: socketId = {players[i].socketId}, comparing with playerId = {playerId}");

                if (players[i].socketId == playerId)
                {
                    Debug.LogWarning($"Found matching player! Index: {i}, socketId: {players[i].socketId}");
                    return getColorByIndex(i);  // Return the color associated with this player index
                }
            }
        }

        Debug.LogWarning($"Player ID not found: {playerId}, returning default color.");
        return "Blue";  // Default to "Blue" if no player matches
    }




    public IEnumerator UpdatePlayerPieceCoroutine(string playerId, int pieceId)
    {
        string color = getColor(playerId);
        string token = color + "1"; // Tag to find pieces of this color
        string tokenId = color + pieceId.ToString(); // Unique identifier for the specific piece

        // Find the specific GameObject by both tag and name
        GameObject piece = GameObject.FindGameObjectsWithTag(token)
                                      .FirstOrDefault(obj => obj.name == tokenId);

        if (piece == null)
        {
            Debug.LogWarning($"No matching piece found for tokenId: {tokenId}");
            yield break;
        }

        Debug.Log($"Processing piece: {piece.name} for tokenId: {tokenId}");

        bool moveEmitted = false; // Flag to ensure emit happens only once

        // Attempt to move the piece based on its color
        switch (color)
        {
            case "Blue":
                Debug.Log("I'm Blue");
                BluePP bluePiece = piece.GetComponent<BluePP>();
                if (bluePiece != null)
                {
                    if (socketManager != null && socketManager.isConnected)
                    {
                        if (socketManager.socket.Id != GM.game.turnSocketId)
                        {
                            bluePiece.MovePiece();
                            Debug.LogWarning("It's not this player's turn.");
                        }
                        else
                        {
                            bluePiece.MovePiece();
                            if (socketManager.socket.Id != GM.game.turnSocketId)
                            {
                                Debug.LogWarning("It's not this player's turn.");
                                yield return null;
                            }
                            else
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                            //if (!moveEmitted) // Ensure emission happens only once
                            //{
                            //    socketManager.socket.Emit("MOVE_UPDATED", "");
                            //    moveEmitted = true;
                            //}
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                    }
                    Debug.LogWarning("BluePiece Moved");
                }
                break;

            case "Yellow":
                Debug.Log("I'm Yellow");
                YellowPP yellowPiece = piece.GetComponent<YellowPP>();
                if (yellowPiece != null)
                {
                    if (socketManager != null && socketManager.isConnected)
                    {
                        if (socketManager.socket.Id != GM.game.turnSocketId)
                        {
                            Debug.LogWarning("It's not this player's turn.");
                            yellowPiece.MovePiece();
                        }
                        else
                        {
                            yellowPiece.MovePiece();
                            if (socketManager.socket.Id != GM.game.turnSocketId)
                            {
                                Debug.LogWarning("It's not this player's turn.");
                                yield return null;
                            }
                            else
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                    }
                    Debug.LogWarning("YellowPiece Moved");
                }
                break;

            case "Red":
                Debug.Log("I'm Red");
                RedPP redPiece = piece.GetComponent<RedPP>();
                if (redPiece != null)
                {
                    if (socketManager != null && socketManager.isConnected)
                    {
                        if (socketManager.socket.Id != GM.game.turnSocketId)
                        {
                            Debug.LogWarning("It's not this player's turn.");
                            redPiece.MovePiece();
                        }
                        else
                        {
                            redPiece.MovePiece();
                            if (socketManager.socket.Id != GM.game.turnSocketId)
                            {
                                Debug.LogWarning("It's not this player's turn.");
                                yield return null;
                            }
                            else
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                    }
                    Debug.LogWarning("RedPiece Moved");
                }
                break;

            case "Green":
                Debug.Log("I'm Green");
                GreenPP greenPiece = piece.GetComponent<GreenPP>();
                if (greenPiece != null)
                {
                    if (socketManager != null && socketManager.isConnected)
                    {
                        if (socketManager.socket.Id != GM.game.turnSocketId)
                        {
                            Debug.LogWarning("It's not this player's turn.");
                            greenPiece.MovePiece();
                        }
                        else
                        {
                            greenPiece.MovePiece();
                            if (socketManager.socket.Id != GM.game.turnSocketId)
                            {
                                Debug.LogWarning("It's not this player's turn.");
                                yield return null;
                            }
                            else
                            {
                                socketManager.socket.Emit("MOVE_UPDATED");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
                    }
                    Debug.LogWarning("GreenPiece Moved");
                }
                break;

            default:
                Debug.LogWarning($"Unknown color: {color}");
                yield break;
        }

        yield return null;
    }

    public void EnablePopUp()
    {
        PopUp.SetActive(true);
    }

    public void QuitToHome()
    {
        SceneManager.LoadScene("Home"); // Change "HomeScene" to your actual scene name
    }
    public void ClosePopup()
    {
        if (PopUp != null)
        {
            PopUp.SetActive(false);
        }
    }



    public void PlayCenterPathAudio()
    {
        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.Play();
        }
    }

    [System.Serializable]
    public class MovePiecePayload
    {
        public string piece;  // Piece identifier (e.g., the piece name)
        public string roomId;   // The room identifier
    }
}
