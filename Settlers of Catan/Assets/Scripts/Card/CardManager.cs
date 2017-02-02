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

	//Make Card Manager Singleton
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

	public static CardManager getInstance ()
	{
		return instance;
	}

	// Use this for initialization
	void Start ()
	{
		cardInventory = Instantiate(cardInventoryPrefab);
		cardInventory.transform.parent = GameObject.Find("CardManager").transform;
		GameObject cards = new GameObject("Cards");
		GameObject resourceCards = new GameObject ("Resource Cards");
		resourceCards.transform.parent = cards.transform;
		GameObject commodityCards = new GameObject("Commodity Cards");
		commodityCards.transform.parent = cards.transform;
		GameObject progressCards = new GameObject("Progress Cards");
		progressCards.transform.parent = cards.transform;
        createStealableCard ();
		createProgressCard ();
		//		distributeResource(PlayerManager.getInstance.getPlayer(0), ResourceKind.BRICK, 3);
		//		distributeCommodity(PlayerManager.getInstance.getPlayer(0), CommodityKind.CLOTH, 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	//Create 95 resource cards and 36 commodity cards
	private void createStealableCard ()
	{
		
		for (int i = 0; i < 5; i++) {
			for (int j = 1; j < 20; j++) {
				int id = i * 19 + j;
				string stringID = id.ToString ();
				ResourceCard card = Instantiate(resourceCardPrefab);
				card.name = "resourceCard" + id;
				card.resourceKind = (ResourceKind)i;
				card.id = stringID;
                card.transform.parent = GameObject.Find("Resource Cards").transform;
				cardInventory.addResoueceCard((ResourceKind)i, card);
			}
		}
		for (int i = 0; i < 3; i++) {
			for (int j = 1; j < 13; j++) {
				int id = i * 12 + j;
				string stringID = id.ToString ();
				CommodityCard card = Instantiate(commodityCardPrefab);
				card.name = "commodityCard" + id;
				card.commodityKind = (CommodityKind)i;
				card.id = stringID;
                card.transform.parent = GameObject.Find("Commodity Cards").transform;
				cardInventory.addCommodityCard((CommodityKind)i, card);

				//was there a reason you decided to add twice?
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

	//Take resource card of a given resourcekKind from the bank and give it to the given player
	public void distributeResource (Player player, ResourceKind resourceKind, int num)
	{
		List<ResourceCard> cards = cardInventory.removeResourceCard(resourceKind, num);
		player.getCardInventory().addResoueceCards(resourceKind, cards);
	}

	//Take commodity card of a given commodityKind from the bank and give it to the given player
	public void distributeCommodity (Player player, CommodityKind commodityKind, int num)
	{
		List<CommodityCard> cards = cardInventory.removeCommodityCard(commodityKind, num);
		player.getCardInventory().addCommodityCards(commodityKind, cards);
	}

	//Take resource card of a given resourcekKind from the the given player and put it back to bank
	public void takeResource (Player player, ResourceKind resourceKind, int num)
	{
		List<ResourceCard> cards = player.getCardInventory().removeResourceCard(resourceKind, num);
		cardInventory.addResoueceCards(resourceKind, cards);
	}

	//Take commodity card of a given commodityKind from the the given player and put it back to bank
	public void takeCommodity (Player player, CommodityKind commodityKind, int num)
	{
		List<CommodityCard> cards = player.getCardInventory().removeCommodityCard(commodityKind, num);
		cardInventory.addCommodityCards(commodityKind, cards);
	}




}
