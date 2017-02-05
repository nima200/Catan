using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInventory : MonoBehaviour
{

	public List<ProgressCard> progressCards = new List<ProgressCard> ();
	public Dictionary<SteableKind,  List<SteableCard>> steableCards = new Dictionary<SteableKind, List<SteableCard>>();


	public CardInventory ()
	{
		for (int i = 0; i < 8; i++) {
			steableCards.Add((SteableKind)i, new List<SteableCard> ());
		}
	}

	public int countSteableCard (SteableKind kind)
	{
		return steableCards [kind].Count;
	}

	public void addSteableCard (SteableKind kind, SteableCard card)
	{
		steableCards [kind].Add (card);
	}

	public void addSteableCards (SteableKind kind, List<SteableCard> cards)
	{
		steableCards [kind].AddRange(cards);
	}

	public void addProgressCard (ProgressCard card)
	{
		progressCards.Add (card);
	}

	public List<SteableCard> removeSteableCard (SteableKind steableKind, int num)
	{
		List<SteableCard> list = steableCards[steableKind];
		List<SteableCard> rc = list.GetRange(0, num);
		list.RemoveRange(0,num);
		return rc;
	}

	public List<ProgressCard> getProgressCards ()
	{
		return progressCards;
	}

	public void iterateSteableCards ()
	{
		for (int i = 0; i < 8; i++) {
			int count = steableCards[(SteableKind)i].Count;
			print ((SteableKind)i + " " + count);
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
