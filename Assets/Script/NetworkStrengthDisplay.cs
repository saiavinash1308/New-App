using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkStrengthDisplay : MonoBehaviour
{
    public float updateInterval = 1f; // Time interval to update network strength
    private float lastUpdateTime;

    private Image networkStrengthImage; // Image for network strength
    public Sprite[] networkIcons; // Array of sprites for different network strengths (0 - weak, 4 - strong)

    void Awake()
    {
        // Initialize the network strength display
        GameObject networkIconObject = new GameObject("Network Strength Icon");
        networkIconObject.transform.SetParent(this.transform);

        networkStrengthImage = networkIconObject.AddComponent<Image>();
        networkStrengthImage.rectTransform.anchorMin = new Vector2(1, 1);
        networkStrengthImage.rectTransform.anchorMax = new Vector2(1, 1);
        networkStrengthImage.rectTransform.pivot = new Vector2(1, 1);
        networkStrengthImage.rectTransform.anchoredPosition = new Vector2(6,824); // Adjust position as needed
        networkStrengthImage.rectTransform.sizeDelta = new Vector2(32, 28); // Set icon size
        networkStrengthImage.rectTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        lastUpdateTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        UpdateNetworkStrength();
    }

    private void UpdateNetworkStrength()
    {
        float timeNow = Time.realtimeSinceStartup;

        if (timeNow > lastUpdateTime + updateInterval)
        {
            int networkStrength = GetNetworkStrength(); // Replace with actual network strength logic (0-4)
            if (networkIcons != null && networkIcons.Length > networkStrength)
            {
                networkStrengthImage.sprite = networkIcons[networkStrength];
            }
            lastUpdateTime = timeNow;
        }
    }

    // Dummy method to simulate network strength (replace with real implementation)
    private int GetNetworkStrength()
    {
        // Simulate varying network strength for demo purposes
        return Random.Range(0, 5); // Returns a random value between 0 (weak) and 4 (strong)
    }
}
