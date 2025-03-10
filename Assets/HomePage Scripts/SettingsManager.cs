using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject Wallet;
    public GameObject Refer;
    public GameObject Profile;
    public GameObject Settings;

    public Button LogOut;

    private void SetAllPanelsInactive()
    {
        Refer.SetActive(false);
        Profile.SetActive(false);
        Wallet.SetActive(false);
        Settings.SetActive(false);
    }

    public void ActivatePanel(GameObject panelToActivate)
    {
        SetAllPanelsInactive();
        panelToActivate.SetActive(true);
    }

    public void OnLogOut() 
    {
        PlayerPrefs.DeleteKey("AuthToken");
        SceneManager.LoadScene("SignUp");
    }

}
