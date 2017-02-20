using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

	private static PlayerManager instance = null;
	public Player playerPrefrab;
	public CardInventory cardInventoryPrefab;
	private List<Player> players = new List<Player> ();
    private int mainPlayerIndex;                            // --> the player running this instance of the game
    private int currentPlayerIndex;                         // --> the player currently PLAYING
    private int nbOfPlayer;

	//Make Player Manager Singleton
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}
		DontDestroyOnLoad (gameObject);
		createPlayer ();
        currentPlayerIndex = 0;
        nbOfPlayer = 4;                         // Set to 4 for now
	}

	public static PlayerManager getInstance()
	{
		return instance;
	}
    

	private void createPlayer ()
	{
		for (int i = 0; i < 4; i++) {
			Player player = Instantiate (playerPrefrab);
			player.instantiate(i,cardInventoryPrefab);
			player.transform.parent = GameObject.Find ("Players").transform;
			players.Add (player);
		}
	}

	public Player getPlayer (int index)
	{
		return players[index];
	}

	public Player getMainPlayer ()
	{
		return players[mainPlayerIndex];
	}

	public Player getCurrentPlayer ()
	{
		return players[currentPlayerIndex];
	}

    public void setCurrentToNextPlayer() {
        currentPlayerIndex = (currentPlayerIndex + 1) % nbOfPlayer;
    }
}
