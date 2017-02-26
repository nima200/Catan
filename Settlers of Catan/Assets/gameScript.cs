using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class gameScript : MonoBehaviour
{

    // Network player is a datascructure for holding networking stuff stuff.
    // Guid of a networkplayer gets you the id 

    public Text Buttontext;

    public LobbyManager List;
    public LobbyPlayerList l;
    private List<LobbyPlayer> le;
    private IEnumerable<LobbyPlayer> playerList;
    private Player p1; Player p2 ; Player p3; Player p4;
    public CanvasGroup mainCanvas;

    public Button b1;
    void Awake()
    {
        List = FindObjectOfType<LobbyManager>();
        l = FindObjectOfType<LobbyPlayerList>();
        le = l.PlayerList;
        playerList = le;
        b1 = GetComponent<Button>();
    }

    // need this to be in playerManager
    // migrate all the players made new to this.
    // intializes the both.
    void migrateData(Player player1,Player player2, Player player3, Player player4)
    {
        p1 = player1;
        p2 = player2;
        p3 = player3;
        p4 = player4;
    }

    // method that would set the name of players as the anmes getting from playerlist. 
    //could be done within the method above^
    // would preferably do it in dot. 
    // dot with be renamed to ListtoGameMigration.
    // will be used at the start of the game. 
    // this will interate through all and give them the names from the playerlist.
    //playerpoint is the player avaiable in list.
    public void gameMigration()
    {
        foreach(LobbyPlayer playerPoint in playerList) {
            Debug.Log((playerPoint.playerName).ToString());

        }  
   //     Debug.Log((playerList.FirstOrDefault().playerName).ToString());
   //   p1.playerName = playerList.FirstOrDefault().playerName;
    }

    // end turn gives ui control to the next player in the system. 
    public void endTurn(Player player)
    {
        player.isTurn = false;
        // then goes to the next player. 
    }

    // disables game object and hides them. 
    // use canvas object later afterwords. 
    public void disableTurn()
    {
        if (b1.enabled)
        {
            b1.enabled = false;
            b1.gameObject.SetActive(false);
            Debug.Log(enabled.ToString());
        }
    }
}























    /**
	// Use this for initialization
	void Start () {
        Buttontext = GetComponentInChildren<Text>();
        P = GetComponentInChildren<myplayer>();
        String name = "hello";
        allPlayers.Add(name);
        List.PlayerListModified  
    }
	
	// Update is called once per frame
	public void showID  () {
        IEnumerator enumerator = allPlayers.GetEnumerator();

        Buttontext.text = enumerator.Current.ToString();

        print(Buttontext.text);
        // slot and number are going to correspond
       // byte turn = lobbyPlayer.slot;
        // or you could go to lobbyplayerlist and getLobbyplayerlist make the function . 
    }
}


// have a shared array for all of the 
// synclist to add names to the game on start

    **/
       