using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SocketManager;

public class MindMorgaGameController : MonoBehaviour
{
    public static MindMorgaGameController Mindgame { get; private set; }

    [SerializeField]
    private Sprite bgImage;

    public Sprite[] images;  // Pre-loaded images from Resources
    public List<Button> btns = new List<Button>();



    public Text player1Score;
    public Text player2Score;

    public Text player1FinalScore;
    public Text player2FinalScore;

    public Text Player1UserName;
    public Text Player2UserName;
    public Text prizePool;

    public GameObject WinnerPopUp;
    public GameObject LoosePopUp;

    [SerializeField] private RectTransform arrowPlayer1;
    [SerializeField] private RectTransform arrowPlayer2;
    private Vector3 originalScale = Vector3.one;
    private Vector3 enlargedScale = new Vector3(1.5f, 1.5f, 1.5f);

    public User[] players;
    public static string currentTurnSocketId;
    public string turnSocketId;
    [SerializeField]
    private SocketManager socketManager;
    private bool isActive = false;
    public float fadeDuration = 2.0f; // Duration of the fade effect
    public float waitBeforeRedirect = 5.0f; // Time before redirecting to home

    public GameObject PopUp;


    void Awake()
    {
        images = Resources.LoadAll<Sprite>("Sprites/Mind");
        socketManager = FindObjectOfType<SocketManager>();
        Mindgame = this;
    }

    private void Start()
    {
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return;
        }

