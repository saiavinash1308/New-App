using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupController : MonoBehaviour
{
    public RectTransform popupImage;  // Assign your popup's RectTransform
    public RectTransform viewport;    // Assign the Viewport RectTransform
    public Button openButton;
    public Button closeButton;

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private bool isVisible = false;

    public float stoppingYPosition = 500f; // Set the exact Y position where popup stops
    public float animationDuration = 0.5f; // Animation speed

    private void Start()
    {
        float popupHeight = popupImage.rect.height; // Get popup height

        // Set positions
        hiddenPosition = new Vector2(0, -popupHeight);  // Move below screen
        visiblePosition = new Vector2(0, stoppingYPosition); // Set exact stopping position

        // Start with popup hidden
        popupImage.anchoredPosition = hiddenPosition;
        popupImage.gameObject.SetActive(false);

        // Assign button click events
        openButton.onClick.AddListener(TogglePopup);
        if (closeButton != null)
            closeButton.onClick.AddListener(HidePopup);
    }

    public void TogglePopup()
    {
        if (isVisible)
            HidePopup();
        else
            ShowPopup();
    }

    public void ShowPopup()
    {
        popupImage.gameObject.SetActive(true);
        popupImage.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutBack);
        isVisible = true;
        Debug.Log("Popup moved to Y: " + stoppingYPosition);
    }

    public void HidePopup()
    {
        popupImage.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            popupImage.gameObject.SetActive(false);
            isVisible = false;
        });

        Debug.Log("Popup moved back to hidden position");
    }
}
