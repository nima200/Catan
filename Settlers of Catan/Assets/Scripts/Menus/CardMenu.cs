using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenu : MonoBehaviour {

    public GameObject resourceCommodityDecks;
    public GameObject progressCardDecks;
    public bool resourceCommodityShown;                 // resourceCommodityShown : TRUE == resourceCommodityDecks is shown
                                                        //                          FALSE == progressCardDecks is shown

    void Start() {
        resourceCommodityShown = true;
    }

    public void Shift() {
        if(resourceCommodityShown) {
            ShowProgressCard();
            resourceCommodityShown = false;
        }
        else {
            ShowResourceCommodity();
            resourceCommodityShown = true;
        }
        Debug.Log("WOHOO ya choose meh !");
    }

    void ShowResourceCommodity() {
        resourceCommodityDecks.SetActive(true);
        progressCardDecks.SetActive(false);
		
	}
	
	void ShowProgressCard() {
        resourceCommodityDecks.SetActive(false);
        progressCardDecks.SetActive(true);

    }
}
