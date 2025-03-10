/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPieces : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public PathObjectParent pathparent;
    Coroutine movePlayerPiece;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;

    public void Awake()
    {
        pathparent = FindObjectOfType<PathObjectParent>();
    }

    public void movestep(PathPoint[] pathpointstomoveon_)
    {
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(PathPoint[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        GameManager.game.AddPathPoint(currentPathPoint);

        GameManager.game.canDiceRoll = true;
        GameManager.game.selfDice = true;
        GameManager.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(PathPoint[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = GameManager.game.numberofstepstoMove;


        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {

            //previousPathPoint.RescaleandRepositiononAllPlayerPiece();
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                yield return new WaitForSeconds(0.35f);
            }


        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;


            GameManager.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);

            GameManager.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;


            if (GameManager.game.numberofstepstoMove != 6)
            {
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
            }
            else
            {
                GameManager.game.selfDice = true;
                GameManager.game.transferDice = false;
            }
            GameManager.game.numberofstepstoMove = 0;
        }


        GameManager.game.canPlayermove = true;
        GameManager.game.RolingDiceManager();

        if (movePlayerPiece != null)
        {
            StopCoroutine("movesteps_Enum");
        }
    }
    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, PathPoint[] pathpointtomove_)
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
}
*/

/*
//final
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieces : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public PathObjectParent pathparent;
    Coroutine movePlayerPiece;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;
   
    public void Awake()
    {
        pathparent = FindObjectOfType<PathObjectParent>();
    }

    public void movestep(PathPoint[] pathpointstomoveon_)
    {
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(PathPoint[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        GameManager.game.AddPathPoint(currentPathPoint);

        GameManager.game.canDiceRoll = true;
        GameManager.game.selfDice = true;
        GameManager.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(PathPoint[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = GameManager.game.numberofstepstoMove;

        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                GameManager.game.ads.Play();
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;

            GameManager.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);
            GameManager.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (currentPathPoint.name == "CenterPath")
            {
                MarkPieceAsFinished();
            }

            if (GameManager.game.numberofstepstoMove != 6)
            {
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
            }
            else
            {
                GameManager.game.selfDice = true;
                GameManager.game.transferDice = false;
            }
            GameManager.game.numberofstepstoMove = 0;
        }

        GameManager.game.canPlayermove = true;
        GameManager.game.RolingDiceManager();

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
            GameManager.game.bluepoints += 100;
            GameManager.game.bluepoints = Mathf.Max(0, GameManager.game.bluepoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Red"))
        {
            playerColor = "Red";
            GameManager.game.redpoints += 100;
            GameManager.game.redpoints = Mathf.Max(0, GameManager.game.redpoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Green"))
        {
            playerColor = "Green";
            GameManager.game.greenpoints += 100;
            GameManager.game.greenpoints = Mathf.Max(0, GameManager.game.greenpoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Yellow"))
        {
            playerColor = "Yellow";
            GameManager.game.yellowpoints += 100;
            GameManager.game.yellowpoints = Mathf.Max(0, GameManager.game.yellowpoints); // Ensure score doesn't go negative
        }

        GameManager.game.IncrementCenterCount(playerColor);
        GameManager.game.CheckWinCondition();
        enabled = false;
    }

    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, PathPoint[] pathpointtomove_)
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
        PathPoint[] pathpoints = GetPathPointsForColor();

        if (numberofstepsalreadymove + steps < pathpoints.Length)
        {
            return true;
        }
        return false;
    }

    public PathPoint[] GetPathPointsForColor()
    {
        if (name.Contains("Blue"))
        {
            return pathparent.BluePlayerPathPoint;
        }
        else if (name.Contains("Red"))
        {
            return pathparent.RedPlayerPathPoint;
        }
        else if (name.Contains("Green"))
        {
            return pathparent.GreenPlayerPathPoint;
        }
        else if (name.Contains("Yellow"))
        {
            return pathparent.YellowPlayerPathPoint;
        }
        return null;
    }

    public bool IsCloseToCenterPath()
    {
        PathPoint[] pathpoints = GetPathPointsForColor();
        return numberofstepsalreadymove + GameManager.game.numberofstepstoMove >= pathpoints.Length;
    }
}

*/













/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieces : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public PathObjectParent pathparent;
    Coroutine movePlayerPiece;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;

    public void Awake()
    {
        pathparent = FindObjectOfType<PathObjectParent>();
    }

    public void movestep(PathPoint[] pathpointstomoveon_)
    {
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(PathPoint[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        GameManager.game.AddPathPoint(currentPathPoint);

        GameManager.game.canDiceRoll = true;
        GameManager.game.selfDice = true;
        GameManager.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(PathPoint[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = GameManager.game.numberofstepstoMove;

        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                yield return new WaitForSeconds(0.1f);
            }
        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;

            GameManager.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);
            GameManager.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (currentPathPoint.name == "CenterPath")
            {
                MarkPieceAsFinished();
            }

            if (GameManager.game.numberofstepstoMove != 6)
            {
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
            }
            else
            {
                GameManager.game.selfDice = true;
                GameManager.game.transferDice = false;
            }
            GameManager.game.numberofstepstoMove = 0;
        }

        GameManager.game.canPlayermove = true;
        GameManager.game.RolingDiceManager();

        if (movePlayerPiece != null)
        {
            StopCoroutine("movesteps_Enum");
        }
    }

    private void MarkPieceAsFinished()
    {
        // Check and update the finished player count based on the piece's color
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

        GameManager.game.IncrementCenterCount(playerColor);
        GameManager.game.CheckWinCondition();
        enabled = false; // Disable this piece as it has finished the game
    }

    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, PathPoint[] pathpointtomove_)
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
}

