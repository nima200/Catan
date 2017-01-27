using UnityEngine;
using System.Collections;

public class ResourceCard : Card {

	public ResourceKind resourceKind;


    public void initialize(string id, ResourceKind rk)
    {
        base.initialize(id);
		this.resourceKind = rk;
//		print (id + " " + rk);
        this.activateable = false;
        this.stealable = true;

    }
}
