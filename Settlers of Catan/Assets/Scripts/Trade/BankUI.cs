using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour {

	public CountDisp[] countDisplays;

	// Use this for initialization
	void Start ()
	{
		countDisplays = gameObject.GetComponentsInChildren<CountDisp> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < countDisplays.Length; i++) {
			string s = countDisplays[i].gameObject.GetComponentInChildren<Text>().text;
//			print(s);
     }
		
	}
}