*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieces : MonoBehaviour
{
    public bool isready;
    public bool movenow;
    public int numberofstepstoMove;
    public int numberofstepsalreadymove;
    public PathObjectParent pathparent;
    Coroutine movePlayerPiece;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;
    public bool IsInSafeZone { get; set; }

    public void Awake()
    {
        pathparent = FindObjectOfType<PathObjectParent>();
    }

    public void movestep(PathPoint[] pathpointstomoveon_)
    {
        movePlayerPiece = StartCoroutine(movesteps_Enum(pathpointstomoveon_));
    }

    public void makeplayerreadytomove(PathPoint[] pathpointstomoveon_)
    {
        isready = true;
        transform.position = pathpointstomoveon_[0].transform.position;
        numberofstepsalreadymove = 1;

        previousPathPoint = pathpointstomoveon_[0];
        currentPathPoint = pathpointstomoveon_[0];
        currentPathPoint.AddPlayerPieces(this);
        GameManager.game.AddPathPoint(currentPathPoint);

        GameManager.game.canDiceRoll = true;
        GameManager.game.selfDice = true;
        GameManager.game.transferDice = false;
    }

    IEnumerator movesteps_Enum(PathPoint[] pathpointstomoveon_)
    {
        yield return new WaitForSeconds(0.25f);
        numberofstepstoMove = GameManager.game.numberofstepstoMove;

        for (int i = numberofstepsalreadymove; i < (numberofstepsalreadymove + numberofstepstoMove); i++)
        {
            if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
            {
                transform.position = pathpointstomoveon_[i].transform.position;
                GameManager.game.ads.Play();
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (isPathPointAvailabletomove(numberofstepstoMove, numberofstepsalreadymove, pathpointstomoveon_))
        {
            numberofstepsalreadymove += numberofstepstoMove;

            GameManager.game.RemovePathPoint(currentPathPoint);
            previousPathPoint.RemovePlayerPieces(this);
            currentPathPoint = pathpointstomoveon_[numberofstepsalreadymove - 1];

            currentPathPoint.AddPlayerPieces(this);
            GameManager.game.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (currentPathPoint.name == "CenterPath")
            {
                MarkPieceAsFinished();
            }

            if (GameManager.game.numberofstepstoMove != 6)
            {
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
            }
            else
            {
                GameManager.game.selfDice = true;
                GameManager.game.transferDice = false;
            }
            GameManager.game.numberofstepstoMove = 0;
        }

        GameManager.game.canPlayermove = true;
        GameManager.game.RolingDiceManager();

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
            GameManager.game.bluepoints += 100;
            GameManager.game.bluepoints = Mathf.Max(0, GameManager.game.bluepoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Red"))
        {
            playerColor = "Red";
            GameManager.game.redpoints += 100;
            GameManager.game.redpoints = Mathf.Max(0, GameManager.game.redpoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Green"))
        {
            playerColor = "Green";
            GameManager.game.greenpoints += 100;
            GameManager.game.greenpoints = Mathf.Max(0, GameManager.game.greenpoints); // Ensure score doesn't go negative
        }
        else if (name.Contains("Yellow"))
        {
            playerColor = "Yellow";
            GameManager.game.yellowpoints += 100;
            GameManager.game.yellowpoints = Mathf.Max(0, GameManager.game.yellowpoints); // Ensure score doesn't go negative
        }

        GameManager.game.IncrementCenterCount(playerColor);
        GameManager.game.CheckWinCondition();
        enabled = false;
    }

    bool isPathPointAvailabletomove(int numberofstepstoMove_, int numberofstepsalreadymove_, PathPoint[] pathpointtomove_)
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
        PathPoint[] pathpoints = GetPathPointsForColor();

        if (numberofstepsalreadymove + steps < pathpoints.Length)
        {
            return true;
        }
        return false;
    }

    public PathPoint[] GetPathPointsForColor()
    {
        if (name.Contains("Blue"))
        {
            return pathparent.BluePlayerPathPoint;
        }
        else if (name.Contains("Red"))
        {
            return pathparent.RedPlayerPathPoint;
        }
        else if (name.Contains("Green"))
        {
            return pathparent.GreenPlayerPathPoint;
        }
        else if (name.Contains("Yellow"))
        {
            return pathparent.YellowPlayerPathPoint;
        }
        return null;
    }

    public bool IsCloseToCenterPath()
    {
        PathPoint[] pathpoints = GetPathPointsForColor();
        return numberofstepsalreadymove + GameManager.game.numberofstepstoMove >= pathpoints.Length;
    }
}










