using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {

    // Creates an empty gameobject, which will just act as a folder for all cards to go into,
    // in the editor Hierarchy. Parent for all cards.

    // TODO: Perhaps it's a good idea to create Progress Cards, Commodity Cards, Resource Cards,
    // just like this, as empty game objects, so that upon generation, cards go into their 
    // correct folders.
    void Start () {
        GameObject Cards = new GameObject("Cards");
    }

    
    public void createCard(string cardType)
    {
        // Calls onto the ID generation method. Since that ID becomes the last element of the ids list,
        // it auto assigns that as the card's ID. 
        CardIDGenerator.generate();
        string cardID = CardIDGenerator.ids[CardIDGenerator.ids.Count - 1];
        GameObject card = new GameObject(cardType + cardID);

        // Adding respective components (scripts) to the card generated.
        // They each have their own scripts so type has to be checked and 
        // the correct script has to be added.

        // TODO: This is where we'd have to assign correct parents to each card,
        // if we do implement the stuff mentioned in lines 9 - 11 above.

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
