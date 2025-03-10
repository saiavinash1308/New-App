//Last updated code
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer rolingdiceanimation;
    [SerializeField] int numberGot;

    Coroutine generateRandomNumberonDice;
    // public bool canDiceRoll = true;
    public int outpieces;

    public void OnMouseDown()
    {
        generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            GameManager.game.UpdatePlayerPoints(numberGot);

            // Automatically set the number of steps to move in the GameManager
            GameManager.game.numberofstepstoMove = numberGot;
            GameManager.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[0]) { outpieces = GameManager.game.blueOutPlayers; }
            else if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[1]) { outpieces = GameManager.game.greenOutPlayers; }
            else if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[2]) { outpieces = GameManager.game.redOutPlayers; }
            else if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[3]) { outpieces = GameManager.game.yellowOutPlayers; }

            if (GameManager.game.numberofstepstoMove != 6 && outpieces == 0)
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.canPlayermove = true;
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }

    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
*/











/*
//audio
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer rolingdiceanimation;
    [SerializeField] int numberGot;

    Coroutine generateRandomNumberonDice;
    // public bool canDiceRoll = true;
    public int outpieces;
    public DiceAudio diceSound;


    public void OnMouseDown()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    public void RollDiceForBot()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            GameManager.game.UpdatePlayerPoints(numberGot);

            // Automatically set the number of steps to move in the GameManager
            GameManager.game.numberofstepstoMove = numberGot;
            GameManager.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.canPlayermove = true;
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }

    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
*/





/*
//final
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;  // Array to hold the sprites for each dice face (1 to 6)
    [SerializeField] SpriteRenderer numberSpriteHolder;  // The SpriteRenderer that will display the dice face
    [SerializeField] SpriteRenderer rolingdiceanimation;  // SpriteRenderer for the rolling animation
    [SerializeField] int numberGot;  // The result of the dice roll

    Coroutine generateRandomNumberonDice;
    public int outpieces;
    public DiceAudio diceSound;

    private void Awake()
    {
        // Find and assign the SpriteRenderer on the NumberSpriteHolder child object
        numberSpriteHolder = transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    public void RollDiceForBot()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            GameManager.game.UpdatePlayerPoints(numberGot);

            // Automatically set the number of steps to move in the GameManager
            GameManager.game.numberofstepstoMove = numberGot;
            GameManager.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.canPlayermove = true;
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }

    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
*/




/*
// dice choice
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;  // Array to hold the sprites for each dice face (1 to 6)
    [SerializeField] SpriteRenderer numberSpriteHolder;  // The SpriteRenderer that will display the dice face
    [SerializeField] SpriteRenderer rolingdiceanimation;  // SpriteRenderer for the rolling animation
    [SerializeField] int numberGot;  // The result of the dice roll

    Coroutine generateRandomNumberonDice;
    public int outpieces;
    public DiceAudio diceSound;

    private void Awake()
    {
        // Find and assign the SpriteRenderer on the NumberSpriteHolder child object
        numberSpriteHolder = transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    public void RollDiceForBot()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;

            GameManager.game.rolledValues.Add(numberGot); // Store the rolled value
            GameManager.game.UpdatePlayerPoints(numberGot);

            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (numberGot == 6)
            {
                // Allow the player to roll the dice again
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = true;
            }
            else if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.rolledValues.Clear(); // Clear the list if no move is possible
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.RolingDiceManager(); // Call RolingDiceManager to handle multiple values
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }


    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
*/







/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;  // Array to hold the sprites for each dice face (1 to 6)
    [SerializeField] SpriteRenderer numberSpriteHolder;  // The SpriteRenderer that will display the dice face
    [SerializeField] SpriteRenderer rolingdiceanimation;  // SpriteRenderer for the rolling animation
    [SerializeField] int numberGot;  // The result of the dice roll

    Coroutine generateRandomNumberonDice;
    public int outpieces;
    public DiceAudio diceSound;

    private void Awake()
    {
        // Find and assign the SpriteRenderer on the NumberSpriteHolder child object
        numberSpriteHolder = transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            GameManager.game.ResetCountdown(); // Stop the countdown when the player rolls the dice
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    public void RollDiceForBot()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            GameManager.game.UpdatePlayerPoints(numberGot);

            // Automatically set the number of steps to move in the GameManager
            GameManager.game.numberofstepstoMove = numberGot;
            GameManager.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0; // Reset numberofstepstoMove on dice transfer
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.canPlayermove = true;
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }

    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer rolingdiceanimation;
    [SerializeField] internal int numberGot;

    Coroutine generateRandomNumberonDice;
    public int outpieces;
    public DiceAudio diceSound;

    private void Awake()
    {
        numberSpriteHolder = transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
{
    if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
    {
        //GameManager.game.ResetCountdown();
        generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        //GameManager.game.StartCountdown();
        }
}


    public void RollDiceForBot()
    {
        if (GameManager.game.canDiceRoll && GameManager.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    IEnumerator RollingDiceCoroutine()
    {
        GameManager.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.game.canDiceRoll)
        {
            GameManager.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;
            GameManager.game.UpdatePlayerPoints(numberGot);

            GameManager.game.numberofstepstoMove = numberGot;
            GameManager.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            if (!CanPlayerMove() && AllPlayersCloseToCenterPath())
            {
                GameManager.game.canDiceRoll = true;
                GameManager.game.selfDice = false;
                GameManager.game.transferDice = true;
                GameManager.game.numberofstepstoMove = 0;
                yield return new WaitForSeconds(0.5f);
                GameManager.game.RolingDiceManager();
            }
            else
            {
                GameManager.game.canPlayermove = true;
            }

            if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }

    bool CanPlayerMove()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }


    bool AllPlayersCloseToCenterPath()
    {
        GameManager gm = GameManager.game;
        List<PlayerPieces> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PlayerPieces piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
