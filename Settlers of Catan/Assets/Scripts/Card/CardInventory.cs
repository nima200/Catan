using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInventory : MonoBehaviour
{

	private Dictionary<ResourceKind, List<GameObject>> resourceCards = new Dictionary<ResourceKind, List<GameObject>> ();
	private Dictionary<CommodityKind, List<GameObject>> commodityCards = new Dictionary<CommodityKind, List<GameObject>> ();
	private List<GameObject> progressCards = new List<GameObject> ();

	public void addResoueceCard (ResourceKind kind, GameObject card)
	{
		if (!resourceCards.ContainsKey (kind)) {
			resourceCards.Add (kind, new List<GameObject> ());
		}
		resourceCards [kind].Add (card);
	}

	public void addCommodityCard (CommodityKind kind, GameObject card)
	{
		if (!commodityCards.ContainsKey (kind)) {
			commodityCards.Add (kind, new List<GameObject> ());
		}
		commodityCards [kind].Add (card);
	}

	public void addProgressCard (GameObject card)
	{
		progressCards.Add (card);
	}

	public List<GameObject> getProgressCards ()
	{
		return progressCards;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
