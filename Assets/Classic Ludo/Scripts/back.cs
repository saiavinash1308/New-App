/*


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class back : MonoBehaviour
{



    public GameObject dialogBox; // Reference to the Panel (Dialog Box)

    void Start()
    {
        // Hide the dialog box at the start
        dialogBox.SetActive(false);
    }

    public void OpenDialogBox()
    {
        // Toggle dialog box visibility
        dialogBox.SetActive(!dialogBox.activeSelf);
    }
}
*/
using UnityEngine;

public class back : MonoBehaviour
{
    public GameObject classocdialogBox; // Reference to the Panel (Dialog Box)
    public GameObject classiccloseButton;
    public GameObject classicrulesPanel;
    public GameObject classicblurImage;

    void Start()
    {
        // Hide the dialog box at the start
        classocdialogBox.SetActive(false);
        classicrulesPanel.SetActive(false);
        classicblurImage.SetActive(false);
    }

    public void OpenDialogBox()
    {
        // Toggle dialog box visibility
        classocdialogBox.SetActive(true);
    }

    public void openRulesPanel()
    {
        classicrulesPanel.SetActive(true);
        classicblurImage.SetActive(true);

    }

    public void clostRulesPanel()
    {
        classicrulesPanel.SetActive(false);
        classicblurImage.SetActive(false);
        classocdialogBox.SetActive(false);
    }

    public void CloseButton()
    {
        classocdialogBox.SetActive(false);
    }
}