using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

    // Parent class, which gets overriden by all types of cards.
    // Below are the general attributes that every card, regardless of type, will have in the game.
    public bool stealable;
    public bool activateable;
    public string id;
    
    // virtual method to allow overriding.
    public virtual void initialize(string id)
    {
        this.id = id;
    }
	
}
