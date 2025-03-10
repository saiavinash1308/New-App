using UnityEngine;
using TMPro; // For TextMeshPro Input Fields
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using UnityEngine.UI;

public class WithdrawPayment : MonoBehaviour
{
    public TMP_InputField amountInputField;  // Reference to the InputField in the scene
    public Button submitButton;          // Reference to the submit button
    public Text statusMessageText;
    public GameObject CurrentPanel;
    public GameObject HomePanel;
    public GameObject walletPanel;
    public GameObject Loading;

    private void Start()
    {
        //submitButton.onClick.AddListener(OnSubmitButtonClick);
    }
    public void OnBackBtn()
    {
        CurrentPanel.SetActive(false);
        walletPanel.SetActive(true);
    }

    public void OnSubmitButtonClick()
    {
        string authToken = PlayerPrefs.GetString("AuthToken");
        Debug.Log("AuthToken" + authToken);

        string amount = amountInputField.text;
        Debug.Log("amount: " + amount);

        if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(amount))
        {
            Debug.LogError("Auth token or amount is missing.");
            return;
        }
        Loading.SetActive(true);
        // Start the request
        StartCoroutine(SendRequest(authToken, amount));
    }

    private IEnumerator SendRequest(string authToken, string amount)
    {
        string url = "https://server.fivlog.space/api/transactions/createwithdraw";  // Replace with your actual endpoint

        // Prepare JSON data
        string jsonData = JsonUtility.ToJson(new UserTransaction { amount = amount });
        Debug.Log(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", authToken);

        Debug.Log("Transaction Request Data: " + jsonData);

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request failed: " + request.error);
            ShowStatusMessage("Request failed. Kindly enter an amount greater than or equal to 200.");
            Loading.SetActive(false);
        }
        else
        {
            var response = JsonUtility.FromJson<ResponseMessage>(request.downloadHandler.text);

            // Check for validation errors from the server
            if (response.message == "Invalid amount" || response.message == "Insufficient balance")
            {
                ShowStatusMessage(response.message); // Show the error message returned from the server
                Loading.SetActive(false);
            }
            else
            {
                // Log the response if successful
                Debug.Log("Request successful! Response: " + request.downloadHandler.text);
                ShowStatusMessage("Request successful! The amount will be credited in the next 24 hours...");

                // Delay before hiding the panel
                yield return new WaitForSeconds(2f);
                statusMessageText.text = ""; // Clear the text
                statusMessageText.gameObject.SetActive(false);
                // Hide the current panel and show the home panel
                CurrentPanel.SetActive(false);
                HomePanel.SetActive(true);
                Loading.SetActive(false);
            }
        }
    }

    private void ShowStatusMessage(string message)
    {
        statusMessageText.text = message;
        statusMessageText.gameObject.SetActive(true);
    }
    public class UserTransaction
    {
        public string amount;
    }
    [System.Serializable]
    public class ResponseMessage
    {
        public string message;
    }
}

