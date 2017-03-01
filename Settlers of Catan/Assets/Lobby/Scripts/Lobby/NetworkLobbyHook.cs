using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// this is an extension of the lobbyhook to be able to allow transfer of data.

public class NetworkLobbyHook : LobbyHook
{

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
        //-------------------Changes---------------//
        LobbyPlayer lobbyPlayerObject = lobbyPlayer.GetComponent<LobbyPlayer>();
        Player gamePlayerObject = gamePlayer.GetComponent<Player>();

        gamePlayerObject.playerName = lobbyPlayerObject.playerName;


        //----------------end--------------------//


    }
}
