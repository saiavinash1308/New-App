

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POP : MonoBehaviour
{
    public PPt[] CommonPathPoint;
    public PPt[] BluePlayerPathPoint;
    public PPt[] RedPlayerPathPoint;
    public PPt[] GreenPlayerPathPoint;
    public PPt[] YellowPlayerPathPoint;
    public PPt[] BasePoint;

    [Header("Scale and Position Difference")]
    public float[] scales;
    public float[] positionDifference;
    public AudioSource killSound;
    public PPt GetStartPathPoint(PP playerPiece_)
    {
        if (playerPiece_.name.Contains("B"))
        {
            return BluePlayerPathPoint[0];
        }
        else if (playerPiece_.name.Contains("R"))
        {
            return RedPlayerPathPoint[0];
        }
        else if (playerPiece_.name.Contains("G"))
        {
            return GreenPlayerPathPoint[0];
        }
        else if (playerPiece_.name.Contains("Y"))
        {
            return YellowPlayerPathPoint[0];
        }
        return null;
    }
}