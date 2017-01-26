using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        GameObject Cards = new GameObject("Cards");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void createCard(string cardType)
    {
        CardIDGenerator.generate();
        string cardID = CardIDGenerator.ids[CardIDGenerator.ids.Count - 1];
        GameObject card = new GameObject(cardType + cardID);
        if (cardType.Equals("card"))
        {
            card.AddComponent<Card>().initialize(cardID);
            card.transform.parent = GameObject.Find("Cards").gameObject.transform;
        } else if (cardType.Equals("resourceCard")) {
            card.AddComponent<ResourceCard>().initialize(cardID);
            card.transform.parent = GameObject.Find("Cards").gameObject.transform;
        } else if (cardType.Equals("commodityCard"))
        {
            card.AddComponent<CommodityCard>().initialize(cardID);
            card.transform.parent = GameObject.Find("Cards").gameObject.transform;
        } else if (cardType.Equals("progressCard"))
        {
            card.AddComponent<ProgressCard>().initialize(cardID);
            card.transform.parent = GameObject.Find("Cards").gameObject.transform;
        }
        
    }
}
