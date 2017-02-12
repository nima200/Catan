using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankHarbTrade : MonoBehaviour
{

    public GameObject lights;
    public GameObject bankHarbMenu;
    public GameObject mainMenu;
    public int ratio;         // TODO : ref to player's inventory + ratio
                              // for now, click on TRADE button to reopen ongoing trade
                              //TODO : make tab for ongoing trade (where?)


    public void OpenBankHarb()
    {
    	Time.timeScale = 0;
        bankHarbMenu.SetActive(true);
        mainMenu.SetActive(false);
        lights.SetActive(false);
    }

    public void CloseBankHarb()
    {
        bankHarbMenu.SetActive(false);
        mainMenu.SetActive(true);
        lights.SetActive(true);
    }

    public void HideBankHarb()  //TODO
    {
        bankHarbMenu.SetActive(false);
        mainMenu.SetActive(true);
        lights.SetActive(true);
    }
}

