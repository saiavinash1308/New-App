using UnityEngine;
using TMPro; // For TextMeshPro Input Fields
using UnityEngine.Networking; // For API requests
using UnityEngine.SceneManagement; // For scene navigation
using System.Text;
using System.Collections; // For encoding the JSON data
using UnityEngine.UI;

public class SignUpManager : MonoBehaviour
{
    public GameObject ContinueBtn;
    // References to the input fields
    public TMP_InputField nameInputField;
   // public TMP_InputField emailInputField;
    public TMP_InputField mobileInputField;
    // public TMP_InputField passwordInputField;
    // public TMP_InputField confirmPasswordInputField;
    public GameObject Loading;
    // Reference to the status message text
    public TextMeshProUGUI statusMessageText;
    public GameObject SignInPanel;
    public GameObject SignUpPanel;
    public GameObject OTPPanel;
    public GameObject SignInPopUp;
    public Toggle TermsToggle;
    public Toggle StateToggle;
    public GameObject termschckmark, statechckmark;
    [SerializeField]
    private bool isterms, isstate;

    // URL for the backend API
    public string apiUrl = "https://server.fivlog.space/api/user/create"; // Replace with your actual API URL


    public void Start()
    {
        TermsToggle.onValueChanged.AddListener(OnTermsChanged);
        StateToggle.onValueChanged.AddListener(OnStateChanged);
        termschckmark.SetActive(false);
        statechckmark.SetActive(false);
    }

    public void OnTermsChanged(bool isOn)
    {
        isterms = isOn;  // Directly use the value from the toggle

        // Show or hide the checkmark based on toggle state
        if (isterms)
        {
            termschckmark.SetActive(true);
        }
        else
        {
            termschckmark.SetActive(false);
        }
    }

    public void OnStateChanged(bool isOn)
    {
        isstate = isOn;  // Directly use the value from the toggle

        // Show or hide the checkmark based on toggle state
        if (isstate)
        {
            statechckmark.SetActive(true);
        }
        else
        {
            statechckmark.SetActive(false);
        }
    }
    // Called when the Sign Up button is clicked
    public void OnSignUpButtonClicked()
    {
        // Capture the input values
        string name = nameInputField.text;
        //string email = emailInputField.text;
        string mobile = mobileInputField.text;
       // string password = passwordInputField.text;
       // string confirmPassword = confirmPasswordInputField.text;

        // Check for empty fields (basic validation)
        if (string.IsNullOrEmpty(name)  || string.IsNullOrEmpty(mobile))
        {
            ShowStatusMessage("All fields must be filled out.");
            return;
        }

        // Check for password match
        if (name == mobile)
        {
            ShowStatusMessage("name and mobile can't be same");
            return;
        }

        //statusMessageText.text = "Loading Please Wait.........";
        ContinueBtn.transform.GetChild(0).gameObject.SetActive(false);
        ContinueBtn.transform.GetChild(1).gameObject.SetActive(true);
        Loading.gameObject.SetActive(true);

        // Start the coroutine to send the sign-up request
        StartCoroutine(SendSignUpRequest(name,  mobile));
        //SignInPanel.SetActive(false);
        //SignUpPanel.SetActive(false);
    }

    // Coroutine to send the sign-up request
    public IEnumerator SendSignUpRequest(string name, string mobile)
    {
        // Create a JSON object for the request body
        string jsonData = JsonUtility.ToJson(new UserRegistration { name = name, mobile = mobile });

        // Create a UnityWebRequest with the JSON payload
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for network or server errors
        //if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //{
        //    ShowStatusMessage("Error: " + request.error);
        //    yield break;
        //}
        // SceneManager.LoadScene("OTP");
        // Log the entire API response for debugging
        string response = request.downloadHandler.text;
        Debug.Log("Response from API: " + response);

        // Manually check if the response contains "OTP generated"
        if ((response.Contains("User already exists")))
        {
            statusMessageText.text = response;
            ShowErrorMessage("User already exists");
            SignUpPanel.SetActive(false);
            Loading.SetActive(false);
            SignInPopUp.SetActive(true);
            //SignInPanel.SetActive(true);
        }
        if (response.Contains("OTP generated. Please verify."))
        {
            // If OTP is generated, move to the OTP scene
            Debug.Log("Sign-up successful, OTP generated. Navigating to OTP scene.");
           // PlayerPrefs.SetString("userEmail", email); // Store email for OTP
            PlayerPrefs.SetString("usermobile", mobile); // Store mobile for OTP
            PlayerPrefs.SetString("userName", name); // Store name for OTP
            PlayerPrefs.Save();
            //SceneManager.LoadScene("OTP"); // Ensure this matches the actual scene name
            Loading.SetActive(false);
            SignUpPanel.SetActive(false);
            OTPPanel.SetActive(true);
        }
        else
        {
            // Show error message if sign-up failed
            Debug.Log("Sign-up failed with response: " + response);
            ShowStatusMessage("Sign-up failed: " + response);
            Loading.gameObject.SetActive(false);
            ContinueBtn.transform.GetChild(0).gameObject.SetActive(true);
            ContinueBtn.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Show a status message
    void ShowStatusMessage(string message)
    {
        statusMessageText.text = message;
        statusMessageText.gameObject.SetActive(true); // Show the status message
    }

    // Class to represent the user's registration data
    [System.Serializable]
    public class UserRegistration
    {
        public string name;
       // public string email;
        public string mobile;
       // public string password;
    }

    public void OnLoginClicked()    
    {
        //SceneManager.LoadScene("Sign In");
        SignInPanel.SetActive(true);
        SignUpPanel.SetActive(false);
        OTPPanel.SetActive(false);
    }

    public void OnSignUpClicked()
    {
        if (isterms && isstate)
        {
            //SceneManager.LoadScene("Sign In");
            SignInPanel.SetActive(true);
            SignUpPanel.SetActive(false);
            OTPPanel.SetActive(false);
            Loading.SetActive(false);
            SignInPopUp.SetActive(false);
        }

    }

    void ShowErrorMessage(string message)
    {
        statusMessageText.text = message;
        //errorMessageText.gameObject.SetActive(true);
    }

}
