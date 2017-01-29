using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public string playerName;
    public int playerID;
    public string greeting;
    public CardInventory cardInventory = new CardInventory();
    

    public Player()
    {
        this.greeting = "hello!!";
        print (this.greeting);
    }
	
}
