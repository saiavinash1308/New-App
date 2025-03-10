/*
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager game;
  public RollingDice rolingDice;
  public int numberofstepstoMove;
  public bool canPlayermove = true;
  List<PathPoint> playeronpathpointList = new List<PathPoint>();

  public bool canDiceRoll = true;
  public bool transferDice = false;
  public bool selfDice = false;
  public int blueOutPlayers;
  public int redOutPlayers;
  public int greenOutPlayers;
  public int yellowOutPlayers;


  public RollingDice[] manageRolingDice;



  private void Awake()
  {
      game = this;
  }
  public void AddPathPoint(PathPoint pathPoint)
  {
      playeronpathpointList.Add(pathPoint);
  }
  public void RemovePathPoint(PathPoint pathPoint)
  {
      if (playeronpathpointList.Contains(pathPoint))
      {
          playeronpathpointList.Remove(pathPoint);
      }
      else
      {
          Debug.Log("Path point donot found to be removed");
      }
  }
  public void RollDice()
  {
      numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
      Debug.Log("Dice rolled: " + numberofstepstoMove);

      // Here you can trigger the player pieces to move
      // You might need a reference to the current player piece
      // For example:
      // currentPlayerPiece.Move();

  }
  public void RolingDiceManager()
  {
      int nextDice;
      if (GameManager.game.transferDice)
      {

          for(int i =0; i < 4; i++)
          {
              if (i == 3)
              {
                  nextDice = 0;
              }
              else
              {
                  nextDice = i + 1;
              }

              if(GameManager.game.rolingDice == GameManager.game.manageRolingDice[i])
              {
                  GameManager.game.manageRolingDice[i].gameObject.SetActive(false);
                  GameManager.game.manageRolingDice[nextDice].gameObject.SetActive(true);
              }
          }
          GameManager.game.canDiceRoll = true;
      }
      else
      {
          if (GameManager.game.selfDice)
          {
              GameManager.game.selfDice = false;
              GameManager.game.canDiceRoll = true;
          }
      }
  }
} 
*/



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;

    private void Awake()
    {
        game = this;
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }
   
    public void RollDice()
    {
        numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        Debug.Log("Dice rolled: " + numberofstepstoMove);
    }
    
    public void RolingDiceManager()
    {
        if (GameManager.game.shouldRollAgain)
        {
            GameManager.game.shouldRollAgain = false;
            GameManager.game.canDiceRoll = true;
            return;
        }

        int nextDice;
        if (GameManager.game.transferDice)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }

                if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[i])
                {
                    GameManager.game.manageRolingDice[i].gameObject.SetActive(false);
                    GameManager.game.manageRolingDice[nextDice].gameObject.SetActive(true);
                    GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                }
            }
            GameManager.game.canDiceRoll = true;
        }
        else
        {
            if (GameManager.game.selfDice)
            {
                GameManager.game.selfDice = false;
                GameManager.game.canDiceRoll = true;
            }
        }
    }

   

    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        // Disable all player pieces and dice rolls
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;

        // Additional end game logic like showing a winner screen can go here
    }

    private void LoadWinnerScene(string winner)
    {
        // You can pass the winner information using PlayerPrefs or any other method
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }
}
*/

