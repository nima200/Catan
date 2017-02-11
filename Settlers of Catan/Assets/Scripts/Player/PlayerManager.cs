using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

	private static PlayerManager instance = null;
	public Player playerPrefrab;
	public CardInventory cardInventoryPrefab;
	private List<Player> players = new List<Player> ();

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
	}

	public static PlayerManager getInstance()
	{
		return instance;
	}

	void Start ()
	{
		createPlayer ();
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

	public Player getCurrentPlayer ()
	{
		return players[0];
	}

	void Update ()
	{
	
	}
}
