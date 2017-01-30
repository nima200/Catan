using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

	public static PlayerManager instance = null;
	public Player playerPrefrab;
	public CardInventory cardInventoryPrefab;
	public List<Player> players = new List<Player> ();


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

	void Start ()
	{
		createPlayer ();
	}

	private void createPlayer ()
	{
		for (int i = 0; i < 4; i++) {
			Player player = Instantiate (playerPrefrab);
			player.name = "Player" + i;
			player.playerID = i;
			player.playerName = "Player" + i;
			player.cardInventory = Instantiate(cardInventoryPrefab);
			player.cardInventory.transform.parent = player.transform;
			player.transform.parent = GameObject.Find ("Players").transform;
			players.Add (player);
		}
	}

	public Player getPlayer (int index)
	{
		return players[index];
	}

	void Update ()
	{
	
	}
}
