using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankAddRemove : AddRemove {
	public PlayerAddRemove playerAddRemove;
	public SteableKind steableKind;

    // Update is called once per frame
    public override void Awake()
	{
        base.Awake();
        Debug.Log("BANK AR text:"+ display.text);
        //Update min and max so that the number of card the player wants does not exceed the number of card the bank has
        CardInventory bankInv = CardManager.getInstance ().getCardInventory ();
		CardInventory playerInv = TurnManager.getInstance().getMainPlayer().getCardInventory();
		int n = bankInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };
	}

	public override void Increase ()
	{
		base.Increase ();
		if (playerAddRemove.GetValue() > 0) {
			playerAddRemove.value = 0;
			playerAddRemove.SetValue();
		}
    }
}
