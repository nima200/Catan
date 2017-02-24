using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountManager : MonoBehaviour {

	Player currentPlayer;
	CardInventory cardInventory;
	public CounterDummy[] counters;

	// Use this for initialization
	void Start ()
	{
		currentPlayer = PlayerManager.getInstance ().getMainPlayer();
		cardInventory = currentPlayer.getCardInventory ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameObject steableChips = GameObject.Find ("Resource Commodities");
		if (steableChips != null) {
			counters = steableChips.GetComponentsInChildren<CounterDummy> ();
			for (int i = 0; i < counters.Length; i++) {
				SteableKind myKind = counters[i].steableKind;
				counters[i].gameObject.GetComponent<Text>().text = cardInventory.countSteableCard(myKind).ToString();
	     }
		}

	}
}
