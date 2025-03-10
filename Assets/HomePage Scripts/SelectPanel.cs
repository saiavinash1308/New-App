using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    public GameObject All;
    public GameObject OneVOne;
    public GameObject FourWinners;
    public GameObject Others;

    public GameObject AllMark;
    public GameObject OneVOneMark;
    public GameObject FourWinnersMark;
    public GameObject OthersMark;
    public GameObject Wallet;
    public GameObject CurrentPanel;

    //public Text NameText;

    // Start is called before the first frame update
    void Start()
    {
        // Initially activate the Home panel and deactivate the others
        // ActivatePanel(Home);
        All.SetActive(true);
        AllMark.SetActive(true);
        //string userName = PlayerPrefs.GetString("userName","Guest");
        //string userName = PlayerPrefs.GetString("userName", "Guest");
        //NameText.text = userName;
    }
    public void EnablePanel(GameObject panel)
    {
        panel.SetActive(true);
        CurrentPanel.SetActive(false);
    }

    private void SetAllPanelsInactive()
    {
        All.SetActive(false);
        OneVOne.SetActive(false);
        FourWinners.SetActive(false);
        Others.SetActive(false);
        Wallet.SetActive(false);

    }

    private void SetAllCheckMarksInactive()
    {
        AllMark.SetActive(false);
        OneVOneMark.SetActive(false);
        FourWinnersMark.SetActive(false);
        OthersMark.SetActive(false);
    }


    public void ActivatePanel(GameObject panelToActivate)
    {
        SetAllPanelsInactive();
        panelToActivate.SetActive(true);
    }

    public void ActivateMark(GameObject MarkToActivate)
    {
        SetAllCheckMarksInactive();
        MarkToActivate.SetActive(true);
    }
}
