using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public GameObject SignInBtn;
    public TMP_InputField emailInputField;
    public TMP_InputField mobileInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI errorMessageText;
    public TextMeshProUGUI statusText;
    public GameObject SignupPanel;
    public GameObject SignInPanel;
    public GameObject OTPPanel;
    public GameObject Loading;
    public GameObject SignupPopUp;
    public string apiUrl = "https://server.fivlog.space/api/user/signin";

    private void Start()
    {
        emailInputField.text = "";
        mobileInputField.text = "";
        SignInBtn.transform.GetChild(0).gameObject.SetActive(true);
        SignInBtn.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnSignInButtonClicked()
    {
        string mobile = mobileInputField.text;

        if (string.IsNullOrEmpty(mobile))
        {
            ShowErrorMessage("Mobile cannot be empty");
            return;
        }

        //statusText.text = "Loading Please Wait............";
        Loading.gameObject.SetActive(true);
        SignInBtn.transform.GetChild(0).gameObject.SetActive(false);
        SignInBtn.transform.GetChild(1).gameObject.SetActive(true);

        // Save mobile number to PlayerPrefs
        PlayerPrefs.SetString("mobile", mobile);
        PlayerPrefs.Save(); // Save changes immediately (optional, as it usually saves automatically)

        StartCoroutine(SendLoginRequest(mobile));
    }


    IEnumerator SendLoginRequest(string mobile)
    {
        string jsonData = JsonUtility.ToJson(new UserCredentials { mobile = mobile });
        Debug.Log("Sending JSON data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Request URL: " + request.url);
        Debug.Log("Request Method: " + request.method);
        Debug.Log("Response Code: " + request.responseCode);

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Connection Error: " + request.error);
            ShowErrorMessage("Connection Error Try Again");
            Loading.gameObject.SetActive(false);
            yield break;
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Protocol Error: " + request.error);
            Debug.LogError("Response Body: " + request.downloadHandler.text);
            //SignupPanel.SetActive(true);
            //SignInPanel.SetActive(false);
            SignupPopUp.SetActive(true);
            //ShowErrorMessage("Mobile number not registered, Kindly SignUp");
            Loading.gameObject.SetActive(false);
            SignInBtn.transform.GetChild(0).gameObject.SetActive(true);
            SignInBtn.transform.GetChild(1).gameObject.SetActive(false);
            yield break;
        }
        else if (request.responseCode == 400)
        {
            string errorResponseText = request.downloadHandler.text;
            Debug.LogError("Error Response: " + errorResponseText);
            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(errorResponseText);
            ShowErrorMessage("Error: " + errorResponse.message);
            yield break;
        }

        string responseText = request.downloadHandler.text;
        Debug.Log("Response: " + responseText);
        //SceneManager.LoadScene("OTP");
        Loading.SetActive(false);
        SignupPopUp.SetActive(false);
        SignInPanel.SetActive(false);
        SignupPanel.SetActive(false);
        SignInBtn.transform.GetChild(0).gameObject.SetActive(true);
        SignInBtn.transform.GetChild(1).gameObject.SetActive(false);
        OTPPanel.SetActive(true);
        LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(responseText);

        if (!string.IsNullOrEmpty(loginResponse.AuthToken) && loginResponse.user != null)
        {
            Debug.Log("Mobile number from response: " + mobile);
            PlayerPrefs.SetString("mobile", mobile);
            PlayerPrefs.Save();
            OTPPanel.SetActive(true);
        }
        else
        {
            //ShowErrorMessage("Incorrect Email, mobile or Password");
        }


        //SceneManager.LoadScene("OTP");
    }

    void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
    }

    public void OnSignUpClicked()
    {
        SignupPanel.gameObject.SetActive(true);
        SignInPanel.gameObject.SetActive(false);
        Loading.SetActive(false);
        SignupPopUp.SetActive(false);
    }
    public void OnSignUpHere()
    {
        // SceneManager.LoadScene("Sign Up");
        SignupPanel.gameObject.SetActive(false);
        SignInPanel.gameObject.SetActive(true);
        OTPPanel.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class UserCredentials
    {
        // public string email;
        public string mobile;
        // public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string AuthToken;
        public User user; // Change this to the nested User class
    }

    [System.Serializable]
    public class User
    {
        public string id; // The actual user ID field
                          // public string email;
        public string mobile;
        internal string GameID;
        // public string name;
    }

    [System.Serializable]
    public class ErrorResponse
    {
        public string message;
    }


    //IEnumerator LoadNextSceneAfterDelay()
    //{
    //    // Wait for the specified duration
    //    yield return new WaitForSeconds(1f);

    //    // Check if the user AuthToken exists
    //    string AuthToken = PlayerPrefs.GetString("AuthToken", null);

    //    // Load the appropriate scene based on the AuthToken
    //    if (!string.IsNullOrEmpty(AuthToken))
    //    {
    //        // User is already logged in, load the main menu or another scene
    //        SceneManager.LoadScene("Sign In"); // Replace with your main menu scene name
    //        Debug.Log("AuthToken not present");
    //    }
    //    else
    //    {
    //        // User is not logged in, load the sign-in scene
    //        SceneManager.LoadScene("HomePage"); // Ensure this matches the name of your sign-in scene
    //        Debug.Log("AuthToken present");
    //    }
    //}

}
