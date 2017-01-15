using UnityEngine;
using System.Collections;

public class ProgressCard : Card {
    public override void initialize(string id)
    {
        base.initialize(id);
        this.activateable = true;
        this.stealable = false;
    }

}
