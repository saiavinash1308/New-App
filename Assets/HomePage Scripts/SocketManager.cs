using System;
using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System.Collections;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance { get; private set; }
    public SocketIOUnity socket;
    public Text statusText;
    internal bool isConnected = false;
    private bool hasEmittedAddUser = false;
    [SerializeField]
    private GM LudoGameManager;
    [SerializeField]
    private MindMorgaGameController MindMorgaGameController;
    private RD RollingDice;
    private string roomId;
    private string socketId;
    private int steps;
    private User[] users;

    internal bool stopSearch = true;

    //public BowlPlayer bowlplayer;
    //public BatsmanPlayer batsmanplayer;
    //public BowlController bowlcontroller;
    //public BatController batcontroller;
    ////internal bool stopSearch = true;
    //public ScoreManager scoremanager;
    //private GameObject returnpanel;

    private float prizePool;
    //public bool isUsebots;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSocket();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EmitAddUserEventWhenConnected()
    {
        while (!isConnected)
        {
            yield return null; // Wait until connected
        }

        string authToken = PlayerPrefs.GetString("AuthToken", null);
        if (!string.IsNullOrEmpty(authToken) && !hasEmittedAddUser)
        {
            Debug.Log("AuthToken: " + authToken);
            EmitEvent("ADD_USER", authToken);
            hasEmittedAddUser = true; // Emit only once
        }
    }

    private void Start()
    {
        StartCoroutine(EmitAddUserEventWhenConnected());
    }

    internal void InitializeSocket()
    {
        //var url = "http://localhost:3000/";
        var url = "https://sockets.fivlog.space";
        var uri = new Uri(url);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            EIO = 4,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.OnConnected += OnConnected;
        // socket.OnDisconnected += OnDisconnected; // Register for disconnection

        Debug.Log("Connecting to server...");
        socket.Connect();
    }

    internal void OnConnected(object sender, EventArgs e)
    {
        isConnected = true;
        Debug.Log("Connected to server.");
        AddListeners();
        EmitAddUserIfNecessary();
    }

    private void EmitAddUserIfNecessary()
    {
        string authToken = PlayerPrefs.GetString("AuthToken", null);
        if (!string.IsNullOrEmpty(authToken) && !hasEmittedAddUser)
        {
            Debug.Log("AuthToken: " + authToken);
            EmitEvent("ADD_USER", authToken);
            hasEmittedAddUser = true; // Ensure it only emits once
        }
    }

    private void OnDisconnected(object sender, EventArgs e)
    {
        isConnected = false;
        Debug.Log("Disconnected from server.");
        hasEmittedAddUser = false; // Reset flag on disconnection
    }

    internal void EmitEvent(string eventName, string data)
    {
        if (isConnected)
        {
            socket.Emit(eventName, data);
        }
        else
        {
            Debug.LogWarning("Attempted to emit event while disconnected.");
        }
    }

    internal void AddListeners()
    {
        //socket.On("MATCH_MAKING_FAILED", onMatchMakingFailed);

        socket.On("STOP_SEARCH", OnStopSearch);
        socket.On("START_GAME", GameStarted);
        socket.On("CURRENT_TURN", OnPlayerTurn);
        socket.On("UPDATE_MOVE", OnUpdateMove);
        socket.On("KILL_PIECE", OnKillPiece);
        socket.On("DICE_ROLLED", OnDiceRolled);
        socket.On("WINNERS", OnWinner);

        // Cricket...

        //socket.On("START_CRICKET_GAME", OnAssignRole);  // CRICKET
        //socket.On("BOWLER_RUN", OnBowlerRun);
        //socket.On("MOVE_BATSMAN", OnBatsManMoved);
        //socket.On("GROUND_TARGET_MOVE", GroundTargetMove);
        //socket.On("BATSMAN_HIT", OnBatsManHit);
        //socket.On("RESET_BOWLER", OnResetBowler);
        //socket.On("RESET_BATSMAN", OnBatsManReset);
        //socket.On("WICKET", OnWicket);
        //socket.On("UPDATE_SCORE", OnUpdateScore);
        //socket.On("SWITCH_CAMERA", OnSwitchCamera);
        //socket.On("WINNER", OnCricWinner);
        //socket.On("CRICKET_IDLE", onCricketIdle);
        //socket.On("BALL_HIT_POSITION", onBallHit);

        //CLASSIC_LUDO...

        socket.On("CLASSIC_LUDO_STOP_SEARCH", ClassicOnStopSearch);
        socket.On("CLASSIC_LUDO_START_GAME", ClassicGameStarted);
        socket.On("CLASSIC_LUDO_CURRENT_TURN", ClassicOnPlayerTurn);
        socket.On("CLASSIC_LUDO_UPDATE_MOVE", ClassicOnUpdateMove);
        socket.On("CLASSIC_LUDO_KILL_PIECE", ClassicOnKillPiece);
        socket.On("CLASSIC_LUDO_DICE_ROLLED", ClassicOnDiceRolled);
        socket.On("CLASSIC_LUDO_WINNERS", ClassicOnWinner);

        //MIND_MORGA

        socket.On("START_MEMORY_GAME", MindMorgaGameStarted);
        socket.On("MEMORY_GAME_CURRENT_TURN", MindMorgaOnPlayerTurn);
        socket.On("OPEN_CARD", OpenCard);
        socket.On("CLOSE_CARDS", CloseCard);
        socket.On("CARDS_MATCHED", CardsMatched);
        socket.On("END_GAME", EndGame);
    }

    //public void onMatchMakingFailed(SocketIOResponse res)
    //{
    //    string message = res.GetValue<string>();
    //    Debug.Log("Match Making Failed"+message);
    //    isUsebots = true;
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        gamecontroller.PlayButton();
    //    });
    //    //string message = res.GetValue<string>();
    //    //Debug.LogWarning("Quit Room");
    //    //MainThreadDispatcher.Enqueue(() =>
    //    //{
    //    //    returnpanel = GameObject.FindGameObjectWithTag("ReturnPanel");
    //    //    if (returnpanel == null)
    //    //    {
    //    //        returnpanel = GameObject.FindGameObjectWithTag("ReturnPanel"); // if you need to handle search differently
    //    //                                                                       // or use
    //    //        returnpanel = GameObject.FindObjectsOfType<GameObject>(true)
    //    //                                  .FirstOrDefault(obj => obj.CompareTag("ReturnPanel"));
    //    //    }
    //    //    returnpanel.SetActive(true);
    //    //    Invoke("ReturnHome", 2f);
    //    //});

    //}

    public void OnStopSearch(SocketIOResponse res)
    {
        string stopData = res.GetValue<string>();
        Debug.Log("Stop Searching" + stopData);
        stopSearch = false;
    }

    //public void GroundTargetMove(SocketIOResponse res)
    //{
    //    try
    //    {
    //        string positionString = res.GetValue<string>();
    //        string[] positionParts = positionString.Split(',');

    //        if (positionParts.Length != 3)
    //        {
    //            Debug.LogError("Invalid position data received.");
    //            return;
    //        }

    //        // Parse the individual components back to float
    //        if (float.TryParse(positionParts[0], out float x) &&
    //            float.TryParse(positionParts[1], out float y) &&
    //            float.TryParse(positionParts[2], out float z))
    //        {
    //            MainThreadDispatcher.Enqueue(() =>
    //            {
    //                GroundTarget groundTarget = GameObject.FindObjectOfType<GroundTarget>();
    //                groundTarget.updatePosition(x, y, z);
    //            });
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to parse position components.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError($"Exception in GroundTargetMove: {ex.Message}");
    //    }
    //}

    public void GameStarted(SocketIOResponse res)
    {
        string responseData = res.GetValue<string>();
        Debug.Log("Game Started Response Data: " + responseData);
        Debug.Log("My Socket Id " + socket.Id);

        GameStartData gameStartData;
        try
        {
            gameStartData = JsonConvert.DeserializeObject<GameStartData>(responseData);
            if (gameStartData == null)
            {
                Debug.LogError("GameStartData is null after deserialization.");
                return;
            }

            if (gameStartData.roomId == null)
            {
                Debug.LogError("No roomId found");
                return;
            }

            if (gameStartData.users == null)
            {
                Debug.LogError("Users array is null.");
                return;
            }
            roomId = gameStartData.roomId;
            prizePool = gameStartData.prizePool;
            Debug.Log($"Number of users: {gameStartData.users.Length}");
            users = new User[gameStartData.users.Length];
            for (int i = 0; i < gameStartData.users.Length; i++)
            {
                Debug.Log($"User {i}: Socket ID = {gameStartData.users[i]}");
                User user;
                string socketId = gameStartData.users[i].socketId;
                string username = gameStartData.users[i].username;
                if (socket.Id != socketId)
                {
                    user = new User(socketId, username);
                }
                else
                {
                    user = new User(gameStartData.users[i].socketId, username, true);
                }
                //Debug.Log("userId " + user.userId);
                Debug.Log("socketId " + user.socketId);
                users[i] = user;
                Debug.Log("User pushed to array");
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                Debug.Log("LudoGameManager " + GM.game);
                //GM.Instance.InitializePlayers(users);
                Debug.Log("Starting player initialization...");
                GM.game.InitializePlayers(users);
                //SearchingScriptfor2Players.Searching.StopSearching();
            });
            //LudoGameManager.InitializePlayers(users);
            Debug.Log("Player initialization completed.");
            //Debug.Log("Search stopped, starting the game!");


        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
            return;
        }

        // Load the desired scene
        // SceneManager.LoadScene("classicludo");
    }

    //internal void OnPlayerTurn(SocketIOResponse res)
    //{
    //    //User currentUser = JsonConvert.DeserializeObject<User>(res.GetValue<string>());
    //    //Debug.Log("Received Player Turn for: " + currentUser?.userId);
    //    //GM.Instance.HandlePlayerTurn(currentUser);
    //    string socketId = res.GetValue<string>();
    //    Debug.Log("This is socketId" + socketId);
    //    //if (socketId != "") {
    //    //        Debug.Log("Instance for handle Player turn");
    //    //}
    //    Debug.Log("Received Player Turn for: " + socketId);
    //     LudoGameManager.HandlePlayerTurn(socketId);
    //    Debug.Log("Handle Player Turn for: " + socketId);
    //    //StartCoroutine(EnsurePlayerTurn(socketId));
    //}
    public void OnPlayerTurn(SocketIOResponse res)
    {
        // Extract the socketId of the player whose turn it is
        string socketId = res.GetValue<string>();

        // Log the event for debugging purposes
        Debug.Log("Received Player Turn for socketId: " + socketId);

        // Update the current turn in LudoGameManager

        // Log the updated current turn socketId for debugging
        //Debug.Log("Current turn updated to socketId: " + GM.currentTurnSocketId);

        // Call the function that handles the player's turn, using the main thread dispatcher
        MainThreadDispatcher.Enqueue(() =>
        {
            //GM.currentTurnSocketId = socketId;
            //Debug.Log("Handling player turn for socketId on the main thread.");

            GM.game.HandlePlayerTurn(socketId);
            //GM.game.UpdateArrowVisibilityForAllPlayers();
        });
    }





    //private IEnumerator EnsurePlayerTurn(string socketId)
    //{
    //    while (GM.game == null)
    //    {
    //        GM gm = GM.game ?? FindObjectOfType<GM>();
    //        if (gm != null)
    //        {
    //            Debug.Log("GM successfully found");
    //            break;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("GM not found");
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // Make sure GM instance is found before calling methods
    //    if (GM.game != null)
    //    {
    //        GM.game.HandlePlayerTurn(socketId);
    //    }
    //}

    //public void OnUpdateMove(SocketIOResponse res)
    //{
    //    string moveData = res.GetValue<string>();
    //    Debug.Log("Move updated: " + moveData);

    //    MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

    //    // Now you can access the 'payload' data through moveUpdate.payload
    //    if (moveUpdate != null && moveUpdate.payload != null)
    //    {
    //        Debug.Log($"Player {moveUpdate.payload.playerId} moved piece {moveUpdate.payload.pieceId} by {moveUpdate.payload.steps} steps.");

    //        // Implement your logic to update the player's position or move the piece
    //        GM.game.UpdatePlayerPosition(moveUpdate.payload);
    //        //StartCoroutine(GM.game.BotRoutine(moveUpdate.payload.playerId, moveUpdate.payload.pieceId, moveUpdate.payload.steps));
    //    }
    //    else
    //    {
    //        Debug.LogError("Error parsing move data");
    //    }

    //}

    public void OnUpdateMove(SocketIOResponse res)
    {
        string moveData = res.GetValue<string>();
        Debug.Log("Move updated: " + moveData);

        try
        {
            MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

            if (moveUpdate != null)
            {
                Debug.Log($"Player {moveUpdate.playerId} moved piece {moveUpdate.pieceId} to position {moveUpdate.piecePosition}. Points: {string.Join(",", moveUpdate.points)}");
                MainThreadDispatcher.Enqueue(() =>
                {
                    GM.game.MovePiece(moveUpdate);
                });
            }
            else
            {
                Debug.LogError("MoveUpdate deserialized as null");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deserializing MoveUpdate: " + e.Message);
        }
    }


    internal string getMySocketId()
    {
        return this.socket.Id;
    }

    //public void OnUpdateMove(SocketIOResponse res)
    //{
    //    string moveData = res.GetValue<string>();
    //    Debug.Log("Move updated: " + moveData);

    //    MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

    //    // Now you can access the 'payload' data through moveUpdate.payload
    //    if (moveUpdate != null && moveUpdate.payload != null)
    //    {
    //        Debug.Log($"Player {moveUpdate.payload.playerId} moved piece {moveUpdate.payload.pieceId} by {moveUpdate.payload.steps} steps.");

    //        // Implement your logic to update the player's position or move the piece
    //        GM.game.UpdatePlayerPosition(moveUpdate.payload);
    //        //StartCoroutine(GM.game.BotRoutine(moveUpdate.payload.playerId, moveUpdate.payload.pieceId, moveUpdate.payload.steps));
    //    }
    //    else
    //    {
    //        Debug.LogError("Error parsing move data");
    //    }

    //}


    private PP GetPlayerPieceFromMoveData(string moveData)
    {

        string[] dataParts = moveData.Split('_');
        if (dataParts.Length == 2)
        {
            string playerId = dataParts[0];
            string pieceId = dataParts[1];
            Debug.LogWarning("PlayerID" + playerId);
            Debug.LogWarning("PieceID" + pieceId);

        }

        return null;
    }

    public void OnKillPiece(SocketIOResponse res)
    {
        string killData = res.GetValue<string>();
        Debug.Log("Piece killed: " + killData);
        KillUpdate killUpdate = JsonUtility.FromJson<KillUpdate>(killData);
        if (killData != null)
        {
            Debug.Log($"Player {killUpdate.payload.playerId} moved piece {killUpdate.payload.pieceId} from position {killUpdate.payload.piecePosition}");
            MainThreadDispatcher.Enqueue(() =>
            {
                GM.game.KillPiece(killUpdate);
            });
        }
        else
        {
            Debug.LogError("Error parsing move data");
        }
    }



    public void OnDiceRolled(SocketIOResponse res)
    {
        string diceData = res.GetValue<string>();
        Debug.Log("Dice rolled data received: " + diceData);

        try
        {
            // Deserialize the incoming JSON data to get the payload
            var receivedData = JsonConvert.DeserializeObject<DiceRollData>(diceData);

            if (receivedData == null)
            {
                Debug.LogError("Received data is null.");
                return;
            }

            // Extract the necessary data from the payload
            string playerId = receivedData.playerId;
            int diceValue = receivedData.diceValue; // The value rolled on the dice

            // Log for debugging purposes
            Debug.Log($"Dice rolled: PlayerId = {playerId}, DiceValue = {diceValue}");

            // Store the dice value in SocketManager
            //RollingDice.RollDiceForBot();  // Assuming you still want to roll the dice for a bot
            SetDiceValue(diceValue);


            // Enqueue the player position update to be executed on the main thread
            MainThreadDispatcher.Enqueue(() =>
            {
                GM.game.rolingDice.RollDiceForBot();
                //UpdatePlayerPosition(playerId, diceValue);  // Update position based on dice value
            });
        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unexpected error occurred: " + ex.Message);
        }
    }

    public void OnWinner(SocketIOResponse res)
    {
        string[] winnerData = res.GetValue<string[]>(); // Parse the response as an array
        if (winnerData != null && winnerData.Length > 0)
        {
            string winnerId = winnerData[0]; // Get the winner's ID
            Debug.Log("Winner data received: " + winnerId);
            MainThreadDispatcher.Enqueue(() =>
            {
                GM.game.HandleGameWinner(winnerId); // Call the game winner handler with the ID
            });

        }
        else
        {
            Debug.LogError("Invalid winner data received!");
        }
    }


    //private void UpdatePlayerPosition(string playerId, int diceValue)
    //{
    //    // Implement the logic to update the player's position based on the dice value
    //    // For example, move the player by the diceValue steps on the board
    //    Debug.Log($"Updating position for Player {playerId} with dice value: {diceValue}");
    //}


    /// ClassicLudo....
    ///



    public void ClassicOnStopSearch(SocketIOResponse res)
    {
        string stopData = res.GetValue<string>();
        Debug.Log("Stop Searching" + stopData);
        stopSearch = false;
    }

    public void ClassicGameStarted(SocketIOResponse res)
    {
        string responseData = res.GetValue<string>();
        Debug.Log("Game Started Response Data: " + responseData);
        Debug.Log("My Socket Id " + socket.Id);

        GameStartData gameStartData;
        try
        {
            gameStartData = JsonConvert.DeserializeObject<GameStartData>(responseData);
            if (gameStartData == null)
            {
                Debug.LogError("GameStartData is null after deserialization.");
                return;
            }

            if (gameStartData.roomId == null)
            {
                Debug.LogError("No roomId found");
                return;
            }

            if (gameStartData.users == null)
            {
                Debug.LogError("Users array is null.");
                return;
            }
            roomId = gameStartData.roomId;
            Debug.Log($"Number of users: {gameStartData.users.Length}");
            users = new User[gameStartData.users.Length];
            for (int i = 0; i < gameStartData.users.Length; i++)
            {
                Debug.Log($"User {i}: Socket ID = {gameStartData.users[i]}");
                User user;
                string socketId = gameStartData.users[i].socketId;
                string username = gameStartData.users[i].username;
                if (socket.Id != socketId)
                {
                    user = new User(gameStartData.users[i].socketId, username);
                }
                else
                {
                    user = new User(gameStartData.users[i].socketId,username, true);
                }
                //Debug.Log("userId " + user.userId);
                Debug.Log("socketId " + user.socketId);
                users[i] = user;
                Debug.Log("User pushed to array");
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                Debug.Log("LudoGameManager " + GM.game);
                //GM.Instance.InitializePlayers(users);
                Debug.Log("Starting player initialization...");
                ClassicLudoGM.game.InitializePlayers(users);
                //SearchingScriptfor2Players.Searching.StopSearching();
            });
            //LudoGameManager.InitializePlayers(users);
            Debug.Log("Player initialization completed.");
            //Debug.Log("Search stopped, starting the game!");


        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
            return;
        }

        // Load the desired scene
        // SceneManager.LoadScene("classicludo");
    }

    //internal void OnPlayerTurn(SocketIOResponse res)
    //{
    //    //User currentUser = JsonConvert.DeserializeObject<User>(res.GetValue<string>());
    //    //Debug.Log("Received Player Turn for: " + currentUser?.userId);
    //    //GM.Instance.HandlePlayerTurn(currentUser);
    //    string socketId = res.GetValue<string>();
    //    Debug.Log("This is socketId" + socketId);
    //    //if (socketId != "") {
    //    //        Debug.Log("Instance for handle Player turn");
    //    //}
    //    Debug.Log("Received Player Turn for: " + socketId);
    //     LudoGameManager.HandlePlayerTurn(socketId);
    //    Debug.Log("Handle Player Turn for: " + socketId);
    //    //StartCoroutine(EnsurePlayerTurn(socketId));
    //}
    public void ClassicOnPlayerTurn(SocketIOResponse res)
    {
        // Extract the socketId of the player whose turn it is
        string socketId = res.GetValue<string>();

        // Log the event for debugging purposes
        Debug.Log("Received Player Turn for socketId: " + socketId);

        // Update the current turn in LudoGameManager

        // Log the updated current turn socketId for debugging
        //Debug.Log("Current turn updated to socketId: " + GM.currentTurnSocketId);

        // Call the function that handles the player's turn, using the main thread dispatcher
        MainThreadDispatcher.Enqueue(() =>
        {
            //GM.currentTurnSocketId = socketId;
            //Debug.Log("Handling player turn for socketId on the main thread.");

            ClassicLudoGM.game.HandlePlayerTurn(socketId);
            //GM.game.UpdateArrowVisibilityForAllPlayers();
        });
    }





    //private IEnumerator EnsurePlayerTurn(string socketId)
    //{
    //    while (GM.game == null)
    //    {
    //        GM gm = GM.game ?? FindObjectOfType<GM>();
    //        if (gm != null)
    //        {
    //            Debug.Log("GM successfully found");
    //            break;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("GM not found");
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // Make sure GM instance is found before calling methods
    //    if (GM.game != null)
    //    {
    //        GM.game.HandlePlayerTurn(socketId);
    //    }
    //}

    //public void OnUpdateMove(SocketIOResponse res)
    //{
    //    string moveData = res.GetValue<string>();
    //    Debug.Log("Move updated: " + moveData);

    //    MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

    //    // Now you can access the 'payload' data through moveUpdate.payload
    //    if (moveUpdate != null && moveUpdate.payload != null)
    //    {
    //        Debug.Log($"Player {moveUpdate.payload.playerId} moved piece {moveUpdate.payload.pieceId} by {moveUpdate.payload.steps} steps.");

    //        // Implement your logic to update the player's position or move the piece
    //        GM.game.UpdatePlayerPosition(moveUpdate.payload);
    //        //StartCoroutine(GM.game.BotRoutine(moveUpdate.payload.playerId, moveUpdate.payload.pieceId, moveUpdate.payload.steps));
    //    }
    //    else
    //    {
    //        Debug.LogError("Error parsing move data");
    //    }

    //}

    public void ClassicOnUpdateMove(SocketIOResponse res)
    {
        string moveData = res.GetValue<string>();
        Debug.Log("Move updated: " + moveData);

        try
        {
            MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

            if (moveUpdate != null)
            {
                Debug.Log($"Player {moveUpdate.playerId} moved piece {moveUpdate.pieceId} to position {moveUpdate.piecePosition}. Points: {string.Join(",", moveUpdate.points)}");
                MainThreadDispatcher.Enqueue(() =>
                {
                    ClassicLudoGM.game.MovePiece(moveUpdate);
                });
            }
            else
            {
                Debug.LogError("MoveUpdate deserialized as null");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deserializing MoveUpdate: " + e.Message);
        }
    }


    //internal string getMySocketId()
    //{
    //    return this.socket.Id;
    //}

    //public void OnUpdateMove(SocketIOResponse res)
    //{
    //    string moveData = res.GetValue<string>();
    //    Debug.Log("Move updated: " + moveData);

    //    MoveUpdate moveUpdate = JsonUtility.FromJson<MoveUpdate>(moveData);

    //    // Now you can access the 'payload' data through moveUpdate.payload
    //    if (moveUpdate != null && moveUpdate.payload != null)
    //    {
    //        Debug.Log($"Player {moveUpdate.payload.playerId} moved piece {moveUpdate.payload.pieceId} by {moveUpdate.payload.steps} steps.");

    //        // Implement your logic to update the player's position or move the piece
    //        GM.game.UpdatePlayerPosition(moveUpdate.payload);
    //        //StartCoroutine(GM.game.BotRoutine(moveUpdate.payload.playerId, moveUpdate.payload.pieceId, moveUpdate.payload.steps));
    //    }
    //    else
    //    {
    //        Debug.LogError("Error parsing move data");
    //    }

    //}


    private ClassicLudoPP ClassicGetPlayerPieceFromMoveData(string moveData)
    {

        string[] dataParts = moveData.Split('_');
        if (dataParts.Length == 2)
        {
            string playerId = dataParts[0];
            string pieceId = dataParts[1];
            Debug.LogWarning("PlayerID" + playerId);
            Debug.LogWarning("PieceID" + pieceId);

        }

        return null;
    }

    public void ClassicOnKillPiece(SocketIOResponse res)
    {
        string killData = res.GetValue<string>();
        Debug.Log("Piece killed: " + killData);
        KillUpdate killUpdate = JsonUtility.FromJson<KillUpdate>(killData);
        if (killData != null)
        {
            Debug.Log($"Player {killUpdate.payload.playerId} moved piece {killUpdate.payload.pieceId} from position {killUpdate.payload.piecePosition}");
            MainThreadDispatcher.Enqueue(() =>
            {
                ClassicLudoGM.game.KillPiece(killUpdate);
            });
        }
        else
        {
            Debug.LogError("Error parsing move data");
        }
    }



    public void ClassicOnDiceRolled(SocketIOResponse res)
    {
        string diceData = res.GetValue<string>();
        Debug.Log("Dice rolled data received: " + diceData);

        try
        {
            // Deserialize the incoming JSON data to get the payload
            var receivedData = JsonConvert.DeserializeObject<DiceRollData>(diceData);

            if (receivedData == null)
            {
                Debug.LogError("Received data is null.");
                return;
            }

            // Extract the necessary data from the payload
            string playerId = receivedData.playerId;
            int diceValue = receivedData.diceValue; // The value rolled on the dice

            // Log for debugging purposes
            Debug.Log($"Dice rolled: PlayerId = {playerId}, DiceValue = {diceValue}");

            // Store the dice value in SocketManager
            //RollingDice.RollDiceForBot();  // Assuming you still want to roll the dice for a bot
            SetDiceValue(diceValue);


            // Enqueue the player position update to be executed on the main thread
            MainThreadDispatcher.Enqueue(() =>
            {
                ClassicLudoGM.game.rolingDice.RollDiceForBot();
                //UpdatePlayerPosition(playerId, diceValue);  // Update position based on dice value
            });
        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unexpected error occurred: " + ex.Message);
        }
    }

    public void ClassicOnWinner(SocketIOResponse res)
    {
        string[] winnerData = res.GetValue<string[]>(); // Parse the response as an array
        if (winnerData != null && winnerData.Length > 0)
        {
            string winnerId = winnerData[0]; // Get the winner's ID
            Debug.Log("Winner data received: " + winnerId);
            MainThreadDispatcher.Enqueue(() =>
            {
                ClassicLudoGM.game.HandleGameWinner(winnerId); // Call the game winner handler with the ID
            });

        }
        else
        {
            Debug.LogError("Invalid winner data received!");
        }
    }


    // MIND_MORGA

    public void MindMorgaGameStarted(SocketIOResponse res)
    {
        string responseData = res.GetValue<string>();
        Debug.Log("Game Started Response Data: " + responseData);
        Debug.Log("My Socket Id " + socket.Id);

        GameStartData gameStartData;
        try
        {
            gameStartData = JsonConvert.DeserializeObject<GameStartData>(responseData);
            if (gameStartData == null)
            {
                Debug.LogError("GameStartData is null after deserialization.");
                return;
            }

            if (gameStartData.roomId == null)
            {
                Debug.LogError("No roomId found");
                return;
            }

            if (gameStartData.users == null)
            {
                Debug.LogError("Users array is null.");
                return;
            }
            roomId = gameStartData.roomId;
            prizePool = gameStartData.prizePool;
            Debug.Log($"Number of users: {gameStartData.users.Length}");
            users = new User[gameStartData.users.Length];
            for (int i = 0; i < gameStartData.users.Length; i++)
            {
                Debug.Log($"User {i}: Socket ID = {gameStartData.users[i]}");
                User user;
                string socketId = gameStartData.users[i].socketId;
                string username = gameStartData.users[i].username;
                if (socket.Id != socketId)
                {
                    user = new User(socketId, username);
                }
                else
                {
                    user = new User(gameStartData.users[i].socketId, username, true);
                }
                //Debug.Log("userId " + user.userId);
                Debug.Log("socketId " + user.socketId);
                users[i] = user;
                Debug.Log("User pushed to array");
            }

            MainThreadDispatcher.Enqueue(() =>
            {
                Debug.Log("MindMorgaGameManager " + MindMorgaGameController.Mindgame);
                //GM.Instance.InitializePlayers(users);
                Debug.Log("Starting player initialization...");
                //MindMorgaGameController = GameObject.FindObjectOfType<MindMorgaGameController>();
                MindMorgaGameController.Mindgame.InitializePlayers(users);
                //SearchingScriptfor2Players.Searching.StopSearching();
            });
            //LudoGameManager.InitializePlayers(users);
            Debug.Log("Player initialization completed.");
            //Debug.Log("Search stopped, starting the game!");


        }
        catch (JsonException jsonEx)
        {
            Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
            return;
        }

        // Load the desired scene
        // SceneManager.LoadScene("classicludo");
    }

    public void MindMorgaOnPlayerTurn(SocketIOResponse res)
    {
        string socketId = res.GetValue<string>();

        // Log the event for debugging purposes
        Debug.Log("Received Player Turn for socketId: " + socketId);


        MainThreadDispatcher.Enqueue(() =>
        {
            MindMorgaGameController.Mindgame.HandlePlayerTurn(socketId);
        });
    }

    public void OpenCard(SocketIOResponse res)
    {
        string cardDataJson = res.GetValue<string>();
        Debug.Log("Card data received: " + cardDataJson);

        var jsonData = JsonConvert.DeserializeObject<CardData>(cardDataJson);

        if (jsonData != null)
        {
            // Store the card data
            SetCardData(jsonData.index, jsonData.card);
            Debug.Log($"Stored card data: Index = {jsonData.index}, Card = {jsonData.card}");

            // Load and display the card
            MainThreadDispatcher.Enqueue(() =>
            {
                MindMorgaGameController.Mindgame.LoadCardSprite();
                Debug.Log("Card data stored and displayed.");
            });
        }
        else
        {
            Debug.LogError("Failed to deserialize card data.");
        }
    }


    public void CloseCard(SocketIOResponse res)
    {
        string cardData = res.GetValue<string>();
        Debug.Log("Close card data received: " + cardData);
        var jsonData = JsonUtility.FromJson<CloseCardsData>(cardData.ToString());
        MainThreadDispatcher.Enqueue(() =>
        {
            MindMorgaGameController.Mindgame.CloseCardSprite(jsonData.index1, jsonData.index2);
            Debug.Log("Closing card data Received");
        });
    }

    public void CardsMatched(SocketIOResponse res)
    {
        string cardData = res.GetValue<string>();
        Debug.Log("Card matched data received: " + cardData);
        var jsonData = JsonUtility.FromJson<MatchCardsData>(cardData.ToString());
        MainThreadDispatcher.Enqueue(() =>
        {
            MindMorgaGameController.Mindgame.DisableMatchedCards(jsonData.index1, jsonData.index2, jsonData.score1, jsonData.score2);
            Debug.Log("Match card data Received");
        });
    }

    public void EndGame(SocketIOResponse res)
    {
        string cardData = res.GetValue<string>();
        Debug.Log("Card matched data received: " + cardData);
        var jsonData = JsonUtility.FromJson<EndCardsData>(cardData.ToString());
        MainThreadDispatcher.Enqueue(() =>
        {
            MindMorgaGameController.Mindgame.EndGame(jsonData.winnerId, jsonData.score1, jsonData.score2);
            Debug.Log("Match card data Received");
        });
    }

    [System.Serializable]
    public class CardData
    {
        public int index;
        public string card;

        public CardData(int index, string cardName)
        {
            this.index = index;
            this.card = cardName;
        }
    }


    [System.Serializable]
    public class CloseCardsData
    {
        public int index1;  // Index of the first card
        public int index2;  // Index of the second card
    }

    [System.Serializable]
    public class MatchCardsData
    {
        public int index1;  // Index of the first matched card
        public int index2;  // Index of the second matched card
        public int score1;  // Index of the first matched card
        public int score2;  // Index of the second matched card
    }

    [System.Serializable]
    public class EndCardsData
    {
        public string winnerId;
        public int score1;  // Index of the first matched card
        public int score2;  // Index of the second matched card
    }


    public string GetRoomId()
    {
        return roomId;
    }

    internal int diceValue;

    public void SetDiceValue(int value)
    {
        diceValue = value;
    }

    public int GetDiceValue()
    {
        return diceValue;
    }

    private CardData storedCardData;

    public void SetCardData(int index, string name)
    {
        storedCardData = new CardData(index, name);
    }

    public CardData GetCardData()
    {
        return storedCardData;
    }




    public int GetSteps()
    {
        return steps;
    }



    public string GetSocketId()
    {
        return socketId;
    }

    private void OnDestroy()
    {
        if (socket != null)
        {
            socket.Disconnect();
            socket.Dispose();
        }
    }
    public User[] getUsers()
    {
        return this.users;
    }

    public float getPrizePool()
    {
        return this.prizePool;
    }

    //public class DiceRollData
    //{
    //    public Payload payload;

    //    [Serializable]
    //    public class Payload
    //    {
    //        public string roomId;
    //        public string playerId;
    //        public int steps;
    //    }
    //}

    public class DiceRollData
    {
        public string playerId { get; set; }
        public int diceValue { get; set; }

        // Constructor to easily create instances
        public DiceRollData(string playerId, int diceValue)
        {
            this.playerId = playerId;
            this.diceValue = diceValue;
        }
    }

    


    [Serializable]
    public class GameStartData
    {
        public string roomId;
        public User[] users;
        public float prizePool;
    }

    [Serializable]
    public class User
    {
        public string socketId;
        public string username;
        public bool isCurrent;

        public User(string socketId,string username, bool isCurrent = false)
        {
            this.socketId = socketId;
            this.username = username;
            this.isCurrent = isCurrent;
        }

        public string getSocketId()
        {
            return this.socketId;
        }

    }

    //[System.Serializable]
    //public class MoveData
    //{
    //    public string playerId;
    //    public int pieceId;
    //    //public int steps;
    //    public int piecePosition;
    //}

    [System.Serializable]
    public class MoveUpdate
    {
        public string playerId;
        public int pieceId;
        public int piecePosition;
        public List<int> points; // Use List<int> for JSON arrays
    }


    [System.Serializable]
    public class killData
    {
        public string playerId;
        public int pieceId;
        //public int steps;
        public int piecePosition;
    }

    [System.Serializable]
    public class KillUpdate
    {
        public killData payload;
    }

    //Cricket...

    //[Serializable]
    //public class CricGameStartData
    //{
    //    public string roomId;
    //    public CricketUser[] users;
    //}

    //[Serializable]
    //public class CricketUser
    //{
    //    public string socketId;
    //    public bool isBatting;

    //    public CricketUser(string socketId, bool isBatting)
    //    {
    //        this.socketId = socketId;
    //        this.isBatting = isBatting;
    //    }

    //    public string getSocketId()
    //    {
    //        return this.socketId;
    //    }

    //    public bool getIsBatting()
    //    {
    //        return this.isBatting;
    //    }
    //}

    

    //private bool isPlayer1;
    //private bool isPlayer2;


    //public GameController gamecontroller;

    //public CricNetManager cricnetmanager;

    //public GameObject WinPanel;
    //public GameObject DrawPanel;
    //public GameObject LosePanel;



    //public CricketUser[] cricusers;

    //private string playerId;  // Store the player role/ID (Player1 or Player2)

    //public string PlayerId
    //{
    //    get
    //    {
    //        return playerId;
    //    }
    //}

    //private void OnAssignRole(SocketIOResponse response)        //START CRICKET GAME
    //{
    //    string responseData = response.GetValue<string>();
    //    Debug.Log("Game Started Response Data: " + responseData);
    //    Debug.Log("My Socket Id " + socket.Id);
    //    CricGameStartData gameStartData;



    //    try
    //    {
    //        gameStartData = JsonConvert.DeserializeObject<CricGameStartData>(responseData);
    //        if (gameStartData == null)
    //        {
    //            Debug.LogError("GameStartData is null after deserialization.");
    //            return;
    //        }
    //        if (gameStartData.roomId == null)
    //        {
    //            Debug.LogError("No roomId found");
    //            return;
    //        }

    //        if (gameStartData.users == null)
    //        {
    //            Debug.LogError("Users array is null.");
    //            return;
    //        }
    //        roomId = gameStartData.roomId;
    //        CricketUser user1 = gameStartData.users[0];
    //        CricketUser user2 = gameStartData.users.Length > 1 ? gameStartData.users[1] : null;
    //        if (user1.getSocketId() == socket.Id)
    //        {
    //            playerId = "Player1";
    //            isPlayer1 = true;
    //            isPlayer2 = false;
    //            CricNetManager.Instance.isPlayer1 = true; // assign socket player1 to netmanager player1
    //            CricNetManager.Instance.isPlayer2 = false;

    //        }
    //        else if (user2 != null && user2.getSocketId() == socket.Id)
    //        {
    //            playerId = "Player2";
    //            isPlayer1 = false;
    //            isPlayer2 = true;
    //            CricNetManager.Instance.isPlayer1 = false;
    //            CricNetManager.Instance.isPlayer2 = true; // assign socket player2 to netmanager player2


    //        }

    //        CricNetManager.Instance.Player1SocketId = user1.socketId;
    //        CricNetManager.Instance.Player2SocketId = user2?.socketId;


    //    }
    //    catch (JsonException jsonEx)
    //    {
    //        Debug.LogError("JSON Parsing Error: " + jsonEx.Message);
    //        return;
    //    }
    //}

    //public void ServerPlayerRole(string Id)
    //{
    //    var data = new
    //    {
    //        playerRole = Id
    //    };

    //    string jsonData = JsonConvert.SerializeObject(data); // Convert object to JSON string
    //    socket.Emit("playerId_assigned", jsonData);          // Send the JSON string to the server
    //}


    //public void OnBowlerRun(SocketIOResponse res)
    //{
    //    Debug.Log("This is debug for run");
    //    string speed = res.GetValue<string>();
    //    float runSpeed = float.Parse(speed);
    //    MainThreadDispatcher.Enqueue(() =>
    //    {

    //        bowlplayer = GameObject.FindObjectOfType<BowlPlayer>();
    //        bowlplayer.StartRunFromSocket(runSpeed);

    //    });
    //}

    ////var data = new
    ////{
    ////    adjustedValue = adjustedValue,
    ////    predictedX = predictedX

    ////};

    //private void OnBatsManMoved(SocketIOResponse res)
    //{
    //    string data = res.GetValue<string>();
    //    float predictedX = float.Parse(data);

    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        batsmanplayer = GameObject.FindObjectOfType<BatsmanPlayer>();
    //        batsmanplayer.MoveBatsman2(predictedX);

    //        if (predictedX < batsmanplayer.transform.position.x)
    //        {
    //            batsmanplayer.anim.Play("Right");
    //        }
    //        else if (predictedX > batsmanplayer.transform.position.x)
    //        {
    //            batsmanplayer.anim.Play("Left");
    //        }
    //        else
    //        {
    //            batsmanplayer.anim.Play("Idle");
    //        }
    //    });
    //}

    //public void OnBatsManHit(SocketIOResponse res)
    //{
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        batsmanplayer = GameObject.FindObjectOfType<BatsmanPlayer>();
    //        batsmanplayer.HittingBatFromSocket();
    //    });

    //}

    //public void onCricketIdle(SocketIOResponse res)
    //{
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        batsmanplayer = GameObject.FindObjectOfType<BatsmanPlayer>();
    //        batsmanplayer.isIdle = true;
    //        batsmanplayer.anim.Play("Idle");
    //    });
    //}

    //public void OnResetBowler(SocketIOResponse res)
    //{
    //    MainThreadDispatcher.Enqueue(() =>
    //    {

    //        bowlplayer = GameObject.FindObjectOfType<BowlPlayer>();
    //        bowlplayer.RestartFromSocket();

    //    });
    //}

    //public void OnBatsManReset(SocketIOResponse res)
    //{
    //    //RESET_BATSMAN
    //    Debug.Log("Calling batsman Reset");
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        batsmanplayer = GameObject.FindObjectOfType<BatsmanPlayer>();
    //        batsmanplayer.ResetBatsmanFromSocket();
    //    });
    //}


    //public void OnWicket(SocketIOResponse res)
    //{
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        scoremanager = GameObject.FindObjectOfType<ScoreManager>();
    //        scoremanager.HitStumpsFromSocket();
    //        batcontroller.StumpsCollided();
    //        bowlcontroller.StumpsCollided();
    //    });
    //}

    //public void OnUpdateScore(SocketIOResponse res)
    //{
    //    Debug.Log("Update score is called");
    //    string batsManData = res.GetValue<string>();
    //    int score = int.Parse(batsManData);

    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        scoremanager = GameObject.FindObjectOfType<ScoreManager>();
    //        if (score == 0)
    //        {
    //            scoremanager.BallMissedFromSocket();
    //            return;
    //        }
    //        else
    //        {
    //            scoremanager.updateScoreFromSocket(score);
    //        }
    //    });
    //}

    //public void onBallHit(SocketIOResponse res)
    //{
    //    string data = res.GetValue<string>();
    //    float random = float.Parse(data);

    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        Bat bat = GameObject.FindObjectOfType<Bat>();
    //        bat.ShootBall(random);  // BALL HIT
    //    });
    //}

    //public void OnSwitchCamera(SocketIOResponse res)
    //{
    //    Debug.Log("Switch the camera");
    //    GameController.instance.StartNextGame();

    //}


    //public void OnCricWinner(SocketIOResponse res)
    //{
    //    string winnerId = res.GetValue<string>();
    //    Debug.Log("This is winnerId: " + winnerId);
    //    MainThreadDispatcher.Enqueue(() =>
    //    {
    //        StartCoroutine(ShowPanel(2f));
    //    });
    //}

    //private IEnumerator ShowPanel(float waitsecs)
    //{
    //    yield return new WaitForSeconds(waitsecs);

    //    // PLAYER1
    //    if (cricnetmanager.isPlayer1 && gamecontroller.player1score > gamecontroller.player2score)
    //    {
    //        WinPanel.SetActive(true);
    //    }
    //    else if (cricnetmanager.isPlayer1 && gamecontroller.player1score < gamecontroller.player2score)
    //    {
    //        LosePanel.SetActive(true);
    //    }
    //    else if (cricnetmanager.isPlayer1 && gamecontroller.player1score == gamecontroller.player2score)
    //    {
    //        DrawPanel.SetActive(true);
    //    }

    //    // PLAYER2
    //    if (cricnetmanager.isPlayer2 && gamecontroller.player2score > gamecontroller.player1score)
    //    {
    //        WinPanel.SetActive(true);
    //    }
    //    else if (cricnetmanager.isPlayer2 && gamecontroller.player2score < gamecontroller.player1score)
    //    {
    //        LosePanel.SetActive(true);
    //    }
    //    else if (cricnetmanager.isPlayer2 && gamecontroller.player2score == gamecontroller.player1score)
    //    {
    //        DrawPanel.SetActive(true);
    //    }
    //}

    

}