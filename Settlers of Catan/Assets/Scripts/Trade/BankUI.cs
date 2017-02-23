using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour {

	public BankAddRemove[] AddRemovelays;

	// Use this for initialization
	void Start ()
	{
		AddRemovelays = gameObject.GetComponentsInChildren<BankAddRemove> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < AddRemovelays.Length; i++) {
			string s = AddRemovelays[i].gameObject.GetComponentInChildren<Text>().text;
//			print(s);
     }
		
	}
}
