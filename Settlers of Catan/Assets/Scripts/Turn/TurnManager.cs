using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    private static TurnManager instance = null;
    public GameObject UtilityMenu;
    public GameObject DiceMenu;

    // Use this for initialization
    void Awake () {
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

    public static TurnManager getInstance()
    {
        return instance;
    }

    public void NextTurn() {
        Player previousMainPlayer = PlayerManager.getInstance().getMainPlayer();
        Debug.Log("Previous main player : " + previousMainPlayer.getPlayerID());
        PlayerManager.getInstance().setCurrentToNextPlayer();
        Player currentPlayer = PlayerManager.getInstance().getCurrentPlayer();
        Player newMainPlayer = PlayerManager.getInstance().getMainPlayer();
        Debug.Log("New main player : " + newMainPlayer.getPlayerID());
        if (currentPlayer.equals(newMainPlayer))
        {
            Debug.Log("I am the new main player");
        }
        else if (currentPlayer.equals(previousMainPlayer))
        {
            Debug.Log("I am the previous main player");
        }
    }
}
