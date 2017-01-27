using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    

	void Start () {
        PlayerMaker("player1", 1111, GameObject.Find("Players"));
        PlayerMaker("player2", 1112, GameObject.Find("Players"));
        PlayerMaker("player3", 1113, GameObject.Find("Players"));
        PlayerMaker("player4", 1114, GameObject.Find("Players"));
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
	
	void Update () {
	
	}
}
