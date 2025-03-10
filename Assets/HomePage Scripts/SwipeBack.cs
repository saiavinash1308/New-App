//using UnityEngine;

//public class SwipeBack : MonoBehaviour
//{
//    private Vector2 startTouchPosition, endTouchPosition;
//    public GameObject currentPanel;
//    public GameObject previousPanel;
//    public float swipeThreshold = 50f;

//    void Update()
//    {
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);

//            if (touch.phase == TouchPhase.Began)
//            {
//                startTouchPosition = touch.position;
//            }
//            else if (touch.phase == TouchPhase.Ended)
//            {
//                endTouchPosition = touch.position;
//                DetectSwipe();
//            }
//        }
//    }

//    void DetectSwipe()
//    {
//        float swipeDistance = endTouchPosition.x - startTouchPosition.x;

//        if (swipeDistance > swipeThreshold) // Right swipe detected
//        {
//            GoBack();
//        }
//    }

//    void GoBack()
//    {
//        if (previousPanel != null)
//        {
//            currentPanel.SetActive(false);
//            previousPanel.SetActive(true);
//        }
//    }
//}
using UnityEngine;

public class SwipeBack : MonoBehaviour
{
    public GameObject currentPanel;    // Active panel
    public GameObject previousPanel;   // Panel to go back to
    public GameObject popupPanel;      // Optional popup panel
    public bool showPopupOnBack = false; // Toggle to show popup instead of switching panel
    public float minSwipeDistance = 50f;

    private Vector2 startTouchPosition, endTouchPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }

        // Optional: Mouse swipe for testing
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        float swipeDistance = endTouchPosition.x - startTouchPosition.x;

        if (Mathf.Abs(swipeDistance) >= minSwipeDistance && swipeDistance > 0)
        {
            HandleBackAction();
        }
    }

    void HandleBackAction()
    {
        if (showPopupOnBack && popupPanel != null)
        {
            popupPanel.SetActive(true); // Show popup instead of switching panels
        }
        else if (previousPanel != null && currentPanel != null)
        {
            currentPanel.SetActive(false);
            previousPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Previous or Current Panel is not assigned.");
        }
    }
}


