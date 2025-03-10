using UnityEngine;
using TMPro; // For TextMeshPro Input Fields
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using UnityEngine.UI;

public class PaymentScript : MonoBehaviour
{
    public TMP_InputField AmountInputField;
    public TextMeshProUGUI statusMessageText;
    public GameObject WalletPanel;
    public GameObject CurrentPanel;
    public GameObject ProceedBtn;
    private string orderId; 

    public string apiUrl = "https://server.fivlog.space/api/transactions/create"; // Replace with your actual API URL
    private UniWebView webView; // For Razorpay Payment

    private bool isWebViewActive = false;

    public GameObject Loading;

    
    public void EnablePanel(GameObject panel)
        {
            panel.SetActive(true);
            CurrentPanel.SetActive(false);
        }

    public void OnProceedButtonClicked()
    {
        // Capture the input values
        string amount = AmountInputField.text;

        if (string.IsNullOrEmpty(amount))
        {
            ShowStatusMessage("Please enter a valid amount.");
            return;
        }

        //statusMessageText.text = "Processing payment, please wait...";
        ProceedBtn.transform.GetChild(0).gameObject.SetActive(false);
        ProceedBtn.transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(SendTransactionRequest(int.Parse(amount)));
    }

    public IEnumerator SendTransactionRequest(int amount)
    {
        string authToken = PlayerPrefs.GetString("AuthToken", null);
        if (string.IsNullOrEmpty(authToken))
        {
            ShowStatusMessage("Authorization token is missing.");
            yield break;
        }

        // Prepare JSON data
        string jsonData = JsonUtility.ToJson(new UserTransaction { amount = amount });

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", authToken);

        Debug.Log("Transaction Request Data: " + jsonData);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Transaction Response: " + request.downloadHandler.text);
            ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);

            // Store the orderId globally
            orderId = apiResponse.orderId;

            Debug.Log("Fetched Order ID: " + orderId);
            ShowStatusMessage("Launching Razorpay Payment Gateway...");
            StartRazorpayPayment(orderId, amount);
        }

        else
        {
            Debug.LogError("Transaction request failed: " + request.error);
            ShowStatusMessage("Failed to create transaction. Retrying...");
            yield return new WaitForSeconds(2); // Retry after 2 seconds
            StartCoroutine(SendTransactionRequest(amount));
        }
    }

    public void StartRazorpayPayment(string orderId, int amount)
    {
        // Initialize UniWebView
        webView = gameObject.AddComponent<UniWebView>();

        // Show the embedded toolbar at the top
        webView.EmbeddedToolbar.SetPosition(UniWebViewToolbarPosition.Top);
        webView.EmbeddedToolbar.Show();

        // Get the actual toolbar height based on device DPI
        int toolbarHeight = Mathf.RoundToInt(Screen.height * 0.1f); // Adjust this if needed

        // Set WebView frame to cover everything below the toolbar
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);

        webView.BackgroundColor = Color.white;
        webView.SetShowToolbar(true, true);
        webView.SetToolbarDoneButtonText("Done");

        // Handle Done button click
        webView.OnShouldClose += (view) =>
        {
            CloseWebView();
            return true;
        };

        // Construct Razorpay URL
        string razorpayUrl;
        if (Application.platform == RuntimePlatform.Android)
        {
            razorpayUrl = $"file:///android_asset/index.html?orderId={orderId}&amount={amount}";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            razorpayUrl = $"file://{Application.streamingAssetsPath}/index.html?orderId={orderId}&amount={amount}";
        }
        else
        {
            razorpayUrl = $"file://{Application.streamingAssetsPath}/index.html?orderId={orderId}&amount={amount}";
        }

        Debug.Log("Loading Razorpay URL: " + razorpayUrl);

        // Load and display the WebView
        webView.Load(razorpayUrl);
        webView.Show();

        ProceedBtn.transform.GetChild(0).gameObject.SetActive(true);
        ProceedBtn.transform.GetChild(1).gameObject.SetActive(false);
        ShowStatusMessage(" ");
        EnablePanel(WalletPanel);
    }

    // Function to handle WebView closing
    private void CloseWebView()
    {
        if (webView != null)
        {
            webView.Hide();
            Destroy(webView);
            isWebViewActive = false;
            //ShowStatusMessage("Payment was canceled.");

            // Emit the orderId to the socket
            if (!string.IsNullOrEmpty(orderId))
            {
                StartCoroutine(SendTransactionUpdate(orderId));
            }
            else
            {
                Debug.LogWarning("Order ID is missing. Cannot send transaction update.");
            }
        }
    }

    private IEnumerator SendTransactionUpdate(string orderId)
    {
        string updateUrl = "https://your-backend.com/api/transactions/update?orderId=" + orderId; // Replace with your actual API URL

        UnityWebRequest request = UnityWebRequest.Get(updateUrl);

        // Add Authorization if needed
        string authToken = PlayerPrefs.GetString("AuthToken", null);
        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", authToken);
        }

        Debug.Log("Sending Transaction Update: " + updateUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Transaction update sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to send transaction update: " + request.error);
        }
    }






    private void OnWebViewMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log($"Message from WebView: {message.RawMessage}");

        RazorpayResponse response = JsonUtility.FromJson<RazorpayResponse>(message.RawMessage);

        if (response.status == "success")
        {
            Debug.Log($"Payment Successful! Payment ID: {response.paymentId}, Order ID: {response.orderId}");
            ShowStatusMessage("Payment successful! Thank you.");
            CurrentPanel.SetActive(false);
        }
        else if (response.status == "failed")
        {
            Debug.LogError("Payment failed or dismissed.");
            ShowStatusMessage("Payment failed. Please try again.");
        }

        // Close and destroy the web view
        webView.Hide();
        Destroy(webView);
        isWebViewActive = false;
    }

    private void ShowStatusMessage(string message)
    {
        statusMessageText.text = message;
        statusMessageText.gameObject.SetActive(true);
    }

    private void Update()
    {
        // Handle back button to close WebView
        if (Input.GetKeyDown(KeyCode.Escape) && isWebViewActive && webView != null)
        {
            webView.Hide();
            Destroy(webView);
            ShowStatusMessage("Payment was canceled.");
            isWebViewActive = false;
        }
    }

    [System.Serializable]
    public class UserTransaction
    {
        public int amount;
    }

    [System.Serializable]
    public class ApiResponse
    {
        public string message;
        public string orderId;
    }

    [System.Serializable]
    public class RazorpayResponse
    {
        public string status;
        public string paymentId;
        public string orderId;
        public string signature;
    }


}
