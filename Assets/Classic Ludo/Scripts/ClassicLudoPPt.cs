
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ClassicLudoGM;

public class ClassicLudoPPt : MonoBehaviour
{
    ClassicLudoPPt[] pathpointtomoveon_;
    public ClassicLudoPOP pathObjectParent;
    public List<ClassicLudoPP> PlayerPiecesList = new List<ClassicLudoPP>();

    [SerializeField] private GameObject killingPopPrefab; // Add this line

    private SocketManager socketManager;

    void Start()
    {
        socketManager = FindObjectOfType<SocketManager>();
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }
        pathObjectParent = GetComponentInParent<ClassicLudoPOP>();
    }

    public bool AddPlayerPieces(ClassicLudoPP playerPiece_)
    {

        Debug.Log("Adding player piece to: " + this.name);
        if (socketManager != null && socketManager.isConnected)
        {
            string name = this.name;  // Get the name of the GameObject
            string numberPart = new string(name.Where(char.IsDigit).ToArray());
            MovePieceUpdatePayload payload = new MovePieceUpdatePayload
            {
                //pieceId = numberPart,
                piecePosition = int.Parse(numberPart),
            };
            string jsonPayload = JsonUtility.ToJson(payload);
            //socketManager.socket.Emit("MOVE_UPDATED", jsonPayload);
            Debug.LogWarning("MovePieceUpdated:" + jsonPayload);
            Debug.Log("Sent piece ID: " + numberPart);
        }
        else
        {
            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
        }
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
                    //ClassicLudoGM.game.shouldRollAgain = true;
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
            //ClassicLudoGM.game.IncrementCenterCount(playerColor);

            // Play the center path audio when a player piece reaches the CenterPath
            ClassicLudoGM.game.PlayCenterPathAudio();

            if (ClassicLudoGM.game.HasPlayerWon(playerColor))
            {
                //ClassicLudoGM.game.shouldRollAgain = false;
                //ClassicLudoGM.game.transferDice = true;
                ClassicLudoGM.game.manageRolingDice[GetPlayerIndex(playerColor)].gameObject.SetActive(false);
                //GM.game.RolingDiceManager();
            }
            else
            {
                //ClassicLudoGM.game.shouldRollAgain = true;
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
        return this.name == "CPP" || this.name == "CPP8" || this.name == "CPP13" ||
               this.name == "CPP21" || this.name == "CPP26" || this.name == "CPP34" ||
               this.name == "CPP39" || this.name == "CPP47" || this.name == "CP";
    }


    private void AdjustSortingOrder(ClassicLudoPP playerPiece_)
    {
        foreach (var piece in PlayerPiecesList)
        {
            piece.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; // Reset sorting order
        }

        playerPiece_.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; // Bring current piece to top
    }

    IEnumerator revertonStart(ClassicLudoPP playerPiece_)
    {
        if (playerPiece_.name.Contains("B"))
        {
            ClassicLudoGM.game.blueOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.BluePlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("R"))
        {
            ClassicLudoGM.game.redOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.RedPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("G"))
        {
            ClassicLudoGM.game.greenOutPlayers -= 1;
            pathpointtomoveon_ = pathObjectParent.GreenPlayerPathPoint;
        }
        else if (playerPiece_.name.Contains("Y"))
        {
            ClassicLudoGM.game.yellowOutPlayers -= 1;
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

    public void ResetToBasePoint(ClassicLudoPP playerPiece_)
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
    }


    void AddPlayer(ClassicLudoPP playerPiece_)
    {
        PlayerPiecesList.Add(playerPiece_);
        ArrangePiecesInGrid();
    }

    public void RemovePlayerPieces(ClassicLudoPP playerPiece_)
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

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount)); // Determine grid dimensions
        float spacing = 0.10f;  // Adjust spacing between pieces
        float yOffset = 0.10f;  // Move grid up/down

        // Calculate total width and height of the grid
        float totalWidth = (gridSize - 1) * spacing;
        float totalHeight = (gridSize - 1) * spacing;

        // Calculate start position to center the grid
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2, totalHeight / 2 - yOffset, 0);

        for (int i = 0; i < pieceCount; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            // Compute new position
            Vector3 newPosition = startPosition + new Vector3(col * spacing, -row * spacing, 0);

            // Apply position to piece
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
