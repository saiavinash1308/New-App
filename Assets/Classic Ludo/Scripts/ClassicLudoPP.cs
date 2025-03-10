
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicLudoPP : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public ClassicLudoPOP pathparent;
    Coroutine movePlayerPiece;
    public ClassicLudoPPt previousPathPoint;
    public ClassicLudoPPt currentPathPoint;
    public bool IsSafeZone { get; set; }
    //public string defpieceid;

    public void Awake()
    {
        pathparent = FindObjectOfType<ClassicLudoPOP>();
    }

    public void movestep(ClassicLudoPPt[] pathpointstomoveon_)
    {
        //GM.game.piece = defpieceid;
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(ClassicLudoPPt[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        ClassicLudoGM.game.AddPathPoint(currentPathPoint);

        ClassicLudoGM.game.canDiceRoll = true;
        //GM.game.selfDice = true;
        //ClassicLudoGM.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(ClassicLudoPPt[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = ClassicLudoGM.game.numberofstepstoMove;

        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                ClassicLudoGM.game.ads.Play();
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;

            ClassicLudoGM.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);
            ClassicLudoGM.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (currentPathPoint.name == "CP")
            {
                MarkPieceAsFinished();
            }

            //GM.game.selfDice = false;
            //ClassicLudoGM.game.transferDice = true;

            ClassicLudoGM.game.numberofstepstoMove = 0;
        }

        ClassicLudoGM.game.canPlayermove = true;
        //GM.game.RolingDiceManager();

        if (movePlayerPiece != null)
        {
            StopCoroutine("movesteps_Enum");
        }
    }




    private void MarkPieceAsFinished()
    {
        string playerColor = "";
        if (name.Contains("Blue"))
        {
            playerColor = "Blue";

        }
        else if (name.Contains("Red"))
        {
            playerColor = "Red";

        }
        else if (name.Contains("Green"))
        {
            playerColor = "Green";

        }
        else if (name.Contains("Yellow"))
        {
            playerColor = "Yellow";

        }

        //ClassicLudoGM.game.IncrementCenterCount(playerColor);
        //ClassicLudoGM.game.CheckWinCondition();
        enabled = false;
    }

    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, ClassicLudoPPt[] pathpointtomove_)
    {
        if (numberofstepstoMove_ == 0)
        {
            return false;
        }
        int leftnumberofPath = pathpointtomove_.Length - numberofstepsalreadymove_;
        if (leftnumberofPath >= numberofstepstoMove_)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanMove(int steps)
    {
        ClassicLudoPPt[] pathpoints = GetPathPointsForColor();

        if (numberofstepsalreadymove + steps < pathpoints.Length)
        {
            return true;
        }
        return false;
    }

    public ClassicLudoPPt[] GetPathPointsForColor()
    {
        if (name.Contains("B"))
        {
            return pathparent.BluePlayerPathPoint;
        }
        else if (name.Contains("R"))
        {
            return pathparent.RedPlayerPathPoint;
        }
        else if (name.Contains("G"))
        {
            return pathparent.GreenPlayerPathPoint;
        }
        else if (name.Contains("Y"))
        {
            return pathparent.YellowPlayerPathPoint;
        }
        return null;
    }

    public bool IsCloseToCenterPath()
    {
        ClassicLudoPPt[] pathpoints = GetPathPointsForColor();
        return numberofstepsalreadymove + ClassicLudoGM.game.numberofstepstoMove >= pathpoints.Length;
    }
}

