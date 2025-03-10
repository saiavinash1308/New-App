using UnityEngine;
using TMPro; // Ensure you have TextMeshPro installed.

public class FrameRateCounter : MonoBehaviour
{
    public float updateInterval = 0.5f; // Time interval to update FPS
    private float lastInterval;
    private int frameCount;

    private TextMeshProUGUI fpsText; // Reference to the TextMeshProUGUI component

    // Colors for FPS feedback
    private string htmlColorTag;

    public float zPosition = 0f;  // Set the Z-axis depth

    void Awake()
    {
        // Initialize the TextMeshPro component
        GameObject fpsDisplay = new GameObject("FPS Display");
        fpsDisplay.transform.SetParent(this.transform);
        fpsText = fpsDisplay.AddComponent<TextMeshProUGUI>();

        fpsText.fontSize = 50;
        fpsText.alignment = TextAlignmentOptions.TopRight;
        fpsText.rectTransform.anchorMin = new Vector2(1, 1);
        fpsText.rectTransform.anchorMax = new Vector2(1, 1);
        fpsText.rectTransform.pivot = new Vector2(1, 1);
        fpsText.rectTransform.anchoredPosition = new Vector2(60, 825); // Adjust position as needed

        // Set the initial scale of the FPS counter (optional)
        fpsText.rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Scale the FPS counter by 0.5x

        // Setup the text properties
        fpsText.color = Color.white; // Default color
        lastInterval = Time.realtimeSinceStartup;

        // Set the Z-axis position of the FPS counter
        fpsText.rectTransform.localPosition = new Vector3(fpsText.rectTransform.localPosition.x, fpsText.rectTransform.localPosition.y, zPosition);
    }

    void Update()
    {
        frameCount++;

        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            float fps = frameCount / (timeNow - lastInterval);
            float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

            // Change the text color based on FPS value
            if (fps < 30)
                htmlColorTag = "<color=yellow>";
            else if (fps < 10)
                htmlColorTag = "<color=red>";
            else
                htmlColorTag = "<color=green>";

            // Display only the frame time in milliseconds
            fpsText.SetText($"{htmlColorTag}{ms:0} ms</color>");

            // Reset frame count and interval
            frameCount = 0;
            lastInterval = timeNow;
        }
    }
}
