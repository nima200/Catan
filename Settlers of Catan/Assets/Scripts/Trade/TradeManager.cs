using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour {

	private static TradeManager instance = null;
	public SpecialHarbour specialHarbourPrefab;
	public GenericHarbour genericHarbourPrefab;
	public CountDisp[] BankCountDisplay;
	public CountDisp[] PlayerCountDisplay;

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
		
		foreach (CountDisp counter in BankCountDisplay) {
			Debug.Log (counter.name);
			Debug.Log ("value is:"+counter.GetValue ());
		}
	}

	public static TradeManager getInstance ()
	{
		return instance;
	}

	// Initiazed harbours
	void Start ()
	{
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

	}

	public bool canDoMaritimeTrade (Player player, SteableKind give, SteableKind take, int ratio)
	{
		CardInventory playerInv = player.getCardInventory ();
		CardInventory bankInv = CardManager.getInstance ().getCardInventory();
		//Check if there are enough resourec cards to trade
		if (playerInv.countSteableCard (give) < 4) {
			return false;
		}
		if (bankInv.countSteableCard (take) < 1) {
			return false;
		}
		//Check whether the trade is initiated by the current player
		if (player != PlayerManager.getInstance ().getCurrentPlayer ()) {
			return false;
		}
		//Check whether give and take are of the same resource kind
		if (give == take) {
			return false;
		}
		return true;
	}

	public void doMaritimeTrade (Player player, SteableKind give, SteableKind take)
	{
		int ratio = getMaritimTradeRatio (player, give, take);
		if (canDoMaritimeTrade (player, give, take, ratio)) {
			CardManager.getInstance().takeSteable(player, give, 4);
			CardManager.getInstance().distributeSteable(player, take, 1);
		}
	}

	public int getMaritimTradeRatio (Player player, SteableKind give, SteableKind take)
	{	
		int ratio = 4;
		List<Harbour> harbours = player.getHarbours ();
		for (int i = 0; i < harbours.Count; i++) {
			if (harbours [i].GetType () == typeof(GenericHarbour)) {
				ratio = 3;
			}
			if (harbours [i].GetType () == typeof(SpecialHarbour) && ((SpecialHarbour)harbours [i]).steableKind == give) {
				return 2;
			}
		}
		return ratio;
	}


	
	// Update is called once per frame
	void Update ()
	{
		CardInventory bankInventory = CardManager.getInstance ().getCardInventory ();
		Player player = PlayerManager.getInstance ().getCurrentPlayer ();
		CardInventory playerInventory = player.getCardInventory ();
		foreach (CountDisp counter in BankCountDisplay) {
			Debug.Log (counter.name);
			Debug.Log ("value is:" + counter.GetValue ());
			int n = bankInventory.countSteableCard (counter.steableKind);
			counter.minMax = new int[2] { 0, n };
		}
		foreach (CountDisp counter in PlayerCountDisplay) {
			int n = playerInventory.countSteableCard (counter.steableKind);
			counter.minMax = new int[2] { 0, n };
		}
	}




}
