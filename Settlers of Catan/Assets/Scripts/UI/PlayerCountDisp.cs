using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountDisp : CountDisp {
	public BankCountDisp bankCountDisplay;
	public SteableKind steableKind;

	// Update is called once per frame
	void Update ()
	{
		Player mainPlayer = PlayerManager.getInstance().getMainPlayer();
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
		if (bankCountDisplay.GetValue() > 0) {
			bankCountDisplay.value = 0;
			bankCountDisplay.SetValue();
		}
    }
}
