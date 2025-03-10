using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes

public class SplashScreenController : MonoBehaviour
{
    public float splashDuration = 3f;  // Adjust this duration as needed

    void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Coroutine to handle the delay
    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);

        string userName = PlayerPrefs.GetString("AuthToken", null);  // Retrieve the username (or token) from PlayerPrefs

        if (!string.IsNullOrEmpty(userName))
        {
            Debug.Log("User is logged in: " + userName);
            SceneManager.LoadScene("Home");  // Replace with your actual home page scene name
        }
        else
        {
            Debug.Log("User is not logged in.");
            SceneManager.LoadScene("SignUp");  // Replace with your actual sign-up scene name
        }
    }
}
