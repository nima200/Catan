using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInventory : MonoBehaviour
{

	public Dictionary<ResourceKind, List<ResourceCard>> resourceCards = new Dictionary<ResourceKind, List<ResourceCard>> ();
	public Dictionary<CommodityKind, List<CommodityCard>> commodityCards = new Dictionary<CommodityKind, List<CommodityCard>> ();
	public List<ProgressCard> progressCards = new List<ProgressCard> ();
	public Dictionary<SteableCard> steableCards = new Dictionary<SteableCard>();


	public CardInventory ()
	{
		for (int i = 0; i < 5; i++) {
			resourceCards.Add ((ResourceKind)i, new List<ResourceCard> ());
		}
		for (int i = 0; i < 3; i++) {
			commodityCards.Add ((CommodityKind)i, new List<CommodityCard> ());
		}
	}

	public int countReourceCard (ResourceKind kind)
	{
		return resourceCards[kind].Count;
	}

	public int countCommodityCard (CommodityKind kind)
	{
		return commodityCards [kind].Count;
	}

	public void addResoueceCard (ResourceKind kind, ResourceCard card)
	{
		resourceCards [kind].Add (card);
	}

	public void addResoueceCards (ResourceKind kind, List<ResourceCard> cards)
	{
		resourceCards [kind].AddRange(cards);
	}

	public void addCommodityCard (CommodityKind kind, CommodityCard card)
	{
		commodityCards [kind].Add (card);
	}

	public void addCommodityCards (CommodityKind kind, List<CommodityCard> cards)
	{
		commodityCards [kind].AddRange(cards);
	}

	public void addProgressCard (ProgressCard card)
	{
		progressCards.Add (card);
	}

	public List<ResourceCard> removeResourceCard (ResourceKind resourceKind, int num)
	{
		List<ResourceCard> list = resourceCards[resourceKind];
		List<ResourceCard> rc = list.GetRange(0, num);
		list.RemoveRange(0,num);
		return rc;
	}

	public List<CommodityCard> removeCommodityCard (CommodityKind commodityKind, int num)
	{
		List<CommodityCard> list = commodityCards[commodityKind];
		List<CommodityCard> rc = list.GetRange(0, num);
		list.RemoveRange(0,num);
		return rc;
	}

	public List<ProgressCard> getProgressCards ()
	{
		return progressCards;
	}

	public void iterateResourceCards ()
	{
		for (int i = 0; i < 5; i++) {
			int count = resourceCards[(ResourceKind)i].Count;
			print ((ResourceKind)i + " " + count);
		}
	}

	public void iterateCommodityCards ()
	{
		for (int i = 0; i < 3; i++) {
			int count = commodityCards[(CommodityKind)i].Count;
			print ((CommodityKind)i + " " + count);
		}
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
