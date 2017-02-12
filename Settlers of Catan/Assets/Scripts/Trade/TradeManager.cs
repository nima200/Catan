using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TradeManager : MonoBehaviour {

	private static TradeManager instance = null;
	public SpecialHarbour specialHarbourPrefab;
	public GenericHarbour genericHarbourPrefab;
	public CountDisp[] BankCountDisplay;
	public CountDisp[] PlayerCountDisplay;
	CardInventory bankInv;
	CardInventory playerInv;
	Player mainPlayer;

	//Make Trade Manager Singleton
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}
		DontDestroyOnLoad (gameObject);
	}

	public void BankTrade ()
	{
		//Check whether the trade is initiated by the current player
		if (mainPlayer != PlayerManager.getInstance ().getCurrentPlayer ()) {
			return;
		}
		Dictionary<SteableKind,  int> cardsToTake = new Dictionary<SteableKind,  int> ();
		Dictionary<SteableKind,  int> cardsToGive = new Dictionary<SteableKind,  int> ();
		int numWanted = 0;
		foreach (CountDisp counter in BankCountDisplay) {
			if (counter.value > 0) {
				//Check if bank has enough resourec cards to trade
				if (bankInv.countSteableCard (counter.steableKind) >= counter.value) {
					cardsToTake [counter.steableKind] = counter.value;
					numWanted = numWanted + counter.value;
				} else {
					return;
				}
			}
		}
		int numTakable = 0;
		foreach (CountDisp counter in PlayerCountDisplay) {
			if (counter.value > 0) {
				//Check if the player has enough resourec cards to trade
				if (playerInv.countSteableCard (counter.steableKind) >= counter.value) {
					cardsToGive [counter.steableKind] = counter.value;
					numTakable = numTakable + counter.value / counter.incrementFactor;
				} else {
					return;
				}
			}
		}
		if (numWanted != numTakable) {
			return;
		}
		foreach (SteableKind r in cardsToTake.Keys) {
			CardManager.getInstance ().distributeSteable (mainPlayer, r, cardsToTake [r]);
		}
		foreach (SteableKind r in cardsToGive.Keys) {
			CardManager.getInstance().takeSteable(mainPlayer, r, cardsToGive[r]);
		}
		resetCounter();
	}

	public static TradeManager getInstance ()
	{
		return instance;
	}


	void resetCounter()
	{
		foreach (CountDisp counter in BankCountDisplay) {
			counter.value=0;
			counter.UpdateVal();
		}
		foreach (CountDisp counter in PlayerCountDisplay) {
			counter.value=0;
			counter.UpdateVal();
		}
	}

	// Initiazed harbours
	void Start ()
	{
		
		bankInv = CardManager.getInstance ().getCardInventory();
		mainPlayer = PlayerManager.getInstance ().getMainPlayer ();
		playerInv = mainPlayer.getCardInventory ();

		GameObject harbours = new GameObject ("Harbours");
		GameObject specialHarbours = new GameObject ("Special Harbours");
		GameObject genericHarbours = new GameObject ("Generic Harbours");
		specialHarbours.transform.parent = harbours.transform;
		genericHarbours.transform.parent = harbours.transform;
		for (int i = 0; i < 5; i++) {
			SpecialHarbour harbour = Instantiate (specialHarbourPrefab);
			harbour.steableKind = (SteableKind)i;
			harbour.name = "Habour " + harbour.steableKind;
			harbour.transform.parent = specialHarbours.transform;
		}	
		for (int i = 0; i < 4; i++) {
			GenericHarbour harbour = Instantiate (genericHarbourPrefab);
			harbour.name = "Habour " + i;
			harbour.transform.parent = genericHarbours.transform;
		}

		GameObject bankHarBox = GameObject.Find ("BankHarb box");
		BankCountDisplay = bankHarBox.GetComponentsInChildren<CountDisp> ();
		GameObject playerBox = GameObject.Find ("Player box");
		PlayerCountDisplay = playerBox.GetComponentsInChildren<CountDisp> ();
		resetCounter();
	}

	
	// Update is called once per frame
	void Update ()
	{
		bankInv = CardManager.getInstance ().getCardInventory();
		for (int i = 0; i < BankCountDisplay.Length; i++) {
			CountDisp counter = BankCountDisplay [i];
			//Give and take cannot be of the same resource kind
			if (counter.value > 0) {
				PlayerCountDisplay[i].value = 0;
				PlayerCountDisplay[i].UpdateVal();
			}
			int n = bankInv.countSteableCard (counter.steableKind);
			counter.minMax = new int[2] { 0, n };
		}

		for (int i = 0; i < PlayerCountDisplay.Length; i++) {
			CountDisp counter = PlayerCountDisplay [i];
			//Give and take cannot be of the same resource kind
			if (counter.value > 0) {
				BankCountDisplay[i].value = 0;
				BankCountDisplay[i].UpdateVal();
			}
			int n = playerInv.countSteableCard (counter.steableKind);
			counter.minMax = new int[2] { 0, n };
			counter.incrementFactor = mainPlayer.getMaritimTradeRatio(counter.steableKind);
		}
	}




}
