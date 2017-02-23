using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAddRemove : AddRemove {
	public BankAddRemove bankAddRemove;
	public SteableKind steableKind;

	// Update is called once per frame
	//public override void Awake ()
    void Update()
    {
        //base.Awake();
        //Debug.Log("PLAYER AR"+display.text);
		Player mainPlayer = TurnManager.getInstance().getMainPlayer();
		//Update min and max so that the number of card the player is able to trade does not exceed the number of card the player has
		CardInventory bankInv = CardManager.getInstance ().getCardInventory ();
		CardInventory playerInv = mainPlayer.getCardInventory();
		int n = playerInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };
		//Update best trade ratio
		incrementFactor = mainPlayer.getMaritimTradeRatio(steableKind);
	}


	public override void Increase ()
	{
		base.Increase ();
		if (bankAddRemove.GetValue() > 0) {
			bankAddRemove.value = 0;
			bankAddRemove.SetValue();
		}
    }
}
