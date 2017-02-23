using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountManager : MonoBehaviour {

    public GameObject CardMenu;
	Player mainPlayer;
	CardInventory cardInventory;
	public CounterDummy[] counters;

	// Use this for initialization
	void Start ()
	{
		mainPlayer = TurnManager.getInstance ().getMainPlayer();
		cardInventory = mainPlayer.getCardInventory ();
        counters = CardMenu.GetComponentsInChildren<CounterDummy>();
        UpdateIndicators();
        // TODO : add eventlistener to trigger update of the display (eg, trade, rollDice, ...)

	}
	
	// Update is called once per frame
	public void UpdateIndicators()
	{
		for (int i = 0; i < counters.Length; i++) {
			SteableKind myKind = counters[i].steableKind;
            //Debug.Log("(1) " + counters[i].gameObject.GetComponentInChildren<Text>().text);
            //Debug.Log("(2) " + cardInventory.countSteableCard(myKind).ToString());

			counters[i].gameObject.GetComponentInChildren<Text>().text = cardInventory.countSteableCard(myKind).ToString();
	    }

	}
}
