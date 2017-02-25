using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public string playerName;
	public int playerID;                                   // TODO : make it private
	public CardInventory cardInventory;
	public List<Harbour> myHarbour = new List<Harbour>();
	public int ratio;
    public bool hasAlchemist;                             // TODO : find it in inventory

    public Player()
    {
		
    }

	public void Initialize (int i, CardInventory cardInventoryPrefab)
	{
		name = "Player" + i;
		playerID = i;
		playerName = "Player" + i;
		cardInventory = Instantiate(cardInventoryPrefab);
        cardInventory.gameObject.SetActive(true);
		cardInventory.transform.parent = this.transform;
	}

	public CardInventory getCardInventory()
	{
		return cardInventory;
	}

	//The player needs to find a way to determine which harbours he has access to
	public List<Harbour> getHarbours ()
	{
		return myHarbour;
	}

	public int getMaritimTradeRatio (SteableKind resource)
	{	
		ratio = 4;
		for (int i = 0; i < myHarbour.Count; i++) {
			if (myHarbour [i].GetType () == typeof(GenericHarbour)) {
				ratio = 3;
			}
			if (myHarbour [i].GetType () == typeof(SpecialHarbour) && ((SpecialHarbour)myHarbour [i]).steableKind == resource) {
				return 2;
			}
		}
		return ratio;
	}

    public bool equals(Player otherPlayer) {
        if (playerID == otherPlayer.getPlayerID()) return true;
        else return false;
    }

    public int getPlayerID() {
        return playerID;
    }

    public bool HasAlchemist() {
        return hasAlchemist;                        // TODO : find alchemist in inventory
    }
}
