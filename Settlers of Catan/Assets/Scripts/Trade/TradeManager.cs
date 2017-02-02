using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour {

	private static TradeManager instance = null;

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

	public static TradeManager getInstance ()
	{
		return instance;
	}

	public bool canTradeWithBank (Player player, ResourceKind give, ResourceKind take)
	{
		CardInventory playerInv = player.getCardInventory ();
		CardInventory bankInv = CardManager.getInstance ().getCardInventory();
		//Check if there are enough resourec cards to trade
		if (playerInv.countReourceCard (give) < 4) {
			return false;
		}
		if (bankInv.countReourceCard (take) < 1) {
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

	//Trade with the bank at ratio 4:1
	public void tradeWithBank (Player player, ResourceKind give, ResourceKind take)
	{
		CardManager.getInstance().takeResource(player, take, 4);
		CardManager.getInstance().distributeResource(player, give, 1);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




}
