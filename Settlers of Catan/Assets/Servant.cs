using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : MonoBehaviour {

    public Servant serv2;
    public int turn;
	// Use this for initialization
	void Awake () {
        Debug.Log("Mi servant is Awake");
        if (turn == 1)
        {
            serv2.gameObject.SetActive(true);
            Debug.Log("I woke up my own servant");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
