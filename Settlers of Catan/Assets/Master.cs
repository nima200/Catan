using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

    public Servant serv;

	// Use this for initialization
	void Start () {
        Debug.Log("Master is awake, and i shall summon my servant.");
        serv.gameObject.SetActive(true);
        Debug.Log("My Duty is over");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
