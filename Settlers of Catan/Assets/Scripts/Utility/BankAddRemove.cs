using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankAddRemove : AddRemove {
	public PlayerAddRemove playerAddRemove;
	public SteableKind steableKind;
	private CardInventory bankInv;


	public override void  Awake ()
	{
		base.Awake();
		this.incrementFactor = 1;
		bankInv = CardManager.getInstance ().getCardInventory ();
	}

	public override void Increase ()
	{
		//Update min and max so that the number of card the player wants does not exceed the number of card the bank has
		int n = bankInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };
		base.Increase ();
		if (playerAddRemove.GetValue() > 0) {
			playerAddRemove.value = 0;
			playerAddRemove.SetValue();
		}
    }
}
