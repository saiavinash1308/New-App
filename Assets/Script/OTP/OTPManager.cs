using UnityEngine;
using TMPro; // For TextMeshPro UI components
using UnityEngine.Networking; // For sending the API request
using UnityEngine.SceneManagement; // For scene navigation
using System.Collections;
using System.Text; // For encoding JSON data
using UnityEngine.UI;
using System.Collections.Generic; // For List

public class OTPManager : MonoBehaviour
{
    public List<TMP_InputField> inputFields; // Array of input fields
    public Button submitOTPButton;
    public Button resendOTPButton;
    public TextMeshProUGUI statusText;
    public GameObject Loading;
    public GameObject CurrentPanel;
    public GameObject otpBtn;

    public string verifyOtpApiUrl = "https://server.fivlog.space/api/user/verifyotp"; // OTP verification endpoint
    public string resendOtpApiUrl = "https://server.fivlog.space/api/user/resendotp"; // Resend OTP endpoint

    void Start()
    {
        submitOTPButton.onClick.AddListener(OnSubmitOTPClicked);
        resendOTPButton.onClick.AddListener(OnResendOTPClicked);
    }

    public void EnablePanel(GameObject panel)
    {
        panel.SetActive(true);
        CurrentPanel.SetActive(false);
    }

    // OTP submission function
    public void OnSubmitOTPClicked()
    {
        string mobile = PlayerPrefs.GetString("mobile", "");
        if (string.IsNullOrEmpty(mobile))
        {
            statusText.text = "Error: Mobile not found!";
            return;
        }

        // Concatenate all input field values to form the OTP
        string otp = "";
        foreach (var inputField in inputFields)
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                otp += inputField.text;
            }
            else
            {
                statusText.text = "Error: One or more OTP fields are empty!";
                return;
            }
        }

        Loading.SetActive(true);
        otpBtn.transform.GetChild(0).gameObject.SetActive(false);
        otpBtn.transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(SubmitOTP(otp, mobile));
    }

    public void OnResendOTPClicked()
    {
        string mobile = PlayerPrefs.GetString("mobile", "");
        if (string.IsNullOrEmpty(mobile))
        {
            statusText.text = "Error: Mobile not found!";
            return;
        }

        StartCoroutine(ResendOTP(mobile));
    }

    IEnumerator SubmitOTP(string otp, string mobile)
    {
        string jsonData = JsonUtility.ToJson(new OTPRequest { mobile = mobile, otp = otp });
        Debug.Log("JSON Data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(verifyOtpApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            Loading.SetActive(false);
            statusText.text = "Incorrect OTP";
            otpBtn.transform.GetChild(0).gameObject.SetActive(true);
            otpBtn.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log("Response from API: " + response);

            OTPResponse otpResponse = JsonUtility.FromJson<OTPResponse>(response);

            if (!string.IsNullOrEmpty(otpResponse.token))
            {
                PlayerPrefs.SetString("AuthToken", otpResponse.token);
                PlayerPrefs.Save();
                statusText.text = "OTP verified successfully!";
                Loading.SetActive(false);
                otpBtn.transform.GetChild(0).gameObject.SetActive(true);
                otpBtn.transform.GetChild(1).gameObject.SetActive(false);
                SceneManager.LoadScene("Home");
            }
            else
            {
                statusText.text = "OTP verification failed: " + otpResponse.message;
            }
        }
    }

    IEnumerator ResendOTP(string mobile)
    {
        string jsonData = JsonUtility.ToJson(new ResendOTPRequest { mobile = mobile });
        Debug.Log("Sending JSON data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(resendOtpApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            statusText.text = "Error: " + request.error;
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log("Response from API: " + response);

            ResendOTPResponse resendResponse = JsonUtility.FromJson<ResendOTPResponse>(response);

            if (resendResponse.success)
            {
                statusText.text = "OTP resent successfully!";
            }
            else
            {
                statusText.text = "Failed to resend OTP: " + resendResponse.message;
            }
        }
    }

    [System.Serializable]
    public class OTPRequest
    {
        public string mobile;
        public string otp; // Single concatenated OTP string
    }

    [System.Serializable]
    public class ResendOTPRequest
    {
        public string mobile;
    }

    [System.Serializable]
    public class OTPResponse
    {
        public string token;
        public string message;
    }

    [System.Serializable]
    public class ResendOTPResponse
    {
        public bool success;
        public string message;
    }
}
