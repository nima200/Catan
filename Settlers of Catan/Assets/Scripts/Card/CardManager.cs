using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardManager : MonoBehaviour
{
	private CardInventory cardInventory = new CardInventory();
    
	// Use this for initialization
	void Start ()
	{
		GameObject Cards = new GameObject ("Cards");
		createStealableCard ();
		createProgressCard ();
		List<GameObject> progressCards = cardInventory.getProgressCards();
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
				GameObject card = new GameObject ("resourceCard" + id);
				card.AddComponent<ResourceCard> ().initialize (stringID, (ResourceKind)i);
				card.transform.parent = GameObject.Find ("Cards").transform;
				cardInventory.addResoueceCard((ResourceKind)i, card);
			}
		}
		for (int i = 0; i < 3; i++) {
			for (int j = 1; j < 13; j++) {
				int id = i * 12 + j;
				string stringID = id.ToString ();
				GameObject card = new GameObject ("commodityCard" + id);
				card.AddComponent<CommodityCard> ().initialize (stringID, (CommodityKind)i);
				card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
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
				GameObject card = new GameObject ("progressCard" + id);
				card.AddComponent<ProgressCard> ().initialize (stringID, (ProgressCardKind)i);
				card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
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

	//	public void createCard (string cardType)
	//	{
	//		CardIDGenerator.generate ();
	//		string cardID = CardIDGenerator.ids [CardIDGenerator.ids.Count - 1];
	//		GameObject card = new GameObject (cardType + cardID);
	//		if (cardType.Equals ("card")) {
	//			card.AddComponent<Card> ().initialize (cardID);
	//			card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
	//		} else if (cardType.Equals ("resourceCard")) {
	//			card.AddComponent<ResourceCard> ().initialize (cardID);
	//			card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
	//		} else if (cardType.Equals ("commodityCard")) {
	//			card.AddComponent<CommodityCard> ().initialize (cardID);
	//			card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
	//		} else if (cardType.Equals ("progressCard")) {
	//			card.AddComponent<ProgressCard> ().initialize (cardID);
	//			card.transform.parent = GameObject.Find ("Cards").gameObject.transform;
	//		}
	//
	//	}
}
