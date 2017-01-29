using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour
{
	private CardInventory cardInventory = new CardInventory();
	public ResourceCard resourceCardPrefab;
	public CommodityCard commodityCardPrefab;
	public ProgressCard progressCardPrefab;


    
	// Use this for initialization
	void Start ()
	{
		GameObject rCards = new GameObject ("Resource Cards");
        GameObject cCards = new GameObject("Commodity Cards");
        GameObject pCards = new GameObject("Progress Cards");
        createStealableCard ();
		GameObject Cards = new GameObject ("Cards");
		createStealableCard ();
		createProgressCard ();
		List<ProgressCard> progressCards = cardInventory.getProgressCards();
		for (int i = 0; i < progressCards.Count; i++) {
			print(progressCards[i].GetComponent<ProgressCard>().id + " " + progressCards[i].GetComponent<ProgressCard>().progressCardKind);
		}
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
				cardInventory.addCommodityCard((CommodityKind)i, card);
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

}
