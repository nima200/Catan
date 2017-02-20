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

    public CountDisp[] BankCountDisplay;
	public CountDisp[] PlayerCountDisplay;

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

	public void BankTrade ()
	{
		//Check whether the trade is initiated by the current player
		if (mainPlayer != PlayerManager.getInstance ().getCurrentPlayer ()) {
			return;
		}
		Dictionary<SteableKind,  int> cardsToTake = new Dictionary<SteableKind,  int> ();
		Dictionary<SteableKind,  int> cardsToGive = new Dictionary<SteableKind,  int> ();
		int numWanted = 0;
		foreach (CountDisp counter in BankCountDisplay) {
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
		foreach (CountDisp counter in PlayerCountDisplay) {
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
	}

	public static TradeManager getInstance ()
	{
		return instance;
	}


	void resetCounter()
	{
		foreach (CountDisp counter in BankCountDisplay) {
			counter.value=0;
			counter.SetValue();
		}
		foreach (CountDisp counter in PlayerCountDisplay) {
			counter.value=0;
			counter.SetValue();
		}
	}

	// Initiazed harbours
	void Start ()
	{

		bankInv = CardManager.getInstance ().getCardInventory();
		mainPlayer = PlayerManager.getInstance ().getMainPlayer ();
		playerInv = mainPlayer.getCardInventory ();

		GameObject harbours = new GameObject ("Harbours");
		GameObject specialHarbours = new GameObject ("Special Harbours");
		GameObject genericHarbours = new GameObject ("Generic Harbours");
		specialHarbours.transform.parent = harbours.transform;
		genericHarbours.transform.parent = harbours.transform;
		for (int i = 0; i < 5; i++) {
			SpecialHarbour harbour = Instantiate (specialHarbourPrefab);
			harbour.steableKind = (SteableKind)i;
			harbour.name = "Harbour " + harbour.steableKind;
			harbour.transform.parent = specialHarbours.transform;
		}	
		for (int i = 0; i < 4; i++) {
			GenericHarbour harbour = Instantiate (genericHarbourPrefab);
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


    // 
    void CreateBankInstance() {

        bankMenu = Instantiate(bankMenuPrefab, bankMenuPrefab.transform);
        bankMenu.gameObject.transform.SetParent(userInterface.transform);
        bankMenu.gameObject.SetActive(true);

        bankHarBox = bankMenu.gameObject.GetComponentInChildren<BankUI>().gameObject;
        playerBox = bankMenu.gameObject.GetComponentInChildren<PlayerUI>().gameObject;

        BankCountDisplay = bankHarBox.GetComponentsInChildren<CountDisp>();
        PlayerCountDisplay = playerBox.GetComponentsInChildren<CountDisp>();
        resetCounter();

        bankInv = CardManager.getInstance().getCardInventory();

        foreach (CountDisp cd in BankCountDisplay)
        {
            Button[] allButtons = cd.gameObject.GetComponentsInChildren<Button>();
            foreach (Button b in allButtons) {
                Debug.Log("Bank button found");
                b.onClick.AddListener(UpdateBound);
            }
        }
        foreach (CountDisp cd in PlayerCountDisplay)
        {
            Button[] allButtons = cd.gameObject.GetComponentsInChildren<Button>();
            foreach (Button b in allButtons)
            {
                Debug.Log("Player button found");
                b.onClick.AddListener(UpdateBound);
            }
        }
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
	

    //TODO : create another method that is called at any click (eventListener ?)


	// Update is called once per frame (ie 30 times per sec!!!!!!!!)
	void UpdateBound ()
	{
        Debug.Log("Oui you called me ?");
        for (int i = 0; i < BankCountDisplay.Length; i++)
        {
            CountDisp counter = BankCountDisplay[i];
            //Give and take cannot be of the same resource kind
            if (counter.value > 0)
            {
                PlayerCountDisplay[i].value = 0;
                PlayerCountDisplay[i].SetValue();
            }
            int n = bankInv.countSteableCard(counter.steableKind);
            counter.minMax = new int[2] { 0, n };
        }

        for (int i = 0; i < PlayerCountDisplay.Length; i++)
        {
            CountDisp counter = PlayerCountDisplay[i];
            //Give and take cannot be of the same resource kind
            if (counter.value > 0)
            {
                BankCountDisplay[i].value = 0;
                BankCountDisplay[i].SetValue();
            }
            int n = playerInv.countSteableCard(counter.steableKind);
            counter.minMax = new int[2] { 0, n };
            counter.incrementFactor = mainPlayer.getMaritimTradeRatio(counter.steableKind);
        }
    }




}
