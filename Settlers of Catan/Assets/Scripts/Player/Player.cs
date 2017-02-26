using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public string playerName;
	public int playerID;
    public bool isTurn;
	public CardInventory cardInventory;
	public List<Harbour> myHarbour = new List<Harbour>();

    // Khalil - Added attribute "isTurn" as a boolean to control UI. 
    public Player()
    {
		
    }

	public void instantiate (int i, CardInventory cardInventoryPrefab)
	{
		name = "Player" + i;
		playerID = i;
		playerName = "Player" + i;
        isTurn = false;
		cardInventory = Instantiate(cardInventoryPrefab);
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
	

}
