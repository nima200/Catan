using Prototype.NetworkLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class gameScript : MonoBehaviour {

    // Network player is a datascructure for holding networking stuff stuff.
    // Guid of a networkplayer gets you the id 
    public Text Buttontext;

    private NetworkPlayer players;
    private NetworkLobbyPlayer lobbyPlayer;
    private LobbyPlayerList l;

	// Use this for initialization
	void Start () {
        Buttontext = GetComponentInChildren<Text>();
        l = GetComponent<LobbyPlayerList>();
        print("Player ID is " + Network.player.ToString()); 
    }
	
	// Update is called once per frame
	public void showID  () {
        Buttontext.text = Network.player.ToString();

        print(Buttontext.text);
        // slot and number are going to correspond
        byte turn = lobbyPlayer.slot;
        // or you could go to lobbyplayerlist and getLobbyplayerlist make the function . 
    }
}
