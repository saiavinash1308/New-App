using UnityEngine;

public class ST : MonoBehaviour
{
    public float turnTime = 30f; // The total time for the timer in seconds
    private float currentTime;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        StartTurnTimer();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateSpriteTransparency(currentTime);

            if (currentTime <= 0)
            {
                EndTurn();
            }
        }
    }

    public void StartTurnTimer()
    {
        currentTime = turnTime;
        UpdateSpriteTransparency(currentTime);
    }

    void UpdateSpriteTransparency(float time)
    {
        float alphaValue = time / turnTime;
        Color newColor = originalColor;
        newColor.a = alphaValue; // Update the transparency (alpha channel)
        spriteRenderer.color = newColor;
    }

    void EndTurn()
    {
        // Code to handle the end of the turn
        Debug.Log("Turn ended due to timeout.");
        // Example: Trigger the next player's turn or any other logic here.
    }
}
