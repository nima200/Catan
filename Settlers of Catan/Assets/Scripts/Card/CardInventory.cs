using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInventory : MonoBehaviour
{

	private Dictionary<ResourceKind, List<ResourceCard>> resourceCards = new Dictionary<ResourceKind, List<ResourceCard>> ();
	private Dictionary<CommodityKind, List<CommodityCard>> commodityCards = new Dictionary<CommodityKind, List<CommodityCard>> ();
	private List<ProgressCard> progressCards = new List<ProgressCard> ();

	public void addResoueceCard (ResourceKind kind, ResourceCard card)
	{
		if (!resourceCards.ContainsKey (kind)) {
			resourceCards.Add (kind, new List<ResourceCard> ());
		}
		resourceCards [kind].Add (card);
	}

	public void addCommodityCard (CommodityKind kind, CommodityCard card)
	{
		if (!commodityCards.ContainsKey (kind)) {
			commodityCards.Add (kind, new List<CommodityCard> ());
		}
		commodityCards [kind].Add (card);
	}

	public void addProgressCard (ProgressCard card)
	{
		progressCards.Add (card);
	}

	public List<ProgressCard> getProgressCards ()
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
