using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FetchWalletAmount : MonoBehaviour
{
    // Array of UI Text elements where the wallet amount will be displayed
    public Text[] walletAmountTexts; // Array to hold multiple Text components

    private const string apiUrl = "https://server.fivlog.space/api/wallet/getWalletAmount"; // Replace with actual URL
    private const string tokenKey = "AuthToken"; // The key where the token is stored in PlayerPrefs

    // Start is called before the first frame update
    void Start()
    {
        // Call the method to fetch wallet data when the script starts
        StartCoroutine(FetchWalletData());
    }

    // Coroutine to fetch wallet data from the server
    IEnumerator FetchWalletData()
    {
        // Retrieve the auth token from PlayerPrefs
        string authToken = PlayerPrefs.GetString("AuthToken", null);
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("Authorization token is missing.");
            yield break;
        }

        // Create the UnityWebRequest for the GET request
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        // Add the authorization token (Bearer token in headers)
        request.SetRequestHeader("authorization", authToken);

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for errors in the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Log the actual response
            string responseText = request.downloadHandler.text;
            Debug.Log("Response from API: " + responseText);

            // Parse the response
            WalletResponse walletResponse = JsonUtility.FromJson<WalletResponse>(responseText);
            if (walletResponse != null)
            {
                Debug.Log("Parsed amount: " + walletResponse.amount);  // Log the parsed amount

                // Loop through all Text elements in the array and update their text
                foreach (Text walletText in walletAmountTexts)
                {
                    walletText.text = "₹ " + walletResponse.amount.ToString("N0"); // Add commas for formatting
                }
            }
            else
            {
                Debug.LogError("Failed to parse wallet response.");
            }
        }
        else
        {
            // Handle errors (e.g., network issues, server errors)
            Debug.LogError("Error fetching wallet amount: " + request.error);
        }
    }
}

// Class to match the expected JSON response structure
[System.Serializable]
public class WalletResponse
{
    public float amount;
}
