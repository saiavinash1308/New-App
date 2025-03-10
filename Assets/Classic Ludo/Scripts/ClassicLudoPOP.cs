

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicLudoPOP : MonoBehaviour
{
    public ClassicLudoPPt[] CommonPathPoint;
    public ClassicLudoPPt[] BluePlayerPathPoint;
    public ClassicLudoPPt[] RedPlayerPathPoint;
    public ClassicLudoPPt[] GreenPlayerPathPoint;
    public ClassicLudoPPt[] YellowPlayerPathPoint;
    public ClassicLudoPPt[] BasePoint;

    [Header("Scale and Position Difference")]
    public float[] scales;
    public float[] positionDifference;
    public AudioSource killSound;
    public ClassicLudoPPt GetStartPathPoint(ClassicLudoPP playerPiece_)
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