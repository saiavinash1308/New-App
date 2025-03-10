using UnityEngine;
using UnityEngine.UI;

public class ClassicLudoWS : MonoBehaviour
{
    public Text winnerText;

    void Start()
    {
        string winner = PlayerPrefs.GetString("Winner");
        winnerText.text = winner + " Player Wins!";
    }
}
