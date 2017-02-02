using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public string playerName;
	public int playerID;
	public CardInventory cardInventory;
	
    public Player()
    {
		
    }

	public void instantiate (int i, CardInventory cardInventoryPrefab)
	{
		name = "Player" + i;
		playerID = i;
		playerName = "Player" + i;
		cardInventory = Instantiate(cardInventoryPrefab);
		cardInventory.transform.parent = this.transform;
	}

	public CardInventory getCardInventory()
	{
		return cardInventory;
	}
	
}
