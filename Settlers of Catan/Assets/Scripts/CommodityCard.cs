using UnityEngine;
using System.Collections;

public class CommodityCard : Card {

    public override void initialize(string id)
    {
        base.initialize(id);
        this.activateable = false;
        this.stealable = true;
    }
}
