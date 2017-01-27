using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

    public bool stealable;
    public bool activateable;
    public string id;
    //public GameObject card;

    public virtual void initialize(string id)
    {
        this.id = id;
    }
	
}
