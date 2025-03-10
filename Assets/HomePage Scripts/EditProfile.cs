using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditUserProfile : MonoBehaviour
{
    public string APIIUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms/api/auth/profile"; // Updated endpoint for the profile

    public TextMeshProUGUI nameInput; // Input field for name
    //public TextMeshProUGUI emailInput; // Input field for email
    public Button saveButton; // Button to save changes
    public Button BackBtn; // Button to save changes
    public GameObject CurrentPanel;
    public GameObject PreviousPanel;

    void Start()
    {
        saveButton.onClick.AddListener(UpdateProfile);
        BackBtn.onClick.AddListener(delegate { SetAllPanelsInactive(); });
    }

    public void UpdateProfile()
    {
        string userToken = PlayerPrefs.GetString("authToken", null);

        if (string.IsNullOrEmpty(userToken))
        {
            Debug.LogError("User token is missing or invalid.");
            return;
        }

        StartCoroutine(UpdateUserData(userToken));
    }

    IEnumerator UpdateUserData(string userToken)
    {
        // Log input values
        Debug.Log("Name Input: " + nameInput.text);
        //Debug.Log("Email Input: " + emailInput.text);

        // Create a JSON object with the updated data
        var userData = new
        {
            name = nameInput.text,
           // email = emailInput.text
        };
        string json = JsonUtility.ToJson(userData);
        Debug.Log("Serialized JSON: " + json);

        using (UnityWebRequest request = UnityWebRequest.Put(APIIUrl, json))
        {
            request.SetRequestHeader("Authorization", "Bearer " + userToken);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error updating user profile: " + request.error + " | Response Code: " + request.responseCode);
            }
            else
            {
                Debug.Log("User profile updated successfully: " + request.downloadHandler.text);
                SceneManager.LoadScene("Profile"); // Load profile scene after successful update
            }
        }
    }

    // Custom certificate handler to bypass SSL validation (for testing only)
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // Accept all certificates (do not use in production)
        }
    }

    private void SetAllPanelsInactive()
    {
        // Deactivates the current panel
        CurrentPanel.SetActive(false);
        PreviousPanel.SetActive(true);
    }
}
