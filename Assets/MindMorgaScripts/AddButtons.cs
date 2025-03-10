using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // Import DoTween namespace

public class AddButtons : MonoBehaviour
{
    [SerializeField]
    private Transform gameField;  // Parent of the buttons (the area in which buttons will be placed)

    [SerializeField]
    private GameObject btn;  // The button prefab

    [SerializeField]
    private float animationDuration = 0.5f;  // Duration for each button's animation

    private float buttonSize = 215f;  // Button width and height (since it's square)
    private float minSpacing = 20f;  // Minimum spacing between buttons

    private int buttonsPerRow;  // Number of buttons per row
    private int buttonsPerColumn;  // Number of buttons per column
    public MindMorgaGameController GameController;
    private int[] rowPattern = { 3, 4, 4, 4, 4, 3 }; // Custom button arrangement

    void Awake()
    {
        // Start spawning buttons after the setup
        StartCoroutine(SpawnButtons());
    }

    private IEnumerator SpawnButtons()
    {
        yield return new WaitForSeconds(0.1f);  // Small delay for setup

        RectTransform gameFieldRect = gameField.GetComponent<RectTransform>();
        float fieldWidth = gameFieldRect.rect.width;
        float fieldHeight = gameFieldRect.rect.height;

        int totalButtons = 22;
        int index = 0;
        float yStart = fieldHeight / 2 - buttonSize / 2;  // Start placing buttons from the top

        for (int row = 0; row < rowPattern.Length; row++)
        {
            int buttonsInRow = rowPattern[row];

            // Calculate the X start position to center the row
            float rowWidth = buttonsInRow * (buttonSize + minSpacing) - minSpacing;
            float xStart = -rowWidth / 2 + buttonSize / 2;

            for (int col = 0; col < buttonsInRow; col++)
            {
                if (index >= totalButtons) break;

                GameObject button = Instantiate(btn);
                button.name = " " + index;

                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.SetParent(gameField, false);
                buttonRect.anchoredPosition = Vector2.zero; // Start at the center

                Vector2 targetPosition = new Vector2(xStart + col * (buttonSize + minSpacing), yStart - row * (buttonSize + minSpacing));

                float randomRotation = Random.Range(0f, 30f);

                buttonRect.DOAnchorPos(targetPosition, animationDuration).SetEase(Ease.OutBack);
                buttonRect.DORotate(new Vector3(0, 0, randomRotation), animationDuration, RotateMode.FastBeyond360);

                index++;
                yield return new WaitForSeconds(0.05f);
            }
        }

        GameController.GetStarted();
    }

    private Vector2 GetGridPosition(int index, float fieldWidth, float fieldHeight)
    {
        // Calculate row and column from the index
        int row = index / buttonsPerRow;
        int column = index % buttonsPerRow;

        // Calculate the X and Y positions for this button based on its row and column
        float xPos = (column * (buttonSize + minSpacing)) - (fieldWidth / 2) + (buttonSize / 2);
        float yPos = (row * (buttonSize + minSpacing)) - (fieldHeight / 2) + (buttonSize / 2);

        // Return the calculated position
        return new Vector2(xPos, yPos);
    }
}
