using UnityEngine;
using System.Collections;

public class ResourceCard : Card {

    public override void initialize(string id)
    {
        base.initialize(id);
        this.activateable = false;
        this.stealable = true;
    }
}
