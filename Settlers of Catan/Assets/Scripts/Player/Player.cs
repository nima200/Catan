﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    [SyncVar]
    public string playerName = "";

	public int playerID;
  //  private bool isTurn;

	public CardInventory cardInventory;
	public List<Harbour> myHarbour = new List<Harbour>();
	public int ratio;
    public bool hasAlchemist;                             // TODO : find it in inventory
    
    public Player()
    {
        //name = "Player" + i;
        //playerID = i;
        PlayerName = "Player " + PlayerName;
        //isTurn = false;
        MyTurnPhase = TurnPhase.WaitForTurn;
        CardInventory = Instantiate(cardInventoryPrefab);
        CardInventory.gameObject.SetActive(true);
        CardInventory.transform.parent = this.transform;
    }


	public void Initialize (int i, CardInventory cardInventoryPrefab)
	{
		//name = "Player" + i;
		//playerID = i;
		playerName = "Player " + playerName;
        //isTurn = false;
		cardInventory = Instantiate(cardInventoryPrefab);
        cardInventory.gameObject.SetActive(true);
		cardInventory.transform.parent = this.transform;
	}

	public CardInventory getCardInventory()
	{
		return cardInventory;
	}

	//The player needs to find a way to determine which harbours he has access to
	public List<Harbour> getHarbours ()
	{
		return myHarbour;
	}

	public int getMaritimTradeRatio (SteableKind resource)
	{	
		ratio = 4;
		for (int i = 0; i < myHarbour.Count; i++) {
			if (myHarbour [i].GetType () == typeof(GenericHarbour)) {
				ratio = 3;
			}
			if (myHarbour [i].GetType () == typeof(SpecialHarbour) && ((SpecialHarbour)myHarbour [i]).steableKind == resource) {
				return 2;
			}
		}
		return ratio;
	}

    public bool equals(Player otherPlayer) {
        if (playerID == otherPlayer.getPlayerID()) return true;
        else return false;
    }

    public int getPlayerID() {
        return playerID;
    }

    public bool HasAlchemist() {
        return hasAlchemist;                        // TODO : find alchemist in inventory
    }

/*    public bool CheckIsTurn() {
        return isTurn;
    }

    public void SetIsTurn(bool b) {
        isTurn = b; 
    } */


    void Awake()
    {
           PlayerManager.getInstance().AddtoList(this);
    }

    /*private void Awake()
    {
        PlayerManager.getInstance().AddtoList(this);
        Debug.Log(playerName);
    }*/

    // for debuging only.
    public void Update()
    {
    }

    public void NextPhase() {
        switch (MyTurnPhase) {
            case TurnPhase.Sandbox1:
                MyTurnPhase = TurnPhase.Sandbox2;
                break;
            case TurnPhase.Sandbox2:
                MyTurnPhase = TurnPhase.WaitForTurn;
                break;
        }
        Debug.Log("New phase of " + PlayerName + ": " + MyTurnPhase.ToString());
    }

    
    
}