//Last Updated Code
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public int redpoints = 0;
    public int yellowpoints = 0;
    public int greenpoints = 0;
    public int bluepoints = 0;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;

    private void Awake()
    {
        game = this;
        rolingDice = manageRolingDice[0];
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }

    public void RollDice()
    {
        numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        Debug.Log("Dice rolled: " + numberofstepstoMove);


    }

    public void UpdatePlayerPoints(int points)
    {
        Debug.Log("Hello");
        // Update points based on the active dice
        if (rolingDice == manageRolingDice[0])
        {
            bluepoints += points;
            Debug.Log("BluePoints" + bluepoints);
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenpoints += points;
            Debug.Log("GreenPoints" + redpoints);
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redpoints += points;
            Debug.Log("RedPoints" + greenpoints);
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowpoints += points;
            Debug.Log("YellowPoints" + yellowpoints);
        }
    }

    public void RolingDiceManager()
    {
        if (GameManager.game.shouldRollAgain)
        {
            GameManager.game.shouldRollAgain = false;
            GameManager.game.canDiceRoll = true;
            return;
        }

        int nextDice;
        if (GameManager.game.transferDice)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }

                if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[i])
                {
                    GameManager.game.manageRolingDice[i].gameObject.SetActive(false);
                    GameManager.game.manageRolingDice[nextDice].gameObject.SetActive(true);
                    GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                    GameManager.game.rolingDice = GameManager.game.manageRolingDice[nextDice];
                    break;
                }
            }
            GameManager.game.canDiceRoll = true;
        }
        else
        {
            if (GameManager.game.selfDice)
            {
                GameManager.game.selfDice = false;
                GameManager.game.canDiceRoll = true;
            }
        }
    }

    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        // Disable all player pieces and dice rolls
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;

        // Additional end game logic like showing a winner screen can go here
    }

    private void LoadWinnerScene(string winner)
    {
        // You can pass the winner information using PlayerPrefs or any other method
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }

    public List<PlayerPieces> GetPlayerPiecesForCurrentDice()
    {
        List<PlayerPieces> playerPieces = new List<PlayerPieces>();

        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public int redpoints = 0;
    public int yellowpoints = 0;
    public int greenpoints = 0;
    public int bluepoints = 0;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    private void Awake()
    {
        game = this;
        ads = GetComponent<AudioSource>();
        rolingDice = manageRolingDice[1];
    }

    private void Start()
    {
        StartCoroutine(BotRoutine());
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }

    public void RollDice()
    {
        numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        Debug.Log("Dice rolled: " + numberofstepstoMove);
    }

    public void UpdatePlayerPoints(int points)
    {
        Debug.Log("Hello");
        // Update points based on the active dice
        if (rolingDice == manageRolingDice[0])
        {
            bluepoints += points;
            Debug.Log("BluePoints: " + bluepoints);
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenpoints += points;
            Debug.Log("GreenPoints: " + greenpoints);
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redpoints += points;
            Debug.Log("RedPoints: " + redpoints);
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowpoints += points;
            Debug.Log("YellowPoints: " + yellowpoints);
        }
    }

    public void RolingDiceManager()
    {
        if (GameManager.game.shouldRollAgain)
        {
            GameManager.game.shouldRollAgain = false;
            GameManager.game.canDiceRoll = true;
            return;
        }

        int nextDice;
        if (GameManager.game.transferDice)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    nextDice = 0;
                    GameManager.game.manageRolingDice[i].gameObject.SetActive(false);
                    GameManager.game.manageRolingDice[nextDice].gameObject.SetActive(true);
                    GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                    GameManager.game.rolingDice = GameManager.game.manageRolingDice[nextDice];
                    break;
                }

                if (i == 0 || i == 1 || i == 2)
                {
                    if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[i])
                    {
                        nextDice = i + 1;
                        GameManager.game.manageRolingDice[i].gameObject.SetActive(false);
                        GameManager.game.manageRolingDice[nextDice].gameObject.SetActive(true);
                        GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                        GameManager.game.rolingDice = GameManager.game.manageRolingDice[nextDice];
                        break;
                    }
                }
            }
            GameManager.game.canDiceRoll = true;
        }
        else
        {
            if (GameManager.game.selfDice)
            {
                GameManager.game.selfDice = false;
                GameManager.game.canDiceRoll = true;
            }
        }
    }

    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        // Disable all player pieces and dice rolls
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;

        // Additional end game logic like showing a winner screen can go here
    }

    private void LoadWinnerScene(string winner)
    {
        // You can pass the winner information using PlayerPrefs or any other method
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }

    public List<PlayerPieces> GetPlayerPiecesForCurrentDice()
    {
        List<PlayerPieces> playerPieces = new List<PlayerPieces>();

        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }

    private IEnumerator BotRoutine()
    {
        while (true)
        {
            //yield return new WaitForSeconds(1f); // Delay to simulate thinking time

            if (rolingDice == manageRolingDice[3] && canDiceRoll) // Check if it's yellow bot's turn
            {
                manageRolingDice[3].RollDiceForBot();
                yield return new WaitForSeconds(1f); // Wait for dice roll to complete

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)]; // Select a random piece to move
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints); // Call movestep instead of MoveSteps
                    yield return new WaitForSeconds(1f); // Wait for the piece to move
                }

                // GameManager.game.RolingDiceManager(); // Manage dice transition
                canDiceRoll = true; // Allow dice roll for the next player
                transferDice = true;
            }

            yield return new WaitForSeconds(1f); // Delay before next action
        }
    }
}
*/







