using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    private static TurnManager instance = null;
    public GameObject UtilityMenu;
    public GameObject DiceMenu;
    private Player mainPlayer;                            // --> the player running this instance of the game
    private Player currentPlayer;                         // --> the player currently PLAYING

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainPlayer = PlayerManager.getInstance().getPlayer(0);      // Player 0 is the one playing on this instance of the game for now
        currentPlayer = PlayerManager.getInstance().getPlayer(0);
        DontDestroyOnLoad(gameObject);
    }

    public static TurnManager getInstance()
    {
        return instance;
    }

    public void NextTurn()
    {
        Player previousCurrentPlayer = mainPlayer;
        Debug.Log("Previous  player : " + previousCurrentPlayer.getPlayerID());
        setCurrentToNextPlayer();
        Debug.Log("New current player : " + currentPlayer.getPlayerID());
        if (currentPlayer.equals(mainPlayer))
        {
            Debug.Log("ACTIVATED: I am the new current player");
        }
        else if (previousCurrentPlayer.equals(mainPlayer))
        {
            Debug.Log("DEACTIVATED: I am the previous player");
        }
        else Debug.Log("UNCHANGED.");
    }

    public Player getMainPlayer()
    {
        return mainPlayer;
    }

    public Player getCurrentPlayer()
    {
        return currentPlayer;
    }

    public void setCurrentToNextPlayer()
    {
        currentPlayer = PlayerManager.getInstance().getNextPlayer();
    }
}
