using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FetchUserProfile : MonoBehaviour
{
    // Arrays of UI Text elements where the username and mobile will be displayed
    public Text[] usernameTextArray; // Array for displaying username
    public Text[] mobileTextArray;   // Array for displaying mobile number
    public Text totalMatchesText;
    public Text matchesWonText;
    public Text matchesLostText;

    private const string apiUrl = "https://server.fivlog.space/api/user/profile"; // Replace with your API URL
    private const string tokenKey = "AuthToken"; // The key where the token is stored in PlayerPrefs

    // Start is called before the first frame update
    void Start()
    {
        // Call the method to fetch the user profile when the script starts
        StartCoroutine(FetchProfileData());
    }

    // Coroutine to fetch user profile data from the server
    IEnumerator FetchProfileData()
    {
        // Retrieve the auth token from PlayerPrefs
        string authToken = PlayerPrefs.GetString(tokenKey, null);
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
            ProfileResponse profileResponse = JsonUtility.FromJson<ProfileResponse>(responseText);

            if (profileResponse != null && profileResponse.user != null)
            {
                // Log parsed data for debugging
                Debug.Log("Parsed Username: " + profileResponse.user.username);
                Debug.Log("Parsed Mobile: " + profileResponse.user.mobile);
                Debug.Log("Parsed Total Matches: " + profileResponse.user.totalMatches);
                Debug.Log("Parsed Matches Won: " + profileResponse.user.matchesWon);

                // Calculate matches lost
                int matchesLost = profileResponse.user.totalMatches - profileResponse.user.matchesWon;

                // Display the username and mobile on all relevant UI Text component
                foreach (Text usernameText in usernameTextArray)
                {
                    usernameText.text = "" + profileResponse.user.username;
                }

                foreach (Text mobileText in mobileTextArray)
                {
                    mobileText.text = "" + profileResponse.user.mobile;
                }

                // Display total matches, matches won, and matches lost
                totalMatchesText.text = "" + profileResponse.user.totalMatches.ToString();
                matchesWonText.text = "" + profileResponse.user.matchesWon.ToString();
                matchesLostText.text = "" + matchesLost.ToString(); // Update the losses text
            }
            else
            {
                Debug.LogError("Failed to parse profile response.");
            }
        }
        else
        {
            // Handle errors (e.g., network issues, server errors)
            Debug.LogError("Error fetching profile data: " + request.error);
        }
    }
}

// Class to match the expected JSON response structure
[System.Serializable]
public class ProfileResponse
{
    public UserProfile user;
}

[System.Serializable]
public class UserProfile
{
    public string username;
    public string mobile;
    public int totalMatches;
    public int matchesWon;
}
