using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAddRemove : AddRemove {
	public BankAddRemove bankAddRemove;
	public SteableKind steableKind;
	private Player mainPlayer;
	private CardInventory bankInv;
	private CardInventory playerInv;

	public override void  Awake ()
	{
		base.Awake();
		this.incrementFactor = 4;
		mainPlayer = TurnManager.getInstance().getMainPlayer();
		bankInv = CardManager.getInstance ().getCardInventory ();
		playerInv = mainPlayer.getCardInventory();
	}

	public override void Increase ()
	{
		//Update best trade ratio
		incrementFactor = mainPlayer.getMaritimTradeRatio(steableKind);
		//Update min and max so that the number of card the player is able to trade does not exceed the number of card the player has
		int n = playerInv.countSteableCard (steableKind);
		minMax = new int[2] { 0, n };
		base.Increase ();
		if (bankAddRemove.GetValue() > 0) {
			bankAddRemove.value = 0;
			bankAddRemove.SetValue();
		}
    }

    public override void Decrease ()
	{
		//Update best trade ratio
		incrementFactor = mainPlayer.getMaritimTradeRatio(steableKind);
		base.Decrease();
	}
}
