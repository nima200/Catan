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
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static UiManager GetInstance()
    {
        return _instance;
    }
    // BUILD MENU
    public GameObject BuildMenu;
    public Selectable BuildRoadButton;
    public Selectable BuildShipButton;
    public Selectable BuildSettleButton;
    public Selectable BuildCityButton;
    public Dropdown DirectionDropdown;



}
