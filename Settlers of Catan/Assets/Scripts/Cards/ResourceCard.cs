﻿using UnityEngine;
using System.Collections;

// The respective attributes of ResourceCard overriding base.initialize(string id).
// We first call base.initialize(id) for assigning the ID since base.init does the job already.
public class ResourceCard : Card {

    public override void initialize(string id)
    {
        base.initialize(id);
        this.activateable = false;
        this.stealable = true;
    }
}
