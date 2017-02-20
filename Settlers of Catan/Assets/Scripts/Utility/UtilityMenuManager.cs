using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityMenuManager : MonoBehaviour {

    public GameObject utilityMenu;
    private Dropdown buildDropDown;
    public GameObject mainMenu;
    public GameObject buildRoadMenu;

	void Start () {
        buildDropDown = utilityMenu.GetComponentInChildren<Dropdown>();
	}

    public void BuildMenuOptions() {
        Debug.Log(buildDropDown.value + " was chosen");
        if (buildDropDown.value == 1) {
            mainMenu.SetActive(false);
            buildRoadMenu.SetActive(true);
        }
    }

    public void HideBuildRoadMenu() {
        mainMenu.SetActive(true);
        buildRoadMenu.SetActive(false);
        Destroy(buildDropDown.gameObject.GetComponentInChildren<ScrollRect>().gameObject);
    }
}
