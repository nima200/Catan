using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : MonoBehaviour
{

	private static PlayerManager instance = null;
    public GameObject Players;
	public Player playerPrefrab;
	public CardInventory cardInventoryPrefab;
	public List<Player> myPlayers = new List<Player> ();
    private int nbOfPlayers;
    private int pointer;

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
        nbOfPlayers = 4;                    // Set to 4 for now
        createPlayer ();
        pointer = 0;                       
	}

	public static PlayerManager getInstance()
	{
		return instance;
	}
    

	private void createPlayer ()
	{
        Player player;
        int offset = 100;
		for (int i = 0; i < nbOfPlayers; i++) {
			player = Instantiate (playerPrefrab, Players.transform);
            player.transform.position = new Vector3(200+(offset*i),200,0);
			player.Initialize(i,cardInventoryPrefab);
            bool e = player.enabled;
			myPlayers.Add(player);
		}
	}

    

    public Player getPlayer (int index)
	{
		return myPlayers[index];
	}

    public int getNbOfPlayer() {
        return nbOfPlayers;
    }

    public Player getNextPlayer()
    {
        pointer = (pointer + 1) % nbOfPlayers;
        return getPlayer(pointer);
    }
}
