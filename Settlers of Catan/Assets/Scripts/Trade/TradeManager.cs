using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TradeManager : MonoBehaviour {

	private static TradeManager instance = null;
	public SpecialHarbour specialHarbourPrefab;
	public GenericHarbour genericHarbourPrefab;
    public GameObject bankHarBox;
    public GameObject playerBox;
    public BankCountDisp[] BankCountDisplay;
	public PlayerCountDisp[] PlayerCountDisplay;
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
		bankInv = CardManager.getInstance ().getCardInventory();
		//Check whether the trade is initiated by the current player
		if (mainPlayer != PlayerManager.getInstance ().getCurrentPlayer ()) {
			return;
		}
		Dictionary<SteableKind,  int> cardsToTake = new Dictionary<SteableKind,  int> ();
		Dictionary<SteableKind,  int> cardsToGive = new Dictionary<SteableKind,  int> ();
		int numWanted = 0;
		foreach (BankCountDisp counter in BankCountDisplay) {
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
		foreach (PlayerCountDisp counter in PlayerCountDisplay) {
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
            Debug.Log("Insufficient resources");
			return;
		}
		foreach (SteableKind r in cardsToTake.Keys) {
			CardManager.getInstance ().distributeSteable (mainPlayer, r, cardsToTake [r]);
            Debug.Log("Took " + cardsToTake[r] + " " + r.ToString());
		}
		foreach (SteableKind r in cardsToGive.Keys) {
			CardManager.getInstance().takeSteable(mainPlayer, r, cardsToGive[r]);
            Debug.Log("Gave " + cardsToGive[r] + " " + r.ToString());
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
			counter.SetValue();
		}
		foreach (CountDisp counter in PlayerCountDisplay) {
			counter.value=0;
			counter.SetValue();
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

		BankCountDisplay = bankHarBox.GetComponentsInChildren<BankCountDisp> ();
		PlayerCountDisplay = playerBox.GetComponentsInChildren<PlayerCountDisp> ();
		resetCounter();
	}

	



}
