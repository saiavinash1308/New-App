using UnityEngine;
using UnityEngine.UI;

public class WS : MonoBehaviour
{
    public Text winnerText;

    void Start()
    {
        string winner = PlayerPrefs.GetString("Winner");
        winnerText.text = winner + " Player Wins!";
    }
}
