using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public Button BackBtn;
    public GameObject PaneltoClose;
    public GameObject PaneltoOpen;
    // Start is called before the first frame update

    private void Start()
    {
        BackBtn.onClick.AddListener(delegate { OnBackButton(); });
    }

    public void OnBackButton()
    {
        PaneltoClose.SetActive(false);
        PaneltoOpen.SetActive(true);
    }
}
