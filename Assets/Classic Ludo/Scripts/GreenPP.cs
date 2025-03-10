
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GM;

public class GreenPP : PP
{
    RD greenHomeRollingDice;
    private SocketManager socketManager;
    //public Button Piece;
    void Start()
    {
        socketManager = FindObjectOfType<SocketManager>();
        if (socketManager == null)
        {
            Debug.LogError("SocketManager not found!");
            return; // Exit early to avoid null reference later
        }
        GM.game.greenOutPlayers = 4;
        makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
        greenHomeRollingDice = GetComponentInParent<Green>().rollingdice;

        

        //Piece.onClick.AddListener(delegate { OnMouseDown(); });
    }



    public void OnMouseDown()
    {
        if (GM.game.rolingDice == GM.game.manageRolingDice[2])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.GreenPlayerPathPoint);
                //GM.game.transferDice = false;
                //GM.game.RolingDiceManager(); // After moving, transfer the dice
                //GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == greenHomeRollingDice)
            {
                GM.game.greenOutPlayers = 4;
                makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                //GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
        if (socketManager != null && socketManager.isConnected)
        {
            if (socketManager.getMySocketId() == GM.game.turnSocketId)
            {

                    string name = this.name;  // Get the name of the GameObject
                string numberPart = new string(name.Where(char.IsDigit).ToArray());
                string roomId = socketManager.GetRoomId();
                MovePiecePayload payload = new MovePiecePayload
                {
                    piece = numberPart,
                    roomId = roomId
                };
                string jsonPayload = JsonUtility.ToJson(payload);
                socketManager.socket.Emit("MOVE_PIECE", jsonPayload);
                Debug.Log("MovePiece:" + jsonPayload);
                Debug.Log("Sent piece ID: " + numberPart);
                Debug.Log("Sent room Id:" + roomId);
            }
        }
        else
        {
            Debug.LogWarning("SocketManager is not connected. Cannot emit ROLL_DICE.");
        }
    }

    //internal void GreenMovePiece()
    //{
    //    if (socketManager.socket.Id != GM.game.turnSocketId)
    //    {
    //        Debug.LogWarning("It's not this player's turn.");
    //        return;
    //    }
    //    else
    //    {

    //        //MovePiece();
    //        socketManager.socket.Emit("MOVE_UPDATED", "");
    //    }
    //}

    public void MovePiece()
    {
        Debug.LogWarning("Green Piece Moving");
        if (GM.game.rolingDice == GM.game.manageRolingDice[2])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.GreenPlayerPathPoint);

                

                //GreenMovePiece();
                //GM.game.transferDice = false;
                //GM.game.RolingDiceManager(); // After moving, transfer the dice
                //GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == greenHomeRollingDice)
            {
                GM.game.greenOutPlayers = 4;
                makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                //GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
        //if (socketManager != null && socketManager.isConnected)
        //{
        //    Debug.Log("Emitting UPDATE_MOVE for object: " + gameObject.name);
        //    socketManager.socket.Emit("MOVE_UPDATED", "");
        //}
    }


}