/*
//dice choice
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public int redpoints = 0;
    public int yellowpoints = 0;
    public int greenpoints = 0;
    public int bluepoints = 0;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    public AudioSource centerPathAudioSource;

    // This is the list to store multiple rolled values
    public List<int> rolledValues = new List<int>();
    public DiceSelectionUI diceSelectionUI;

    private void Awake()
    {
        game = this;
        ads = GetComponent<AudioSource>();
        rolingDice = manageRolingDice[0]; // Ensure it starts with the blue player's dice
        UpdateDiceOpacity(); // Update opacity at start
    }

    private void Start()
    {
        StartCoroutine(BotRoutine());
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }

    public void RollDice()
    {
        int rolledValue = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        rolledValues.Add(rolledValue); // Store the rolled value
        Debug.Log("Dice rolled: " + rolledValue);
    }

    public void UpdatePlayerPoints(int points)
    {
        // Update points based on the active dice
        if (rolingDice == manageRolingDice[0])
        {
            bluepoints += points;
            Debug.Log("BluePoints: " + bluepoints);
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenpoints += points;
            Debug.Log("GreenPoints: " + greenpoints);
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redpoints += points;
            Debug.Log("RedPoints: " + redpoints);
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowpoints += points;
            Debug.Log("YellowPoints: " + yellowpoints);
        }
    }

    public void RolingDiceManager()
    {
        if (rolledValues.Count > 1)
        {
            ShowDiceSelectionUI();
        }
        else if (rolledValues.Count == 1)
        {
            numberofstepstoMove = rolledValues[0];
            rolledValues.Clear(); // Clear the list after use
            canPlayermove = true;

            if (numberofstepstoMove == 6)
            {
                canDiceRoll = true;
                selfDice = true;
            }
        }

        if (transferDice)
        {
            // Cycle to the next dice
            int currentIndex = System.Array.IndexOf(manageRolingDice, rolingDice);
            int nextIndex = (currentIndex + 1) % manageRolingDice.Length;

            rolingDice = manageRolingDice[nextIndex];
            Debug.Log("Current Dice Index: " + currentIndex + " | Next Dice Index: " + nextIndex);

            if (rolingDice == manageRolingDice[0])
            {
                Debug.Log("Blue Player's turn");
            }

            UpdateDiceOpacity(); // Update the opacity after changing the dice

            canDiceRoll = true;
            transferDice = false; // Reset transfer flag
        }
        else
        {
            if (selfDice)
            {
                selfDice = false;
                canDiceRoll = true;
            }
        }
    }


    public void ShowDiceSelectionUI()
    {
        diceSelectionUI.ShowDiceOptions(rolledValues);
    }

    public void UseSelectedDiceValue(int selectedValue)
    {
        numberofstepstoMove = selectedValue;
        rolledValues.Remove(selectedValue); // Remove the used value from the list
        RolingDiceManager(); // Continue with the next steps after selection
    }

    private void UpdateDiceOpacity()
    {
        for (int i = 0; i < manageRolingDice.Length; i++)
        {
            // Get the SpriteRenderer for the NumberSpriteHolder
            SpriteRenderer diceRenderer = manageRolingDice[i].transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();

            // Get the SpriteRenderer for the new GameObject (replace "NewGameObjectName" with your new GameObject's name)
            SpriteRenderer newObjectRenderer = manageRolingDice[i].transform.Find("DiceBoard").GetComponent<SpriteRenderer>();

            if (diceRenderer != null)
            {
                Color diceColor = diceRenderer.color;
                Color newObjectColor = newObjectRenderer.color;

                if (manageRolingDice[i] == rolingDice)
                {
                    // Set full opacity for the active dice and the new object
                    diceColor.a = 1f;
                    newObjectColor.a = 1f;
                }
                else
                {
                    // Set reduced opacity for inactive dice and the new object
                    diceColor.a = 0.5f;
                    newObjectColor.a = 0.1f;
                }

                diceRenderer.color = diceColor;
                newObjectRenderer.color = newObjectColor;
            }
            else
            {
                Debug.LogWarning(manageRolingDice[i].name + " does not have a SpriteRenderer on its NumberSpriteHolder or the new GameObject.");
            }
        }
    }

    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        // Disable all player pieces and dice rolls
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;

        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;

        // Additional end game logic like showing a winner screen can go here
    }

    private void LoadWinnerScene(string winner)
    {
        // You can pass the winner information using PlayerPrefs or any other method
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }

    public List<PlayerPieces> GetPlayerPiecesForCurrentDice()
    {
        List<PlayerPieces> playerPieces = new List<PlayerPieces>();

        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }

    private IEnumerator BotRoutine()
    {
        while (true)
        {
            if (rolingDice == manageRolingDice[3] && canDiceRoll) // Check if it's yellow bot's turn
            {
                manageRolingDice[3].RollDiceForBot();
                yield return new WaitForSeconds(1f); // Wait for dice roll to complete

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)]; // Select a random piece to move
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints); // Call movestep instead of MoveSteps
                    yield return new WaitForSeconds(1f); // Wait for the piece to move
                }

                canDiceRoll = true; // Allow dice roll for the next player
                transferDice = true; // Mark for transfer to the next dice
            }

            yield return new WaitForSeconds(1f); // Delay before next action
        }
    }

    public void PlayCenterPathAudio()
    {
        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.Play();
        }
    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public int redpoints = 0;
    public int yellowpoints = 0;
    public int greenpoints = 0;
    public int bluepoints = 0;


    public float rollTime = 10f;  // Time to roll the dice
    public float moveTime = 10f;  // Time to move after rolling the dice
    private float currentRollTime;
    private float currentMoveTime;

    private Coroutine countdownCoroutine;
    private bool hasRolledDice = false;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    public AudioSource centerPathAudioSource; // Add this reference

    public float countdownTime = 10f; // Set the countdown time (10 seconds)
    private float currentCountdown; // To track the remaining time
    public Image blueTimerImage;
    public Image redTimerImage;
    public Image greenTimerImage;
    public Image yellowTimerImage;


    // Declare individual arrows
    public GameObject bluearrow;
    public GameObject redarrow;
    public GameObject greenarrow;
    public GameObject yellowarrow;

    private Text blueScoreText;
    private Text redScoreText;
    private Text greenScoreText;
    private Text yellowScoreText;
    public Text winnerText;
    public GameObject scorePopupPanel;

    public GameObject[] RedLife;
    public Image[] RedheartImages;
    public int RedplayerLives = 3;

    public GameObject[] YellowLife;
    public Image[] YellowheartImages;
    public int YellowplayerLives = 3;

    public GameObject[] BlueLife;
    public Image[] BlueheartImages;
    public int BlueplayerLives = 3;

    public GameObject[] GreenLife;
    public Image[] GreenheartImages;
    public int GreenplayerLives = 3;

    private Text livesText;
    public GameObject LifeOverPopup;
    public Text gameOverText;


    private const string TOGGLE_PREF_KEY = "ButtonTogglerState";


    private void Awake()
    {
        game = this;
        ads = GetComponent<AudioSource>();
        rolingDice = manageRolingDice[0]; // Ensure it starts with the blue player's dice
        UpdateDiceOpacity(); // Update opacity at start
    }

    private void Start()
    {
        StartCoroutine(BotRoutine());
        StartCountdown();
        UpdateArrowVisibility();
        ResizePlayerPieces(); // Ensure initial resizing
        if (!canDiceRoll)
        {
            bluearrow.SetActive(false);
            redarrow.SetActive(false);
            greenarrow.SetActive(false);
            yellowarrow.SetActive(false);
        }

        // Load the saved toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;  // Default to 'on' if no value is saved

        // Update the audio volume based on the toggle state
        SetAudioVolume(isOn);

    }


    // Method to update audio volume based on toggle state
    public void SetAudioVolume(bool isOn)
    {
        float volume = isOn ? 1.0f : 0.0f;

        if (ads != null)
        {
            ads.volume = volume;
        }

        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.volume = volume;
        }
    }

    private void Update()
    {
        if (!canDiceRoll)
        {
            bluearrow.SetActive(false);
            redarrow.SetActive(false);
            greenarrow.SetActive(false);
            yellowarrow.SetActive(false);
        }
    }


    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }

    public void RollDice()
    {
        numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        Debug.Log("Dice rolled: " + numberofstepstoMove);

        // Stop and reset the countdown timer when the dice is rolled
        ResetCountdown();
    }


    public void UpdatePlayerPoints(int points)
    {
        if (rolingDice == manageRolingDice[0])
        {
            bluepoints += points;
            bluepoints = Mathf.Max(0, bluepoints); // Ensure score doesn't go negative
            Debug.Log("BluePoints: " + bluepoints);
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenpoints += points;
            greenpoints = Mathf.Max(0, greenpoints); // Ensure score doesn't go negative
            Debug.Log("GreenPoints: " + greenpoints);
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redpoints += points;
            redpoints = Mathf.Max(0, redpoints); // Ensure score doesn't go negative
            Debug.Log("RedPoints: " + redpoints);
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowpoints += points;
            yellowpoints = Mathf.Max(0, yellowpoints); // Ensure score doesn't go negative
            Debug.Log("YellowPoints: " + yellowpoints);
        }
        ShowWinner();
    }

    //public void ShowScoresInPopup()
    //{
    //    // Set the text components to show the scores
    //    blueScoreText.text = "Blue Points: " + bluepoints.ToString();
    //    greenScoreText.text = "Green Points: " + greenpoints.ToString();
    //    redScoreText.text = "Red Points: " + redpoints.ToString();
    //    yellowScoreText.text = "Yellow Points: " + yellowpoints.ToString();

    //    // Find the winner and show the winner's name and score
    //    string winnerName = GetWinner(out int highestScore);

    //    // Set winner text
    //    winnerText.text = winnerName + " is the winner with " + highestScore.ToString() + " points!";

    //    // Show the popup panel
    //    scorePopupPanel.SetActive(true);
    //}

    //// Helper function to determine the winner
    //private string GetWinner(out int highestScore)
    //{
    //    highestScore = Mathf.Max(bluepoints, greenpoints, redpoints, yellowpoints);

    //    if (highestScore == bluepoints)
    //        return "Blue";
    //    else if (highestScore == greenpoints)
    //        return "Green";
    //    else if (highestScore == redpoints)
    //        return "Red";
    //    else if (highestScore == yellowpoints)
    //        return "Yellow";

    //    return "No winner";  // In case of some error
    //}
    internal void ShowWinner()
    {
        // Find the highest score
        int highestScore = Mathf.Max(bluepoints, greenpoints, redpoints, yellowpoints);
        string winner = "";
        if (highestScore == bluepoints)
            winner = "Blue";
        else if (highestScore == greenpoints)
            winner = "Green";
        else if (highestScore == redpoints)
            winner = "Red";
        else if (highestScore == yellowpoints)
            winner = "Yellow";

        // Display the winner in the console
        Debug.Log("Winner is " + winner + " with " + highestScore + " points!");

        // Update the UI text with the winner's name and score
        winnerText.text = "Winner: " + winner + " with " + highestScore + " points!";
    }

    internal void ShowWinnerPopup()
    {
        scorePopupPanel.SetActive(true);
    }

    private void UpdateDiceOpacity()
    {
        for (int i = 0; i < manageRolingDice.Length; i++)
        {
            SpriteRenderer diceRenderer = manageRolingDice[i].transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
            SpriteRenderer newObjectRenderer = manageRolingDice[i].transform.Find("DiceBoard").GetComponent<SpriteRenderer>();

            if (diceRenderer != null)
            {
                Color diceColor = diceRenderer.color;
                Color newObjectColor = newObjectRenderer.color;

                if (manageRolingDice[i] == rolingDice)
                {
                    diceColor.a = 1f;
                    newObjectColor.a = 1f;
                }
                else
                {
                    diceColor.a = 0.5f;
                    newObjectColor.a = 0.1f;
                }

                diceRenderer.color = diceColor;
                newObjectRenderer.color = newObjectColor;
            }
            else
            {
                Debug.LogWarning(manageRolingDice[i].name + " does not have a SpriteRenderer on its NumberSpriteHolder or the new GameObject.");
            }
        }
    }


    private void UpdateArrowVisibility()
    {
        // Set all arrows inactive initially
        bluearrow.SetActive(false);
        redarrow.SetActive(false);
        greenarrow.SetActive(false);
        yellowarrow.SetActive(false);

        // Activate the corresponding arrow based on the active dice
        if (rolingDice == manageRolingDice[0]) // Blue player's turn
        {
            bluearrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[1]) // Red player's turn
        {
            redarrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[2]) // Green player's turn
        {
            greenarrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[3]) // Yellow player's turn
        {
            yellowarrow.SetActive(true);
        }
    }

    public void RolingDiceManager()
    {
        // Check if vibration is enabled based on the stored toggle preference
        const string TOGGLE_PREF_KEY = "ButtonTogglerState";  // Same key used in ButtonSpriteToggler
        bool isVibrationEnabled = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1; // Default to 'on'

        if (shouldRollAgain)
        {
            shouldRollAgain = false;
            canDiceRoll = true;
            return;
        }

        if (transferDice)
        {
            int currentIndex = System.Array.IndexOf(manageRolingDice, rolingDice);
            int nextIndex = (currentIndex + 1) % manageRolingDice.Length;

            rolingDice = manageRolingDice[nextIndex];
            Debug.Log("Current Dice Index: " + currentIndex + " | Next Dice Index: " + nextIndex);

            if (isVibrationEnabled)
            {
                //Handheld.Vibrate();  // Only vibrate if the toggle is on
            }

            UpdateDiceOpacity();
            UpdateArrowVisibility();
            ResizePlayerPieces(); // Call to resize pieces

            canDiceRoll = true;
            transferDice = false;

            StartCountdown();
        }
        else if (selfDice)
        {
            selfDice = false;
            canDiceRoll = true;
            StartCountdown();
            UpdateArrowVisibility();
            ResizePlayerPieces(); // Call to resize pieces
        }
    }




    public void StartCountdown()
    {
        ResetCountdown();

        blueTimerImage.gameObject.SetActive(false);
        redTimerImage.gameObject.SetActive(false);
        greenTimerImage.gameObject.SetActive(false);
        yellowTimerImage.gameObject.SetActive(false);

        if (rolingDice == manageRolingDice[0])
        {
            blueTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(blueTimerImage));
        }
        else if (rolingDice == manageRolingDice[1])
        {
            redTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(redTimerImage));
        }
        else if (rolingDice == manageRolingDice[2])
        {
            greenTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(greenTimerImage));
        }
        else if (rolingDice == manageRolingDice[3])
        {
            yellowTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(yellowTimerImage));
        }
    }



    public void ResetCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }


    private IEnumerator CountdownCoroutine(Image currentTimerImage)
    {
        float startTime = Time.time;
        float endTime = startTime + countdownTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            currentTimerImage.fillAmount = timeRemaining / countdownTime;

            yield return null;
        }

        currentTimerImage.fillAmount = 0;
        currentTimerImage.gameObject.SetActive(false);

        if (rolingDice == manageRolingDice[0])
        {
            HandleTurnTimeout("Blue");  // Red player's time is up
        }
        else if (rolingDice == manageRolingDice[1])
        {
            HandleTurnTimeout("Red");  // Yellow player's time is up
        }
        else if (rolingDice == manageRolingDice[2])
        {
            HandleTurnTimeout("Green");  // Yellow player's time is up
        }
        else if (rolingDice == manageRolingDice[3])
        {
            HandleTurnTimeout("Yellow");  // Yellow player's time is up
        }
    }


    private void HandleTurnTimeout(string playerColor)
    {
        Debug.Log(playerColor + "'s time is up!");
        LoseLife(playerColor);  // Lose life for the respective player
        transferDice = true;    // Transfer the dice to the next player
        canDiceRoll = false;    // Stop the dice roll for the current player
        RolingDiceManager();    // Manage the dice transfer and turn change
    }

    private void LoseLife(string playerColor)
    {
        if (playerColor == "Red" && RedplayerLives > 0)
        {
            RedplayerLives--;
            UpdateLivesUI("Red");
        }
        else if (playerColor == "Blue" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Blue");
        }
        else if (playerColor == "Green" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Green");
        }
        else if (playerColor == "Yellow" && YellowplayerLives > 0)
        {
            YellowplayerLives--;
            UpdateLivesUI("Yellow");
        }

        // If a player runs out of lives, handle game over
        if (RedplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (BlueplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (GreenplayerLives <= 0)
        {
            HandleGameOver();
        }
        else if (YellowplayerLives <= 0)
        {
            HandleGameOver();
        }
    }

    private void UpdateLivesUI(string playerColor)
    {
        if (playerColor == "Red")
        {
            for (int i = 0; i < RedheartImages.Length; i++)
            {
                RedheartImages[i].enabled = i < RedplayerLives;
            }
        }
        else if (playerColor == "Blue")
        {
            for (int i = 0; i < BlueheartImages.Length; i++)
            {
                BlueheartImages[i].enabled = i < BlueplayerLives;
            }
        }
        else if (playerColor == "Green")
        {
            for (int i = 0; i < GreenheartImages.Length; i++)
            {
                GreenheartImages[i].enabled = i < GreenplayerLives;
            }
        }
        else if (playerColor == "Yellow")
        {
            for (int i = 0; i < YellowheartImages.Length; i++)
            {
                YellowheartImages[i].enabled = i < YellowplayerLives;
            }
        }
    }

    private void HandleGameOver()
    {
        if (gameOverText != null)
        {
            LifeOverPopup.SetActive(true);  // Show the Game Over popup
            gameOverText.text = "You have missed the maximum number of turns!";  // Set the Game Over message
        }

        // Stop player movement
        canPlayermove = false;

        // Stop dice rolling
        canDiceRoll = false;

        // Disable all player pieces to stop any further interactions
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        // Disable all dice
        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        // Optionally, stop any other gameplay routines, like bot behavior or countdowns
        StopAllCoroutines();  // This stops all ongoing coroutines, including bot routines or timers

        // If you want to freeze the game completely, you can also pause the game time
        Time.timeScale = 0;  // Pauses the game completely
    }


    public void OnContinueBtn()
    {
        SceneManager.LoadScene("Home");
    }


    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;
    }

    private void LoadWinnerScene(string winner)
    {
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }

    public List<PlayerPieces> GetPlayerPiecesForCurrentDice()
    {
        List<PlayerPieces> playerPieces = new List<PlayerPieces>();

        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }

    private IEnumerator BotRoutine()
    {
        while (true)
        {
            if (rolingDice == manageRolingDice[3] && canDiceRoll)
            {
                //Handheld.Vibrate();
                manageRolingDice[3].RollDiceForBot();
                yield return new WaitForSeconds(1f);

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)];
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints);
                    yield return new WaitForSeconds(1f);
                }

                canDiceRoll = true;
                transferDice = true;
            }

            if (rolingDice == manageRolingDice[0] && canDiceRoll)
            {
                //Handheld.Vibrate();
                manageRolingDice[0].RollDiceForBot();
                yield return new WaitForSeconds(1f);

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)];
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints);
                    yield return new WaitForSeconds(1f);
                }

                canDiceRoll = true;
                transferDice = true;
            }

            if (rolingDice == manageRolingDice[2] && canDiceRoll)
            {
                //Handheld.Vibrate();
                manageRolingDice[2].RollDiceForBot();
                yield return new WaitForSeconds(1f);

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)];
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints);
                    yield return new WaitForSeconds(1f);
                }

                canDiceRoll = true;
                transferDice = true;
            }

            yield return new WaitForSeconds(1f);
        }

    }

    void ResizePlayerPieces()
    {
        // Get the active color
        string activeColor = "";
        if (rolingDice == manageRolingDice[0]) activeColor = "Blue";
        else if (rolingDice == manageRolingDice[1]) activeColor = "Red";
        else if (rolingDice == manageRolingDice[2]) activeColor = "Green";
        else if (rolingDice == manageRolingDice[3]) activeColor = "Yellow";

        // Resize pieces based on the active color
        foreach (string color in new[] { "Blue", "Red", "Green", "Yellow" })
        {
            float scale;
            float height;
            float depth;

            GameObject[] pieces = GameObject.FindGameObjectsWithTag(color);

            foreach (GameObject piece in pieces)
            {
                PlayerPieces playerPiece = piece.GetComponent<PlayerPieces>();

                // Determine if the piece is in a safe zone
                bool isInSafeZone = playerPiece != null && playerPiece.IsInSafeZone;

                // If the piece is in a safe zone, make it smaller
                if (isInSafeZone)
                {
                    scale = 20f;   // Smaller scale when in the safe zone
                    height = 20f;
                    depth = 2f;
                }
                else
                {
                    // If the piece is not in the safe zone, use the standard scaling
                    scale = color == activeColor ? 30f : 20f;
                    height = color == activeColor ? 30f : 20f;
                    depth = color == activeColor ? 3f : 1.5f;
                }

                // Resize the piece
                piece.transform.localScale = new Vector3(scale, height, depth);

                // Update sorting order based on the y-position to prevent hiding
                SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // The higher the y-position, the lower the sorting order
                    spriteRenderer.sortingOrder = Mathf.RoundToInt(-piece.transform.position.y * 100);
                    // Increase sorting order if the piece matches the active color
                    if (color == activeColor)
                    {
                        spriteRenderer.sortingOrder += 10;
                    }
                }
            }
        }
    }




    public void PlayCenterPathAudio()
    {
        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.Play();
        }
    }
}
