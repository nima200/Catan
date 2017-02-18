using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour {

	public BankCountDisp[] countDisplays;

	// Use this for initialization
	void Start ()
	{
		countDisplays = gameObject.GetComponentsInChildren<BankCountDisp> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < countDisplays.Length; i++) {
			string s = countDisplays[i].gameObject.GetComponentInChildren<Text>().text;
//			print(s);
     }
		
	}
}
