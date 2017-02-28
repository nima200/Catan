using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrade : MonoBehaviour {

    
    public GameObject tradeMenu;
    public GameObject mainMenu;
    public bool isMainPlayer; //TODO : ref to curPlayer
                              // for now, click on TRADE button to reopen ongoing trade
                              //TODO : make tab for ongoing trade (where?)
                              //TODO : implement StartTurn method in all scripts ?


    public void OpenTrade() {
        tradeMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseTrade() {
        tradeMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void HideTrade() {
        tradeMenu.SetActive(false);
        mainMenu.SetActive(false);
    }
}

