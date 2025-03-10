using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ReferManager : MonoBehaviour
{
    public Button BackBtn;
    public GameObject CurrentPanel;
    public GameObject PreviousPanel;

    private void Start()
    {
        // Corrected 'onClick' with lowercase 'o'
        BackBtn.onClick.AddListener(delegate { SetAllPanelsInactive(); });
    }

    private void SetAllPanelsInactive()
    {
        // Deactivates the current panel
        CurrentPanel.SetActive(false);
        PreviousPanel.SetActive(true);
    }
}

