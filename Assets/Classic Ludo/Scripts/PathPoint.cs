/*  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
   PathPoint[] pathpointtomoveon_;
   public PathObjectParent pathObjectParent;
   public List<PlayerPieces> PlayerPiecesList = new List<PlayerPieces>();

   void Start()
   {
       pathObjectParent = GetComponentInParent<PathObjectParent>();
   }
   public bool AddPlayerPieces(PlayerPieces playerPiece_)
   {
       if (this.name != "PathPoint" && this.name != "PathPoint(8)" && this.name != "PathPoint(13)" && this.name != "PathPoint(21)" && this.name != "PathPoint(26)" && this.name != "PathPoint(34)" && this.name != "PathPoint(39)" && this.name != "PathPoint(47)" && this.name != "CenterPath")
       {
           if (PlayerPiecesList.Count == 1)

           {
               string prevplayerpiecename = PlayerPiecesList[0].name;
               string currplayerpiecename = playerPiece_.name;
               currplayerpiecename = currplayerpiecename.Substring(0, currplayerpiecename.Length - 4);

               if (!prevplayerpiecename.Contains(currplayerpiecename))

               {
                   PlayerPiecesList[0].isready = false;

                   StartCoroutine(revertonStart(PlayerPiecesList[0]));



                   PlayerPiecesList[0].numberofstepsalreadymove = 0;
                   RemovePlayerPieces(PlayerPiecesList[0]);
                   PlayerPiecesList.Add(playerPiece_);


                   return false;

               }
           }
       }

       addplayer(playerPiece_);
       return true;


   }



    IEnumerator revertonStart(PlayerPieces playerPiece_)
    {

        if (playerPiece_.name.Contains("Blue")) { GameManager.game.blueOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint; }
        else if (playerPiece_.name.Contains("Red")) { GameManager.game.redOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint; }
        else if (playerPiece_.name.Contains("Green")) { GameManager.game.greenOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint; }
        else if (playerPiece_.name.Contains("Yellow")) { GameManager.game.yellowOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.YellowPlayerPathPoint; }


        for (int i = playerPiece_.numberofstepsalreadymove; i >= 0; i--)
        {
            playerPiece_.transform.position = pathpointtomoveon_[i].transform.position;
            yield return new WaitForSeconds(0.02f);
        }

        PlayerPiecesList[0].transform.position = pathObjectParent.BasePoint[BasePointPosition(playerPiece_.name)].transform.position;
        //PlayerPiecesList[0].transform.position = pathObjectParent.BasePoint[];
    }

   int BasePointPosition(string name)
   {
       // if (name.Contains("Blue")) { GameManager.game.blueOutPlayers -= 1; }
       // else if (name.Contains("Green")) { GameManager.game.greenOutPlayers -= 1; }
       // else if (name.Contains("Red")) { GameManager.game.redOutPlayers -= 1; }
       //else if (name.Contains("Yellow")) { GameManager.game.yellowOutPlayers -= 1; }


       for (int i = 0; i < pathObjectParent.BasePoint.Length; i++)
       {
           if (pathObjectParent.BasePoint[i].name == name)
           {
               return i;
           }
       }

       return -1;
   }




   void addplayer(PlayerPieces playerPiece_)
   {

       PlayerPiecesList.Add(playerPiece_);
       RescaleandRepositiononAllPlayerPiece();
   }


   public void RemovePlayerPieces(PlayerPieces playerPiece_)
   {
       if (PlayerPiecesList.Contains(playerPiece_))
       {
           PlayerPiecesList.Remove(playerPiece_);
           RescaleandRepositiononAllPlayerPiece();
       }
   }
   public void RescaleandRepositiononAllPlayerPiece()
   {
       int placeCount = PlayerPiecesList.Count;
       bool isOdd = (placeCount % 2) == 0 ? false : true;
       int extend = placeCount / 2;
       int counter = 0;
       int spriteLayer = 0;

       if (isOdd)
       {

           for (int i = -extend; i <= extend; i++)
           {
               PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
               PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
               counter++;
           }



       }
       else
       {
           for (int i = -extend; i < extend; i++)
           {
               PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
               PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
               counter++;
           }
       }

       for (int i = 0; i < PlayerPiecesList.Count; i++)
       {
           PlayerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
           spriteLayer++;
       }
   }

}
*/


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathpointtomoveon_;
    public PathObjectParent pathObjectParent;
    public List<PlayerPieces> PlayerPiecesList = new List<PlayerPieces>();

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPieces(PlayerPieces playerPiece_)
    {
        if (!IsSafeZone())
        {
            if (PlayerPiecesList.Count == 1)
            {
                string prevplayerpiecename = PlayerPiecesList[0].name;
                string currplayerpiecename = playerPiece_.name;
                currplayerpiecename = currplayerpiecename.Substring(0, currplayerpiecename.Length - 4);

                if (!prevplayerpiecename.Contains(currplayerpiecename))
                {
                    PlayerPiecesList[0].isready = false;
                    StartCoroutine(revertonStart(PlayerPiecesList[0]));

                    PlayerPiecesList[0].numberofstepsalreadymove = 0;
                    RemovePlayerPieces(PlayerPiecesList[0]);
                    PlayerPiecesList.Add(playerPiece_);

                    // Mark that the player gets an additional roll for killing another player
                    GameManager.game.shouldRollAgain = true;

                    return false;
                }
            }
        }

        addplayer(playerPiece_);

        // Check if player reached the CenterPath and mark for an additional roll
        if (this.name == "CenterPath")
        {
            GameManager.game.shouldRollAgain = true;
        }

        // Bring the current player's piece to the top
        AdjustSortingOrder(playerPiece_);

        return true;
    }

    private bool IsSafeZone()
    {
        return this.name == "Pathpoint" || this.name == "Pathpoint (8)" || this.name == "Pathpoint (13)" ||
               this.name == "Pathpoint (21)" || this.name == "Pathpoint (26)" || this.name == "Pathpoint (34)" ||
               this.name == "Pathpoint (39)" || this.name == "Pathpoint (47)" || this.name == "CenterPath";
    }





   // private void AdjustSortingOrder(PlayerPieces playerPiece_)
   // {
    //    foreach (var piece in PlayerPiecesList)
    //    {
    //        piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
    //    }
