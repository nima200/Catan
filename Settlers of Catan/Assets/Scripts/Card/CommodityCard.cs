using UnityEngine;
using System.Collections;

public class CommodityCard : Card {
	public CommodityKind commodityKind;

    public void initialize(string id, CommodityKind ck)
    {
        base.initialize(id);
		this.commodityKind = ck;
//		print (id + " " + ck);
        this.activateable = false;
        this.stealable = true;
    }
}
