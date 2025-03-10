using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public List<Sprite> sprites; // List of sprites for the animation
    public float frameRate = 0.1f; // Time per frame in seconds
    public bool loop = true; // Should the animation loop?

    private Image imageComponent;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isAnimating = true; // Control animation without disabling the script

    void Start()
    {
        // Get the Image component
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogError("No Image component found on this GameObject!");
        }

        if (sprites == null || sprites.Count == 0)
        {
            Debug.LogError("No sprites assigned to the SpriteAnimator!");
        }
    }

    void OnEnable()
    {
        isAnimating = true; // Resume animation when GameObject is enabled
        ResetAnimation();
    }

    void Update()
    {
        if (!isAnimating || sprites == null || sprites.Count == 0 || imageComponent == null)
            return;

        // Increment the timer
        timer += Time.deltaTime;

        // If the timer exceeds the frame rate, move to the next frame
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame++;

            // Check if we should loop or stop the animation
            if (currentFrame >= sprites.Count)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    isAnimating = false; // Stop animation without disabling the script
                    return;
                }
            }

            // Update the image's sprite
            imageComponent.sprite = sprites[currentFrame];
        }
    }

    private void ResetAnimation()
    {
        currentFrame = 0;
        timer = 0f;
        isAnimating = true; // Ensure animation starts fresh

        if (imageComponent != null && sprites.Count > 0)
        {
            imageComponent.sprite = sprites[currentFrame];
        }
    }
}
