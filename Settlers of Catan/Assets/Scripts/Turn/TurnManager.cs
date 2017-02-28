using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    private static TurnManager instance = null;
    public GameObject UtilityMenu;
    public GameObject DiceMenu;
    private Player localPlayer;                            // --> the player running this instance of the game
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

        DontDestroyOnLoad(gameObject);
    }

    public void SetFirstPlayer()
    {
        localPlayer = PlayerManager.getInstance().getLocalPlayer();
        currentPlayer = PlayerManager.getInstance().getPlayer(0);
        //currentPlayer.SetIsTurn(true);
    }

    public static TurnManager getInstance()
    {

        return instance;
    }

    public void NextTurn()
    {

        // TODO : Broadcast over network
        DiceRoll.getInstance().ResetDice();
        Player previousCurrentPlayer = currentPlayer;                                   // TODO : assigning players? 
        Debug.Log("Previous  player : " + previousCurrentPlayer.getPlayerID());
        setCurrentToNextPlayer();
        Debug.Log("New current player : " + currentPlayer.getPlayerID());
        if (currentPlayer.isLocalPlayer)
        {
            Selectable[] allUtilitySelectable = UtilityMenu.GetComponentsInChildren<Selectable>();
            foreach (Selectable s in allUtilitySelectable)
            {
                s.interactable = true;
            }
            Button[] allDiceButtons = DiceMenu.GetComponentsInChildren<Button>();
            foreach (Button b in allDiceButtons)
            {
                b.interactable = true;
            }
            Debug.Log("ACTIVATED: I am the new current player");
        }
        else if (previousCurrentPlayer.isLocalPlayer)
        {
            Selectable[] allUtilitySelectable = UtilityMenu.GetComponentsInChildren<Selectable>();
            foreach (Selectable s in allUtilitySelectable)
            {
                s.interactable = false;
            }
            Button[] allDiceButtons = DiceMenu.GetComponentsInChildren<Button>();
            foreach (Button b in allDiceButtons)
            {
                b.interactable = false;
            }
            Debug.Log("DEACTIVATED: I am the previous player");
        }
        else Debug.Log("UNCHANGED.");
    }

    public Player getMainPlayer()
    {
        return localPlayer;
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
