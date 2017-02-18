using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankCountDisp : CountDisp {
	public PlayerCountDisp playerCountDisplay;
	public SteableKind steableKind;

	// Update is called once per frame
	void Update ()
	{
		//Update min and max so that the number of card the player wants does not exceed the number of card the bank has
		CardInventory bankInv = CardManager.getInstance ().getCardInventory ();
		CardInventory playerInv = PlayerManager.getInstance().getMainPlayer().getCardInventory();
		int n = bankInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };
	}

	public override void Increase ()
	{
		base.Increase ();
		if (playerCountDisplay.GetValue() > 0) {
			playerCountDisplay.value = 0;
			playerCountDisplay.SetValue();
		}
    }
}
