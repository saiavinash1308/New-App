using UnityEngine;
using UnityEngine.UI;  // For Toggle references

public class FooterScript : MonoBehaviour
{
    // public GameObject Home;
    // public GameObject Wallet;
    // public GameObject LeaderBoard;
    // public GameObject Profile;

    // public Toggle homeToggle;
    // public Toggle walletToggle;
    // public Toggle leaderBoardToggle;
    //// public Toggle profileToggle;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     // Initially activate the Home panel and deactivate others
    //     Home.SetActive(true);
    //     walletToggle.isOn = false;  // Disable all toggles initially (Home is on)
    //     leaderBoardToggle.isOn = false;
    //    // profileToggle.isOn = false;

    //     // Add listeners for toggle changes
    //     homeToggle.onValueChanged.AddListener(OnHomeToggleChanged);
    //     walletToggle.onValueChanged.AddListener(OnWalletToggleChanged);
    //     leaderBoardToggle.onValueChanged.AddListener(OnLeaderBoardToggleChanged);
    //    // profileToggle.onValueChanged.AddListener(OnProfileToggleChanged);
    // }

    // private void SetAllPanelsInactive()
    // {
    //     Home.SetActive(false);
    //     LeaderBoard.SetActive(false);
    //     Profile.SetActive(false);
    //     Wallet.SetActive(false);
    // }

    // // Method to activate the selected panel
    // public void ActivatePanel(GameObject panelToActivate)
    // {
    //     SetAllPanelsInactive();
    //     panelToActivate.SetActive(true);
    // }

    // // Methods that are called when toggles are changed
    // private void OnHomeToggleChanged(bool isOn)
    // {
    //     if (isOn)
    //     {
    //         ActivatePanel(Home);
    //         SetOtherTogglesInactive(homeToggle);
    //     }
    // }

    // private void OnWalletToggleChanged(bool isOn)
    // {
    //     if (isOn)
    //     {
    //         ActivatePanel(Wallet);
    //         SetOtherTogglesInactive(walletToggle);
    //     }
    // }

    // private void OnLeaderBoardToggleChanged(bool isOn)
    // {
    //     if (isOn)
    //     {
    //         ActivatePanel(LeaderBoard);
    //         SetOtherTogglesInactive(leaderBoardToggle);
    //     }
    // }

    // private void OnProfileToggleChanged(bool isOn)
    // {
    //     if (isOn)
    //     {
    //         ActivatePanel(Profile);
    //        // SetOtherTogglesInactive(profileToggle);
    //     }
    // }

    // // Helper method to deactivate all other toggles except the selected one
    // private void SetOtherTogglesInactive(Toggle activeToggle)
    // {
    //     // Disable other toggles
    //     if (activeToggle != homeToggle) homeToggle.isOn = false;
    //     if (activeToggle != walletToggle) walletToggle.isOn = false;
    //     if (activeToggle != leaderBoardToggle) leaderBoardToggle.isOn = false;
    //    // if (activeToggle != profileToggle) profileToggle.isOn = false;
    // }
    public GameObject Home;
    public GameObject Wallet;
    public GameObject LeaderBoard;
    public GameObject Profile;
    public GameObject SelectionPanel;
    public GameObject SelectionLoadingPanel;
    public GameObject EditProfilePanel;
    public GameObject SettingsPanel;
    public GameObject ReferPanel;
    public GameObject PaymentPanel;
    public GameObject walletPanel;

    public GameObject HomeMark;
    public GameObject WalletMark;
    public GameObject ProfileMark;

    // Start is called before the first frame update
    void Start()
    {
        // Initially activate the Home panel and deactivate the others
        // ActivatePanel(Home);
        Home.SetActive(true);
        HomeMark.SetActive(true);
    }

    private void SetAllPanelsInactive()
    {
        Home.SetActive(false);
        LeaderBoard.SetActive(false);
        Profile.SetActive(false);
        Wallet.SetActive(false);
        SelectionPanel.SetActive(false);
        SelectionLoadingPanel.SetActive(false);
        EditProfilePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        ReferPanel.SetActive(false);
        PaymentPanel.SetActive(false);
        walletPanel.SetActive(false);
    }

    public void SetAllCheckMarksInactive()
    {
        HomeMark.SetActive(false);
        ProfileMark.SetActive(false);
        WalletMark.SetActive(false);
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
