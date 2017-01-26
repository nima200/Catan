using UnityEngine;
using System.Collections;

public class CommodityCard : Card {

    // The respective attributes of CommodityCard overriding base.initialize(string id).
    // We first call base.initialize(id) for assigning the ID since base.init does the job already.
    public override void initialize(string id)
    {
        base.initialize(id);
        this.activateable = false;
        this.stealable = true;
    }
}