        // Initialize scores to 0
        player1Score.text = "0";
        player2Score.text = "0";
        player1FinalScore.text = "0";
        player2FinalScore.text = "0";
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
    }

    private void UpdatePlayerUI()
    {
        if (players == null || players.Length < 2)
        {
            Debug.LogError("Invalid number of players. The game requires at least 2 players.");
            return;
        }

        Player1UserName.text = players[0].username;
        Player2UserName.text = players[1].username;
        prizePool.text = socketManager.getPrizePool().ToString();
    }

    public void HandlePlayerTurn(string socketId)
    {
        StartCoroutine(HandleTurnCoroutine(socketId));
    }

    private IEnumerator HandleTurnCoroutine(string socketId)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].socketId == socketId)
            {
                UpdateTurnArrow(i);
                yield return new WaitForSeconds(0.5f);
                break;
            }
        }
        turnSocketId = socketId;
    }

    private void UpdateTurnArrow(int playerIndex)
    {
        arrowPlayer1.localScale = originalScale;
        arrowPlayer2.localScale = originalScale;

        if (playerIndex == 0)
        {
            arrowPlayer1.localScale = enlargedScale;
        }
        else if (playerIndex == 1)
        {
            arrowPlayer2.localScale = enlargedScale;
        }
    }

    public void GetStarted()
    {
        GetButtons();
        AddListeners();
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameButton");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => ClickButton());
        }
    }

    public void ClickButton()
    {
        if (isActive)
            return;
        if (socketManager != null && socketManager.isConnected)
        {
            if (socketManager.getMySocketId() == turnSocketId)
            {
                GameObject selectedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                string buttonName = selectedObject.name;
                Debug.LogError(selectedObject.name);

                isActive = true;

                // Emit the "PICK_CARD" event
                socketManager.socket.Emit("PICK_CARD", buttonName);
                Debug.Log($"PICK_CARD emitted for {buttonName}");


            }
        }
        else
        {
            Debug.LogWarning("SocketManager is not connected. Cannot emit PICK_CARD.");
        }
    }

    public void LoadCardSprite()
    {
        isActive = false;

        // Retrieve stored card data
        CardData cardData = socketManager.GetCardData();
        if (cardData == null)
        {
            Debug.LogWarning("No stored card data available.");
            return;
        }

        int index = cardData.index;
        string cardName = cardData.card;

        if (index < 0 || index >= btns.Count)
        {
            Debug.LogWarning($"Invalid card index: {index}");
            return;
        }

        // Find the button at the given index
        Button button = btns[index];
        GameObject selectedObject = button.gameObject;

        // Activate child object and disable Image component
        selectedObject.transform.GetChild(0).gameObject.SetActive(true);
        //selectedObject.GetComponent<Image>().enabled = false;

        // Load the sprite with the corresponding name
        Sprite sprite = System.Array.Find(images, img => img.name == cardName);
        if (sprite != null)
        {
            button.image.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"Sprite {cardName} not found in the loaded images!");
        }

        Debug.Log($"Card displayed: Index = {index}, Card = {cardName}");

        // Start coroutine to revert changes after 1 second
        StartCoroutine(RevertCardAppearance(selectedObject));
    }

    private IEnumerator RevertCardAppearance(GameObject selectedObject)
    {
        yield return new WaitForSeconds(0.5f);

        // Revert changes
        selectedObject.transform.GetChild(0).gameObject.SetActive(false);
        //selectedObject.GetComponent<Image>().enabled = true;
    }




    public void CloseCardSprite(int index1, int index2)
    {
        if (index1 < 0 || index1 >= btns.Count || index2 < 0 || index2 >= btns.Count)
        {
            Debug.LogWarning($"Invalid indices: {index1}, {index2}");
            return;
        }

        Button firstButton = btns[index1];
        Button secondButton = btns[index2];

        GameObject firstObject = firstButton.gameObject;
        GameObject secondObject = secondButton.gameObject;

        // Activate child object and disable Image component before closing
        firstObject.transform.GetChild(0).gameObject.SetActive(true);
        //firstObject.GetComponent<Image>().enabled = false;

        secondObject.transform.GetChild(0).gameObject.SetActive(true);
        //secondObject.GetComponent<Image>().enabled = false;

        Debug.Log($"Cards closing: {index1}, {index2}");

        // Start a single coroutine to handle both cards at the same time
        StartCoroutine(RevertClosedCards(firstObject, firstButton, secondObject, secondButton));

        if (socketManager != null && socketManager.isConnected)
        {
            if (socketManager.getMySocketId() == turnSocketId)
            {
                socketManager.socket.Emit("UPDATE_TURN", "");
                Debug.Log($"Update turn emitted");
            }
        }
    }

    private IEnumerator RevertClosedCards(GameObject firstObject, Button firstButton, GameObject secondObject, Button secondButton)
    {
        yield return new WaitForSeconds(0.5f);

        // Reset the sprite and revert changes for both cards
        firstButton.image.sprite = bgImage;
        firstObject.transform.GetChild(0).gameObject.SetActive(false);
        //firstObject.GetComponent<Image>().enabled = true;

        secondButton.image.sprite = bgImage;
        secondObject.transform.GetChild(0).gameObject.SetActive(false);
        //secondObject.GetComponent<Image>().enabled = true;

        Debug.Log("Both cards reset at the same time.");
    }




    public void DisableMatchedCards(int index1, int index2, int score1, int score2)
    {
        // Disable the first matched card
        Button firstButton = btns[index1];
        if (firstButton != null)
        {
            firstButton.gameObject.SetActive(false);  // Disable the GameObject
        }

        // Disable the second matched card
        Button secondButton = btns[index2];
        if (secondButton != null)
        {
            secondButton.gameObject.SetActive(false);  // Disable the GameObject
        }

        Debug.Log("Matched cards disabled: " + index1 + ", " + index2);

        player1Score.text = score1.ToString();
        player2Score.text = score2.ToString();
    }

    public void EndGame(string winnerId, int score1, int score2)
    {
        player1FinalScore.text = score1.ToString();
        player2FinalScore.text = score2.ToString();

        if (socketManager.socket.Id == winnerId)
        {
            StartCoroutine(ShowPopup(WinnerPopUp, "You Win!"));
        }
        else
        {
            StartCoroutine(ShowPopup(LoosePopUp, "You Lose!"));
        }
    }

    private IEnumerator ShowPopup(GameObject popup, string resultMessage)
    {
        popup.SetActive(true);

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popup.AddComponent<CanvasGroup>(); // Add CanvasGroup if not present
        }

        // Set initial alpha to 0
        canvasGroup.alpha = 0f;

        // Find result text inside the popup and update it
        Text popupText = popup.GetComponentInChildren<Text>();
        if (popupText != null)
        {
            popupText.text = resultMessage;
        }

        float elapsedTime = 0f;

        // Fade-in effect
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure full visibility

        // Wait before redirecting
        yield return new WaitForSeconds(waitBeforeRedirect);

        // Load home scene
        SceneManager.LoadScene("Home"); // Replace "HomeScene" with the actual scene name
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



}