//
        // Bring current piece to top if it matches the current rolling dice turn
     //   foreach (var piece in PlayerPiecesList)
     //   {
     //       if (GameManager.game.rolingDice == piece.GetComponentInParent<LudoHome>().rollingdice)
     //       {
      //          piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
     //       }
     //   }
  //  }

    private void AdjustSortingOrder(PlayerPieces playerPiece_)
     {
       foreach (var piece in PlayerPiecesList)
       {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
     }
 
    IEnumerator revertonStart(PlayerPieces playerPiece_)
    {
        if (playerPiece_.name.Contains("Blue")) { GameManager.game.blueOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint; }
        else if (playerPiece_.name.Contains("Red")) { GameManager.game.redOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint; }
        else if (playerPiece_.name.Contains("Green")) { GameManager.game.greenOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint; }
        else if (playerPiece_.name.Contains("Yellow")) { GameManager.game.yellowOutPlayers -= 1; pathpointtomoveon_ = pathObjectParent.YellowPlayerPathPoint; }

        for (int i = playerPiece_.numberofstepsalreadymove; i >= 0; i--)
        {
            if (i >= pathpointtomoveon_.Length) // Ensure 'i' is within the array bounds
            {
                Debug.LogError("Index out of bounds: " + i);
                continue; // Skip to the next iteration if the index is out of bounds
            }

            playerPiece_.transform.position = pathpointtomoveon_[i].transform.position;
            Debug.Log(playerPiece_.transform.position);
            yield return new WaitForSeconds(0.02f);
        }

        ResetToBasePoint(playerPiece_);
    }

    void ResetToBasePoint(PlayerPieces playerPiece_)
    {
        Debug.Log(playerPiece_.name);

        if (playerPiece_.name == "RedPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[1].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[2].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[3].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[4].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[5].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[6].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[7].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[8].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[9].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[10].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[11].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[12].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[13].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[14].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[15].transform.position;
        }
    }

    void addplayer(PlayerPieces playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        RescaleandRepositiononAllPlayerPiece();
    }

    public void RemovePlayerPieces(PlayerPieces playerPiece_)
    {
        if (PlayerPiecesList.Contains(playerPiece_))
        {
            PlayerPiecesList.Remove(playerPiece_);
            RescaleandRepositiononAllPlayerPiece();
        }
    }

    public void RescaleandRepositiononAllPlayerPiece()
    {
        int placeCount = PlayerPiecesList.Count;
        bool isOdd = (placeCount % 2) == 0 ? false : true;
        int extend = placeCount / 2;
        int counter = 0;
        int spriteLayer = 0;

        if (isOdd)
        {
            for (int i = -extend; i <= extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extend; i < extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < PlayerPiecesList.Count; i++)
        {
            PlayerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
}
*/


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathpointtomoveon_;
    public PathObjectParent pathObjectParent;
    public List<PlayerPieces> PlayerPiecesList = new List<PlayerPieces>();

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPieces(PlayerPieces playerPiece_)
    {
        if (!IsSafeZone())
        {
            if (PlayerPiecesList.Count == 1)
            {
                string prevplayerpiecename = PlayerPiecesList[0].name;
                string currplayerpiecename = playerPiece_.name;
                currplayerpiecename = currplayerpiecename.Substring(0, currplayerpiecename.Length - 4);

                if (!prevplayerpiecename.Contains(currplayerpiecename))
                {
                    PlayerPiecesList[0].isready = true;
                    StartCoroutine(revertonStart(PlayerPiecesList[0]));

                    PlayerPiecesList[0].numberofstepsalreadymove = 0;
                    RemovePlayerPieces(PlayerPiecesList[0]);
                    PlayerPiecesList.Add(playerPiece_);

                    // Mark that the player gets an additional roll for killing another player
                    GameManager.game.shouldRollAgain = true;

                    return false;
                }
            }
        }

        addplayer(playerPiece_);

        // Check if player reached the CenterPath and handle the game logic
        if (this.name == "CenterPath")
        {
            string playerColor = playerPiece_.name.Split('(')[0];
            GameManager.game.IncrementCenterCount(playerColor);

            if (GameManager.game.HasPlayerWon(playerColor))
            {
                GameManager.game.shouldRollAgain = false;
                GameManager.game.transferDice = true;
                GameManager.game.manageRolingDice[GetPlayerIndex(playerColor)].gameObject.SetActive(false);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.shouldRollAgain = true;
            }
        }

        // Bring the current player's piece to the top
        AdjustSortingOrder(playerPiece_);

        return true;
    }

    private int GetPlayerIndex(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return 0;
            case "Green":
                return 1;
            case "Red":
                return 2;
            case "Yellow":
                return 3;
            default:
                return -1;
        }
    }

    private bool IsSafeZone()
    {
        return this.name == "Pathpoint" || this.name == "Pathpoint (8)" || this.name == "Pathpoint (13)" ||
               this.name == "Pathpoint (21)" || this.name == "Pathpoint (26)" || this.name == "Pathpoint (34)" ||
               this.name == "Pathpoint (39)" || this.name == "Pathpoint (47)" || this.name == "CenterPath";
    }

    private void AdjustSortingOrder(PlayerPieces playerPiece_)
    {
        foreach (var piece in PlayerPiecesList)
        {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
    }

    IEnumerator revertonStart(PlayerPieces playerPiece_)
    {
        // Deduct the points from GameManager based on player color
        if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.game.blueOutPlayers = 4;
            pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint;
            GameManager.game.bluepoints -= playerPiece_.numberofstepsalreadymove - 1; // Deduct points
        }
        else if (playerPiece_.name.Contains("Red"))
        {
            GameManager.game.redOutPlayers = 4;
            pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint;
            GameManager.game.redpoints -= playerPiece_.numberofstepsalreadymove - 1; // Deduct points
        }
        else if (playerPiece_.name.Contains("Green"))
        {
            GameManager.game.greenOutPlayers = 4;
            pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint;
            GameManager.game.greenpoints -= playerPiece_.numberofstepsalreadymove - 1; // Deduct points
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.game.yellowOutPlayers = 4;
            pathpointtomoveon_ = pathObjectParent.YellowPlayerPathPoint;
            GameManager.game.yellowpoints -= playerPiece_.numberofstepsalreadymove - 1; // Deduct points
        }

        for (int i = playerPiece_.numberofstepsalreadymove; i >= 0; i--)
        {
            if (i >= pathpointtomoveon_.Length) // Ensure 'i' is within the array bounds
            {
                Debug.LogError("Index out of bounds: " + i);
                continue; // Skip to the next iteration if the index is out of bounds
            }

            playerPiece_.transform.position = pathpointtomoveon_[i].transform.position;
            Debug.Log(playerPiece_.transform.position);
            yield return new WaitForSeconds(0.02f);
        }

        ResetToBasePoint(playerPiece_);
    }


    void ResetToBasePoint(PlayerPieces playerPiece_)
    {
        Debug.Log(playerPiece_.name);

        if (playerPiece_.name == "RedPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
    }

    void addplayer(PlayerPieces playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        RescaleandRepositiononAllPlayerPiece();
    }

    public void RemovePlayerPieces(PlayerPieces playerPiece_)
    {
        if (PlayerPiecesList.Contains(playerPiece_))
        {
            PlayerPiecesList.Remove(playerPiece_);
            RescaleandRepositiononAllPlayerPiece();
        }
    }

    public void RescaleandRepositiononAllPlayerPiece()
    {
        int placeCount = PlayerPiecesList.Count;
        bool isOdd = (placeCount % 2) == 0 ? false : true;
        int extend = placeCount / 2;
        int counter = 0;
        int spriteLayer = 0;

        if (isOdd)
        {
            for (int i = -extend; i <= extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extend; i < extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < PlayerPiecesList.Count; i++)
        {
            PlayerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
}

*/

/*
//till transparency
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathpointtomoveon_;
    public PathObjectParent pathObjectParent;
    public List<PlayerPieces> PlayerPiecesList = new List<PlayerPieces>();

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPieces(PlayerPieces playerPiece_)
    {
        if (!IsSafeZone())
        {
            if (PlayerPiecesList.Count == 1)
            {
                string prevplayerpiecename = PlayerPiecesList[0].name;
                string currplayerpiecename = playerPiece_.name;
                currplayerpiecename = currplayerpiecename.Substring(0, currplayerpiecename.Length - 4);

                if (!prevplayerpiecename.Contains(currplayerpiecename))
                {
                    PlayerPiecesList[0].isready = true;
                    StartCoroutine(RevertToStart(PlayerPiecesList[0]));
                    StartCoroutine(AddKillerPoints(playerPiece_)); // Add points to the killer

                    PlayerPiecesList[0].numberofstepsalreadymove = 0;
                    PlayerPiecesList.Add(playerPiece_);

                    // Mark that the player gets an additional roll for killing another player
                    GameManager.game.shouldRollAgain = true;

                    return false;
                }
            }
        }

        AddPlayer(playerPiece_);

        // Check if player reached the CenterPath and handle the game logic
        if (this.name == "CenterPath")
        {
            string playerColor = playerPiece_.name.Split('(')[0];
            GameManager.game.IncrementCenterCount(playerColor);

            if (GameManager.game.HasPlayerWon(playerColor))
            {
                GameManager.game.shouldRollAgain = false;
                GameManager.game.transferDice = true;
                GameManager.game.manageRolingDice[GetPlayerIndex(playerColor)].gameObject.SetActive(false);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.shouldRollAgain = true;
            }
        }

        // Bring the current player's piece to the top
        AdjustSortingOrder(playerPiece_);

        return true;
    }

    private int GetPlayerIndex(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return 0;
            case "Green":
                return 1;
            case "Red":
                return 2;
            case "Yellow":
                return 3;
            default:
                return -1;
        }
    }

    private bool IsSafeZone()
    {
        return this.name == "Pathpoint" || this.name == "Pathpoint (8)" || this.name == "Pathpoint (13)" ||
               this.name == "Pathpoint (21)" || this.name == "Pathpoint (26)" || this.name == "Pathpoint (34)" ||
               this.name == "Pathpoint (39)" || this.name == "Pathpoint (47)" || this.name == "CenterPath";
    }

    private void AdjustSortingOrder(PlayerPieces playerPiece_)
    {
        foreach (var piece in PlayerPiecesList)
        {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
    }

    IEnumerator RevertToStart(PlayerPieces playerPiece_)
    {
        if (playerPiece_.name.Contains("Blue"))
        {
            pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint;
            GameManager.game.bluepoints -= playerPiece_.numberofstepsalreadymove; // Deduct points
        }
        else if (playerPiece_.name.Contains("Red"))
        {
            pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint;
            GameManager.game.redpoints -= playerPiece_.numberofstepsalreadymove; // Deduct points
        }
        else if (playerPiece_.name.Contains("Green"))
        {
            pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint;
            GameManager.game.greenpoints -= playerPiece_.numberofstepsalreadymove; // Deduct points
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            pathpointtomoveon_ = pathObjectParent.YellowPlayerPathPoint;
            GameManager.game.yellowpoints -= playerPiece_.numberofstepsalreadymove; // Deduct points
        }

        // Reset to start point
        playerPiece_.transform.position = pathpointtomoveon_[0].transform.position;
        playerPiece_.numberofstepsalreadymove = 0;
        playerPiece_.currentPathPoint = pathpointtomoveon_[0];

        yield return null;
    }

    IEnumerator AddKillerPoints(PlayerPieces killerPiece_)
    {
        if (killerPiece_.name.Contains("Blue"))
        {
            GameManager.game.bluepoints += 50;
        }
        else if (killerPiece_.name.Contains("Red"))
        {
            GameManager.game.redpoints += 50;
        }
        else if (killerPiece_.name.Contains("Green"))
        {
            GameManager.game.greenpoints += 50;
        }
        else if (killerPiece_.name.Contains("Yellow"))
        {
            GameManager.game.yellowpoints += 50;
        }

        yield return null;
    }

    void AddPlayer(PlayerPieces playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        RescaleandRepositiononAllPlayerPiece();
    }

    public void RemovePlayerPieces(PlayerPieces playerPiece_)
    {
        if (PlayerPiecesList.Contains(playerPiece_))
        {
            PlayerPiecesList.Remove(playerPiece_);
            RescaleandRepositiononAllPlayerPiece();
        }
    }

    public void RescaleandRepositiononAllPlayerPiece()
    {
        int placeCount = PlayerPiecesList.Count;
        bool isOdd = (placeCount % 2) == 0 ? false : true;
        int extend = placeCount / 2;
        int counter = 0;
        int spriteLayer = 0;

        if (isOdd)
        {
            for (int i = -extend; i <= extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extend; i < extend; i++)
            {
                PlayerPiecesList[counter].transform.localScale = new Vector3(pathObjectParent.scales[placeCount - 1], pathObjectParent.scales[placeCount - 1], 1f);
                PlayerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[placeCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < PlayerPiecesList.Count; i++)
        {
            PlayerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathpointtomoveon_;
    public PathObjectParent pathObjectParent;
    public List<PlayerPieces> PlayerPiecesList = new List<PlayerPieces>();

    [SerializeField] private GameObject killingPopPrefab; // Add this line

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPieces(PlayerPieces playerPiece_)
    {

        if (IsSafeZone())
        {
            playerPiece_.IsInSafeZone = true;
        }
        else
        {
            playerPiece_.IsInSafeZone = false;
        }

        if (!IsSafeZone())
        {
            if (PlayerPiecesList.Count == 1)
            {
                string prevplayerpiecename = PlayerPiecesList[0].name;
                string currplayerpiecename = playerPiece_.name;
                currplayerpiecename = currplayerpiecename.Substring(0, currplayerpiecename.Length - 4);

                if (!prevplayerpiecename.Contains(currplayerpiecename))
                {
                    PlayerPiecesList[0].isready = true;
                    StartCoroutine(RevertonStart(PlayerPiecesList[0]));
                    StartCoroutine(AddKillerPoints(playerPiece_)); // Add points to the killer

                    // Instantiate the killing pop effect
                    InstantiateKillingPop(PlayerPiecesList[0].transform.position);

                    PlayerPiecesList[0].numberofstepsalreadymove = 0;
                    RemovePlayerPieces(PlayerPiecesList[0]);
                    PlayerPiecesList.Add(playerPiece_);

                    // Mark that the player gets an additional roll for killing another player
                    GameManager.game.shouldRollAgain = true;

                    return false;
                }
            }
        }

        AddPlayer(playerPiece_);

        // Check if player reached the CenterPath and handle the game logic
        if (this.name == "CenterPath")
        {
            string playerColor = playerPiece_.name.Split('(')[0];
            GameManager.game.IncrementCenterCount(playerColor);

            // Play the center path audio when a player piece reaches the CenterPath
            GameManager.game.PlayCenterPathAudio();

            if (GameManager.game.HasPlayerWon(playerColor))
            {
                GameManager.game.shouldRollAgain = false;
                GameManager.game.transferDice = true;
                GameManager.game.manageRolingDice[GetPlayerIndex(playerColor)].gameObject.SetActive(false);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.shouldRollAgain = true;
            }
        }

        // Bring the current player's piece to the top
        AdjustSortingOrder(playerPiece_);

        return true;
    }

    private int GetPlayerIndex(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return 0;
            case "Green":
                return 1;
            case "Red":
                return 2;
            case "Yellow":
                return 3;
            default:
                return -1;
        }
    }

    public bool IsSafeZone()
    {
        return this.name == "Pathpoint" || this.name == "Pathpoint (8)" || this.name == "Pathpoint (13)" ||
               this.name == "Pathpoint (21)" || this.name == "Pathpoint (26)" || this.name == "Pathpoint (34)" ||
               this.name == "Pathpoint (39)" || this.name == "Pathpoint (47)" || this.name == "CenterPath";
    }

    private void AdjustSortingOrder(PlayerPieces playerPiece_)
    {
        foreach (var piece in PlayerPiecesList)
        {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
    }

    IEnumerator RevertonStart(PlayerPieces playerPiece_)
    {
        if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.game.blueOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("Red"))
        {
            GameManager.game.redOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("Green"))
        {
            GameManager.game.greenOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.game.yellowOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.YellowPlayerPathPoint;
        }

        for (int i = playerPiece_.numberofstepsalreadymove; i >= 0; i--)
        {
            if (i >= pathpointtomoveon_.Length) // Ensure 'i' is within the array bounds
            {
                Debug.LogError("Index out of bounds: " + i);
                continue; // Skip to the next iteration if the index is out of bounds
            }

            playerPiece_.transform.position = pathpointtomoveon_[i].transform.position;
            pathObjectParent.killSound.Play();
            Debug.Log(playerPiece_.transform.position);
            yield return new WaitForSeconds(0.02f);
        }

        ResetToBasePoint(playerPiece_);
    }

    IEnumerator AddKillerPoints(PlayerPieces killerPiece_)
    {
        if (killerPiece_.name.Contains("Blue"))
        {
            GameManager.game.bluepoints += 50;
        }
        else if (killerPiece_.name.Contains("Red"))
        {
            GameManager.game.redpoints += 50;
        }
        else if (killerPiece_.name.Contains("Green"))
        {
            GameManager.game.greenpoints += 50;
        }
        else if (killerPiece_.name.Contains("Yellow"))
        {
            GameManager.game.yellowpoints += 50;
        }

        yield return null;
    }

    void ResetToBasePoint(PlayerPieces playerPiece_)
    {
        Debug.Log(playerPiece_.name);

        if (playerPiece_.name == "RedPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "RedPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[0].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "GreenPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[13].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "YellowPlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[26].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(1)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(2)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
        else if (playerPiece_.name == "BluePlayerPieces(3)")
        {
            playerPiece_.transform.position = pathObjectParent.CommonPathPoint[39].transform.position;
        }
    }

    void AddPlayer(PlayerPieces playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        RescaleandRepositiononAllPlayerPiece();
    }

    public void RemovePlayerPieces(PlayerPieces playerPiece_)
    {
        if (PlayerPiecesList.Contains(playerPiece_))
        {
            PlayerPiecesList.Remove(playerPiece_);
            RescaleandRepositiononAllPlayerPiece();
        }
    }

    void RescaleandRepositiononAllPlayerPiece()
    {
        // Number of pieces to be arranged
        int pieceCount = PlayerPiecesList.Count;

        if (pieceCount == 0) return;

        // Determine the size of the grid
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount)); // Calculate grid size for a square formation

        float baseSpacing = 2.0f; // Base spacing between pieces
        float maxSize = 14.0f; // Default max size of pieces
        float minSize = 6.5f; // Minimum size of pieces

        if (pieceCount > 1)
        {
            baseSpacing = 2.0f; // Example adjustment for spacing
            minSize = 12.0f; // Example adjustment for minimum size
            maxSize = 5.5f; // Example adjustment for maximum size
        }

        // Calculate spacing and scale based on the number of pieces
        float totalSize = baseSpacing * gridSize;
        float scale = Mathf.Clamp(totalSize / (baseSpacing * gridSize), minSize, maxSize);

        // Calculate the starting position for the grid
        Vector3 startPosition = transform.position - new Vector3((gridSize - 1) * baseSpacing / 2, (gridSize - 1) * baseSpacing / 2, 0);

        for (int i = 0; i < pieceCount; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            // Calculate the position in the grid
            Vector3 piecePosition = startPosition + new Vector3(col * baseSpacing, row * baseSpacing, 0);

            // Reposition the piece
            PlayerPiecesList[i].transform.position = piecePosition;

            // Apply scaling to the piece
            PlayerPiecesList[i].transform.localScale = new Vector3(scale, scale, scale);
        }
    }




    void InstantiateKillingPop(Vector3 position)
    {
        GameObject killingPopInstance = Instantiate(killingPopPrefab, position, Quaternion.identity);
        Destroy(killingPopInstance, 1.5f); // Destroy the killing pop object after 3 seconds
    }
}













































