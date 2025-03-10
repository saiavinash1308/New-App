
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SocketManager;

public class PP : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public POP pathparent;
    Coroutine movePlayerPiece;
    public PPt previousPathPoint;
    public PPt currentPathPoint;
    public bool IsSafeZone { get; set; }
    //public string defpieceid;

    public void Awake()
    {
        pathparent = FindObjectOfType<POP>();
    }

    
    public void movestep(PPt[] pathpointstomoveon_)
    {
        //GM.game.piece = defpieceid;
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(PPt[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        GM.game.AddPathPoint(currentPathPoint);

        GM.game.canDiceRoll = true;
        //GM.game.selfDice = true;
        //GM.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(PPt[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = GM.game.numberofstepstoMove;

        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                GM.game.ads.Play();
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;

            GM.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);
            GM.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (currentPathPoint.name == "CP")
            {
                MarkPieceAsFinished();
            }

            //GM.game.selfDice = false;
            //GM.game.transferDice = true;

            GM.game.numberofstepstoMove = 0;
        }

        GM.game.canPlayermove = true;
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

        //GM.game.IncrementCenterCount(playerColor);
        //GM.game.CheckWinCondition();
        enabled = false;
    }

    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, PPt[] pathpointtomove_)
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
        PPt[] pathpoints = GetPathPointsForColor();

        if (numberofstepsalreadymove + steps < pathpoints.Length)
        {
            return true;
        }
        return false;
    }

    public PPt[] GetPathPointsForColor()
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
        PPt[] pathpoints = GetPathPointsForColor();
        return numberofstepsalreadymove + GM.game.numberofstepstoMove >= pathpoints.Length;
    }
}

