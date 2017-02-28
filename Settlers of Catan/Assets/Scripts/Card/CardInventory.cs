using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardInventory : MonoBehaviour
{

	public List<ProgressCard> progressCards = new List<ProgressCard> ();
	public Dictionary<SteableKind,  List<SteableCard>> steableCards = new Dictionary<SteableKind, List<SteableCard>>();
	int STEABLE_KIND_TOTAL = 9;
	public delegate void InventoryChangedDelegate();
	public event InventoryChangedDelegate InventoryChanged;

	void Awake ()
	{
		for (int i = 0; i < STEABLE_KIND_TOTAL; i++) {
			steableCards.Add((SteableKind)i, new List<SteableCard> ());
        }

    }

	public int countSteableCard (SteableKind kind)
	{
		return steableCards[kind].Count;
	}

	public void addSteableCard (SteableKind kind, SteableCard card)
	{
		steableCards [kind].Add (card);
		if (InventoryChanged != null) {
			InventoryChanged();
		}
	}

	public void addSteableCards (SteableKind kind, List<SteableCard> cards)
	{
		steableCards [kind].AddRange(cards);
		if (InventoryChanged != null) {
			InventoryChanged();
		}
	}

	public void addProgressCard (ProgressCard card)
	{
		progressCards.Add (card);
		if (InventoryChanged != null) {
			InventoryChanged();
		}
	}

	public List<SteableCard> removeSteableCard (SteableKind steableKind, int num)
	{
		List<SteableCard> list = steableCards[steableKind];
		List<SteableCard> rc = list.GetRange(0, num);
		list.RemoveRange(0,num);
		if (InventoryChanged != null) {
			InventoryChanged();
		}
		return rc;
	}

	public List<ProgressCard> getProgressCards ()
	{
		return progressCards;
	}

	public void iterateSteableCards ()
	{
		for (int i = 0; i < STEABLE_KIND_TOTAL; i++) {
			int count = steableCards[(SteableKind)i].Count;
			print ((SteableKind)i + " " + count);
		}
	}


}
