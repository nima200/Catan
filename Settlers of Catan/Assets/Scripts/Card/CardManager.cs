using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour
{
    public CardInventory cardInventoryPrefab;
	public ResourceCard resourceCardPrefab;
	public CommodityCard commodityCardPrefab;
	public ProgressCard progressCardPrefab;
	private static CardManager instance = null;
	private CardInventory cardInventory;
	int RESOURCE_KIND_TOTAL = 6;
	int RESOURCE_CARD_NUM = 19;
	int COMMODITY_CARD_NUM = 12;
	int COMMODITY_KIND_TOTAL = 3;

	//Make Card Manager Singleton
	void Awake ()
	{
        Debug.Log("Card manager created");
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}
		DontDestroyOnLoad (gameObject);
	}

	public static CardManager getInstance ()
	{
		return instance;
	}

	// Use this for initialization
	void Start ()
	{
		cardInventory = Instantiate (cardInventoryPrefab);
		cardInventory.transform.parent = gameObject.transform;
		GameObject cards = new GameObject ("Cards");
		GameObject resourceCards = new GameObject ("Resource Cards");
		resourceCards.transform.parent = cards.transform;
		GameObject commodityCards = new GameObject ("Commodity Cards");
		commodityCards.transform.parent = cards.transform;
		GameObject progressCards = new GameObject ("Progress Cards");
		progressCards.transform.parent = cards.transform;
		createStealableCard ();
		createProgressCard ();
        Debug.Log("Number of Player registered by Player Manager:" + PlayerManager.getInstance().myPlayers.Count);
		for (int i = 0; i < PlayerManager.getInstance().getNbOfPlayer(); i++) {                //WARNING : EXCEPTION 
            //Debug.Log("What is the index "+ i);
			distributeSteable(PlayerManager.getInstance().getPlayer(i), SteableKind.GOLD,2);
		}

		distributeSteable(TurnManager.getInstance().getCurrentPlayer(), SteableKind.ORE, 3);
		distributeSteable(TurnManager.getInstance().getCurrentPlayer(), SteableKind.CLOTH, 6);
		distributeSteable(TurnManager.getInstance().getCurrentPlayer(), SteableKind.LUMBER, 9);
//		UICountManager.getInstance().UpdateIndicators();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	//Create 95 resource cards and 36 commodity cards
	private void createStealableCard ()
	{
		
		for (int i = 0; i < RESOURCE_KIND_TOTAL; i++) {
			for (int j = 1; j <= RESOURCE_CARD_NUM; j++) {
				int id = i * RESOURCE_CARD_NUM + j;
				string stringID = id.ToString ();
				ResourceCard card = Instantiate(resourceCardPrefab);
				card.name = "Resource Card " + id;
				card.steableKind = (SteableKind)i;
				card.id = stringID;
                card.transform.parent = GameObject.Find("Resource Cards").transform;
				cardInventory.addSteableCard((SteableKind)i,card);
			}
		}
		for (int i = 0; i < COMMODITY_KIND_TOTAL; i++) {
			for (int j = 1; j <= COMMODITY_CARD_NUM; j++) {
				int id = i * COMMODITY_CARD_NUM + j;
				string stringID = id.ToString ();
				CommodityCard card = Instantiate(commodityCardPrefab);
				card.name = "Commodity Card " + id;
				card.steableKind = (SteableKind)(i+RESOURCE_KIND_TOTAL);
				card.id = stringID;
                card.transform.parent = GameObject.Find("Commodity Cards").transform;
				cardInventory.addSteableCard((SteableKind)(i+RESOURCE_KIND_TOTAL), card);
			}
		}
	}

	//Create a deck of progress cards in random order
	private void createProgressCard ()
	{
		for (int i = 0; i < 3; i++) {
			for (int j = 1; j < 19; j++) {
				int id = i * 18 + j;
				string stringID = id.ToString ();
				ProgressCard card = Instantiate(progressCardPrefab);
				card.name = "progressCard" + id;
				card.progressCardKind = (ProgressCardKind)i;
				card.id = stringID;
                card.transform.parent = GameObject.Find("Progress Cards").transform;
				cardInventory.addProgressCard(card);
			}
		}
		Shuffle(cardInventory.getProgressCards());
	}

	//Shuffle the deck of progress cards
	private void Shuffle<ProgressCard> (List<ProgressCard> pProgressCards)
	{  
		System.Random rng = new System.Random ();
		int n = pProgressCards.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next (n + 1);  
			ProgressCard value = pProgressCards [k];  
			pProgressCards [k] = pProgressCards [n];  
			pProgressCards [n] = value;  
		}  
	}

	public CardInventory getCardInventory()
	{
		return cardInventory;
	}

	/*********************************************
	Below are operations between the player and the bank
	**********************************************/

	//Take resource/commodity card of a given steableKind from the bank and give it to the given player
	public void distributeSteable (Player player, SteableKind steableKind, int num)
	{
		List<SteableCard> cards = cardInventory.removeSteableCard(steableKind, num);
		player.getCardInventory().addSteableCards(steableKind, cards);
	}

	//Take resource/commodity card of a given steableKind from the the given player and put it back to bank
	public void takeSteable (Player player, SteableKind steableKind, int num)
	{
		List<SteableCard> cards = player.getCardInventory().removeSteableCard(steableKind, num);
		cardInventory.addSteableCards(steableKind, cards);
	}



}
