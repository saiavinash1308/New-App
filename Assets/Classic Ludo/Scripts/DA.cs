using UnityEngine;

public class DA : MonoBehaviour
{
    private AudioSource ads;
    private const string TOGGLE_PREF_KEY = "ButtonTogglerState";  // Same key used in ButtonSpriteToggler

    void Start()
    {
        ads = GetComponent<AudioSource>();

        // Load the saved toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;  // Default to 'on' if no value is saved

        // Set initial audio volume based on the toggle state
        ads.volume = isOn ? 1.0f : 0.0f;
    }

    public void PlaySound()
    {
        // Ensure the volume is set according to the toggle state each time the sound is played
        bool isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;
        ads.volume = isOn ? 1.0f : 0.0f;

        // Play the sound only if the volume is not zero
        if (ads.volume > 0)
        {
            ads.Play();
        }
    }
}
