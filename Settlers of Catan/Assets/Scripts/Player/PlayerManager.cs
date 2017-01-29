using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

	public static PlayerManager instance = null;
	public Player playerPrefrab;
	public List<Player> players = new List<Player> ();


	void Awake ()
	{
//		//Check if instance already exists
//		if (instance == null) {
//			instance = this;
//		}
//		else if (instance != this) {
//			Destroy (gameObject);    
//		}
//             
//		//Sets this to not be destroyed when reloading scene
//		DontDestroyOnLoad (gameObject);
	}

	void Start ()
	{
		createPlayer ();
	}

	void createPlayer ()
	{
		for (int i = 0; i < 4; i++) {
			Player player = Instantiate (playerPrefrab);
			player.name = "Player" + i;
			player.playerID = i;
			player.playerName = "Player" + i;
			player.transform.parent = GameObject.Find ("Players").transform;
			players.Add (player);
		}
	}


	void Update ()
	{
	
	}
}
