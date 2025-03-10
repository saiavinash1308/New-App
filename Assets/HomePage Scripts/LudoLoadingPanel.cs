using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LudoLoadingPanel : MonoBehaviour
{
    public float splashDuration = 2f;
    public Slider loadingSlider;
    public GameObject SelectScene;
    public GameObject CurrentPanel;

    private void OnEnable() // Use OnEnable instead of Start, so it resets every time the panel is shown
    {
        ResetLoadingPanel();
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private void ResetLoadingPanel()
    {
        loadingSlider.value = 0f; // Reset slider value
        SelectScene.SetActive(false); // Ensure next scene is hidden
        CurrentPanel.SetActive(true); // Ensure loading panel is active
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        float elapsedTime = 0f;

        while (elapsedTime < splashDuration)
        {
            loadingSlider.value = elapsedTime / splashDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        loadingSlider.value = 1f;
        SelectScene.SetActive(true);
        CurrentPanel.SetActive(false);
    }
}
