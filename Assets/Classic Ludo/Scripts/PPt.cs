
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GM;

public class PPt : MonoBehaviour
{
    PPt[] pathpointtomoveon_;
    public POP pathObjectParent;
    public List<PP> PlayerPiecesList = new List<PP>();

    [SerializeField] private GameObject killingPopPrefab; // Add this line

    //private SocketManager socketManager;

    void Start()
    {
        //socketManager = FindObjectOfType<SocketManager>();
        //if (socketManager == null)
        //{
        //    Debug.LogError("SocketManager not found!");
        //    return; // Exit early to avoid null reference later
        //}
        pathObjectParent = GetComponentInParent<POP>();
    }

    public bool AddPlayerPieces(PP playerPiece_)
    {

        Debug.Log("Adding player piece to: " + this.name);
        Debug.Log("Is this a safe zone? " + IsSafeZone());

        if (IsSafeZone())
        {
            playerPiece_.IsSafeZone = true;
        }
        else
        {
            playerPiece_.IsSafeZone = false;
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
                    PlayerPiecesList[0].isready = false;
                    StartCoroutine(revertonStart(PlayerPiecesList[0]));


                    // Instantiate the killing pop effect
                    InstantiateKillingPop(PlayerPiecesList[0].transform.position);

                    PlayerPiecesList[0].numberofstepsalreadymove = 0;
                    RemovePlayerPieces(PlayerPiecesList[0]);
                    PlayerPiecesList.Add(playerPiece_);

                    // Mark that the player gets an additional roll for killing another player
                    //GM.game.shouldRollAgain = true;
                    //Handheld.Vibrate();

                    return false;
                }
            }
        }

        AddPlayer(playerPiece_);

        // Check if player reached the CenterPath and handle the game logic
        if (this.name == "CP")
        {
            string playerColor = playerPiece_.name.Split('(')[0];
            //GM.game.IncrementCenterCount(playerColor);

            // Play the center path audio when a player piece reaches the CenterPath
            GM.game.PlayCenterPathAudio();

            if (GM.game.HasPlayerWon(playerColor))
            {
                //GM.game.shouldRollAgain = false;
                //GM.game.transferDice = true;
                GM.game.manageRolingDice[GetPlayerIndex(playerColor)].gameObject.SetActive(false);
                //GM.game.RolingDiceManager();
            }
            else
            {
                //GM.game.shouldRollAgain = true;
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
        Debug.Log("Checking safe zone for: " + this.name); // Debug line to check the name
        return this.name == "CPP0" || this.name == "CPP8" || this.name == "CPP13" ||
               this.name == "CPP21" || this.name == "CPP26" || this.name == "CPP34" ||
               this.name == "CPP39" || this.name == "CPP47" || this.name == "CP";
    }


    private void AdjustSortingOrder(PP playerPiece_)
    {
        foreach (var piece in PlayerPiecesList)
        {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
    }

    IEnumerator revertonStart(PP playerPiece_)
    {
        if (playerPiece_.name.Contains("B"))
        {
            GM.game.blueOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("R"))
        {
            GM.game.redOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("G"))
        {
            GM.game.greenOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("Y"))
        {
            GM.game.yellowOutPlayers -= 1;
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

    public void ResetToBasePoint(PP playerPiece_)
    {
        Debug.Log(playerPiece_.name);

        if (playerPiece_.name == "Red0")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[0].transform.position;
        }
        else if (playerPiece_.name == "Red1")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[1].transform.position;
        }
        else if (playerPiece_.name == "Red2")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[2].transform.position;
        }
        else if (playerPiece_.name == "Red3")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[3].transform.position;
        }
        else if (playerPiece_.name == "Green0")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[4].transform.position;
        }
        else if (playerPiece_.name == "Green1")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[5].transform.position;
        }
        else if (playerPiece_.name == "Green2")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[6].transform.position;
        }
        else if (playerPiece_.name == "Green3")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[7].transform.position;
        }
        else if (playerPiece_.name == "Yellow0")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[8].transform.position;
        }
        else if (playerPiece_.name == "Yellow1")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[9].transform.position;
        }
        else if (playerPiece_.name == "Yellow2")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[10].transform.position;
        }
        else if (playerPiece_.name == "Yellow3")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[11].transform.position;
        }
        else if (playerPiece_.name == "Blue0")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[12].transform.position;
        }
        else if (playerPiece_.name == "Blue1")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[13].transform.position;
        }
        else if (playerPiece_.name == "Blue2")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[14].transform.position;
        }
        else if (playerPiece_.name == "Blue3")
        {
            playerPiece_.transform.position = pathObjectParent.BasePoint[15].transform.position;
        }
        //if (socketManager != null && socketManager.isConnected)
        //{
        //        socketManager.socket.Emit("MOVE_UPDATED", "");
            
        //}
        //else
        //{ 
        //    Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
        //}
    }


    void AddPlayer(PP playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        ArrangePiecesInGrid();
    }

    public void RemovePlayerPieces(PP playerPiece_)
    {
        if (PlayerPiecesList.Contains(playerPiece_))
        {
            PlayerPiecesList.Remove(playerPiece_);
            ArrangePiecesInGrid();
        }
    }

    private void ArrangePiecesInGrid()
    {
        int pieceCount = PlayerPiecesList.Count;
        if (pieceCount == 0) return;

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount));
        float spacing = 0.10f;
        float yOffset = 0.10f; // Adjust this value to move the grid up or down

        Vector3 startPosition = transform.position - new Vector3(spacing * (gridSize - 1) / 2, spacing * (gridSize - 1) / 2 - yOffset, 0);

        for (int i = 0; i < pieceCount; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            Vector3 newPosition = startPosition + new Vector3(col * spacing, -row * spacing, 0);
            PlayerPiecesList[i].transform.position = newPosition;
        }
    }



    void InstantiateKillingPop(Vector3 position)
    {
        GameObject killingPopInstance = Instantiate(killingPopPrefab, position, Quaternion.identity);
        Destroy(killingPopInstance, 1.5f); // Destroy the killing pop object after 3 seconds
    }

    public class MovePieceUpdatePayload
    {
        public int piecePosition;  
    }
}
