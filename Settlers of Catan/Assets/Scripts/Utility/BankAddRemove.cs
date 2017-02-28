using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankAddRemove : AddRemove {
	public PlayerAddRemove playerAddRemove;
	public SteableKind steableKind;


	public override void  Awake ()
	{
		base.Awake();
		this.incrementFactor = 1;
	}

	// Update is called once per frame
    void Update()
	{
        //Update min and max so that the number of card the player wants does not exceed the number of card the bank has
        CardInventory bankInv = CardManager.getInstance ().getCardInventory ();
		CardInventory playerInv = TurnManager.getInstance().getMainPlayer().getCardInventory();
		int n = bankInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };

	}

	public override void Increase ()
	{
		Debug.Log("called");
		base.Increase ();
		if (playerAddRemove.GetValue() > 0) {
			playerAddRemove.value = 0;
			playerAddRemove.SetValue();
		}
    }
}
