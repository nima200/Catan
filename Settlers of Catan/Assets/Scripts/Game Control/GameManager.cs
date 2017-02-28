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
    private static GameManager Instance { get;  set; }
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
    }

	
	// Update is called once per frame
	void Update () {
		
	}

}
