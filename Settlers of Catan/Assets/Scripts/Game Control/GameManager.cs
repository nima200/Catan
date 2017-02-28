using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // has a getter, and is a singleton.
    private static GameManager Instance { get;  set; } // reconsidering might be redudant. 

    // These are only to allow the list that is being inherited from the lobbymanager to be converted into a inumberable type.
    private List<LobbyPlayer> intermiedateList;
    private IEnumerable<LobbyPlayer> IEnumberableLobbyplayerList;
    // -----------------------------------------------------------------//

    // ---Managers --//
    public LobbyManager _lobbyManager;
    public LobbyPlayerList _lobbyplayerlist;
    public CardManager _cardManager;
    public TradeManager _tradeManager;
    public CardMenuManager _cardmenumanager;


   //playerlist being used to make lobbyplayerlist a inumberable non synced list.

    private IEnumerable<LobbyPlayer> _playerList;

    // Use this for initialization
    // change this by giving it the playerlist

    public PlayerManager _playerManager;


    // awake for singleton. 
    void Awake()
    {
        if (Instance == null) {
            //if not, set instance to this
            Instance = this;
        }

        //If instance already exists and it's not this:
        else if (Instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        //Creating new Managers.
        Instantiate(_playerManager);
        Instantiate(_cardManager);
        Instantiate(_tradeManager);
        Instantiate(_cardmenumanager);

        // finding lobbymanager and playerlist from assets that weren't destroyed on scene change.
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _lobbyplayerlist= FindObjectOfType<LobbyPlayerList>();
        intermiedateList = _lobbyplayerlist.PlayerList;
        IEnumberableLobbyplayerList = intermiedateList;

        // IEnumberableLobbyPlayerlist is the list of lobbyplayers that were available before scene change. It allows iteration as an IEnumberable type. 

    }


    public void rollDice() {
        DiceRoll.getInstance().RollTrigger();
        int[] currentIntRoll = DiceRoll.getInstance().getIntRoll();
        EventDie currentEventRoll = DiceRoll.getInstance().getEventRoll();

        //TODO : Broadcast roll to all players
        //      communicate with map, give resources
        //      
    }

    // Update is called once per frame
    public static GameManager getInstance() {
        return Instance;
    }

    public Player getCurrent() {
        return TurnManager.getInstance().getCurrentPlayer();
    }

    // Getter for gamemanager used by savegame.
    public static GameManager getCurrent()
    {
        return Instance;
    }

}
