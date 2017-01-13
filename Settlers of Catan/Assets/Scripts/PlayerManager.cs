using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
        GameObject players = GameObject.Find("Players");
        PlayerMaker("player1", 1111, players);
        PlayerMaker("player2", 1112, players);
        PlayerMaker("player3", 1113, players);
        PlayerMaker("player4", 1114, players);
    }

    void PlayerMaker(string name, int ID, GameObject parent)
    {
        GameObject player = new GameObject("player" + ID);
        player.AddComponent<Player>();
        player.GetComponent<Player>().playerID = ID;
        player.GetComponent<Player>().playerName = name;
        setParent(player, parent);
    }

    void setParent(GameObject child, GameObject parent)
    {
        child.transform.parent = parent.transform;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
