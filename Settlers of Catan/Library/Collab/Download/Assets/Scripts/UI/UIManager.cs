using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Button voteButton;
    
    public Dropdown gBVoteDD;

	// Use this for initialization
	void Start ()
    {
        voteButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (gBVoteDD.value == 0)
        {
            voteButton.interactable = false;
        } else {
            voteButton.interactable = true;
        }
	}
}
