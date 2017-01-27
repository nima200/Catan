using UnityEngine;
using System.Collections;

public class ProgressCard : Card {
	public ProgressCardKind progressCardKind;

    public void initialize(string id, ProgressCardKind pck)
    {
        base.initialize(id);
		this.progressCardKind = pck;
//		print (id + " " + pck);
        this.activateable = true;
        this.stealable = false;
    }

}
