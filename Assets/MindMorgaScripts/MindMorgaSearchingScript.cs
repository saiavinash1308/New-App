using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MindMorgaSearchScript : MonoBehaviour
{
    public bool isSearching;
    public Image loadingImage;
    public static MindMorgaSearchScript Searching { get; private set; }
    private SocketManager socketManager;
    public GameObject Loading;
    public GameObject PopUp;
    public Button Back;
    public Button Quit;
    public Button Cancle;

    void Awake()
    {
        Searching = this;
        socketManager = FindObjectOfType<SocketManager>();
    }

    void Start()
    {
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }

        isSearching = true;
        StartCoroutine(SearchAndLoadCoroutine());
    }

    private IEnumerator SearchAndLoadCoroutine()
    {
        loadingImage.gameObject.SetActive(true); // Show loading image
        Loading.gameObject.SetActive(true);
        while (socketManager != null && socketManager.stopSearch)
        {
            loadingImage.fillAmount = Mathf.PingPong(Time.time, 1f); // Smooth fill between 0 and 1

            yield return new WaitForSeconds(2f);
        }

        loadingImage.gameObject.SetActive(false);
        Loading.gameObject.SetActive(false);
        SceneManager.LoadScene("MindMorga");
    }
    public void StopSearching()
    {
        Debug.LogWarning("Searching Stopped...");
        isSearching = false;
    }

    public void EnablePopUp()
    {
        PopUp.SetActive(true);
    }

    public void QuitToHome()
    {
        if (socketManager != null && socketManager.isConnected)
        {
            socketManager.socket.Emit("QUIT_GAME", " ");
            Debug.Log("Sent game quit");
            SceneManager.LoadScene("Home");
        }
        else
        {
            Debug.LogWarning("Socket is not connected. Cannot send game ID.");
        }
    }
    public void ClosePopup()
    {
        if (PopUp != null)
        {
            PopUp.SetActive(false);
        }
    }
}
