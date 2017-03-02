using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TradeManager : MonoBehaviour {

	private static TradeManager instance = null;
	public SpecialHarbour specialHarbourPrefab;
	public GenericHarbour genericHarbourPrefab;

    public BankMenu bankMenuPrefab;
    private BankMenu bankMenu;

    public Canvas userInterface;
    public GameObject bankHarBox;
    public GameObject playerBox;

    public BankAddRemove[] bankAddRemove;
    public PlayerAddRemove[] playerAddRemove;

    CardInventory bankInv;
	CardInventory playerInv;
	Player mainPlayer;

	//Make Trade Manager Singleton
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}
		DontDestroyOnLoad (gameObject);
	}

    // Initiazed harbours
    void Start()
    {

        bankInv = CardManager.getInstance().getCardInventory();
        mainPlayer = TurnManager.getInstance().getMainPlayer();
        if (mainPlayer != null)
        {
			Debug.Log("there is a main player :" + mainPlayer);
            playerInv = mainPlayer.getCardInventory();
        }


        GameObject harbours = new GameObject("Harbours");
        GameObject specialHarbours = new GameObject("Special Harbours");
        GameObject genericHarbours = new GameObject("Generic Harbours");
        specialHarbours.transform.parent = harbours.transform;
        genericHarbours.transform.parent = harbours.transform;
        for (int i = 0; i < 5; i++)
        {
            SpecialHarbour harbour = Instantiate(specialHarbourPrefab);
            harbour.steableKind = (SteableKind)i;
            harbour.name = "Harbour " + harbour.steableKind;
            harbour.transform.parent = specialHarbours.transform;
        }
        for (int i = 0; i < 4; i++)
        {
            GenericHarbour harbour = Instantiate(genericHarbourPrefab);
            harbour.name = "Harbour " + i;
            harbour.transform.parent = genericHarbours.transform;
        }
    }


    // attached to "Bank trade"
    // 2 functions :
    //  (1) initiate trade session : call CreateBankInstance
    //  (2) show trade menu again if it got hidden
    public void ShowBankInstance()
    {
        if (!bankMenu)
        {
            CreateBankInstance();
            Debug.Log("Bank trade session instance created.");
        }
        else bankMenu.gameObject.SetActive(true);
    }


    public void BankTrade ()
	{
		//Check whether the trade is initiated by the current player
		if (mainPlayer != TurnManager.getInstance ().getCurrentPlayer ()) {
			return;
		}
		Dictionary<SteableKind,  int> cardsToTake = new Dictionary<SteableKind,  int> ();
		Dictionary<SteableKind,  int> cardsToGive = new Dictionary<SteableKind,  int> ();
		int numWanted = 0;
		foreach (BankAddRemove counter in bankAddRemove) {
			if (counter.value > 0) {
				//Check if bank has enough resourec cards to trade
				if (bankInv.countSteableCard (counter.steableKind) >= counter.value) {
					cardsToTake [counter.steableKind] = counter.value;
					numWanted = numWanted + counter.value;
				} else {
					return;
				}
			}
		}
		int numTakable = 0;
		foreach (PlayerAddRemove counter in playerAddRemove) {
			if (counter.value > 0) {
				//Check if the player has enough resourec cards to trade
				if (playerInv.countSteableCard (counter.steableKind) >= counter.value) {
					cardsToGive [counter.steableKind] = counter.value;
					numTakable = numTakable + counter.value / counter.incrementFactor;
				} else {
					return;
				}
			}
		}
		if (numWanted != numTakable) {
            Debug.Log("Insufficient resources");
			return;
		}
		foreach (SteableKind r in cardsToTake.Keys) {
			CardManager.getInstance ().distributeSteable (mainPlayer, r, cardsToTake [r]);
            Debug.Log("Took " + cardsToTake[r] + " " + r.ToString());
		}
		foreach (SteableKind r in cardsToGive.Keys) {
			CardManager.getInstance().takeSteable(mainPlayer, r, cardsToGive[r]);
            Debug.Log("Gave " + cardsToGive[r] + " " + r.ToString());
		}
		resetCounter();
//		UICountManager.getInstance().UpdateIndicators();
	}



	void resetCounter()
	{
		foreach (AddRemove counter in bankAddRemove) {
			counter.value=0;
			counter.SetValue();
		}
		foreach (AddRemove counter in playerAddRemove) {
			counter.value=0;
			counter.SetValue();
		}
	}

    // 
    void CreateBankInstance() {

        bankMenu = Instantiate(bankMenuPrefab, userInterface.transform);
        bankMenu.gameObject.SetActive(true);

        bankHarBox = bankMenu.gameObject.GetComponentInChildren<BankUI>().gameObject;
        playerBox = bankMenu.gameObject.GetComponentInChildren<PlayerUI>().gameObject;

        bankAddRemove = bankHarBox.GetComponentsInChildren<BankAddRemove>();
        playerAddRemove = playerBox.GetComponentsInChildren<PlayerAddRemove>();
        resetCounter();

    }

    public void DestroyBankInstance()
    {
        Debug.Log("Bank trade session instance destroyed.");
        Destroy(bankMenu.gameObject);
    }

    public void HideBankInstance() {
        Debug.Log("Bank trade session instance hidden.");
        bankMenu.gameObject.SetActive(false);
    }


    public static TradeManager getInstance()
    {
        return instance;
    }

}
