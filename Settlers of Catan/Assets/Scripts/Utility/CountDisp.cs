using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDisp : MonoBehaviour {
    public UnityEngine.UI.Text display;
    public int value;
    public int[] minMax;  // <--- TODO : have a reference to a GAMEOBJECT holding a min and max val, (implement a getMin() and getMax() methods)
    public Button minus;
    public Button plus;
    public SteableKind steableKind;
    public int incrementFactor;


    /*==========================
     *          START
     *==========================
     * init min, max and value
     */
    void Start () {
        value = 1;
        minMax = new int[2] { 1, 6 };
		incrementFactor = 1;
        minus.onClick.AddListener(Decrease);
        plus.onClick.AddListener(Increase);
	}

    /*==========================
     *        UPDATE VAL
     *==========================
     * Update value of text (display)
     */
    public void UpdateVal() {
        display.text = value.ToString();
    }

    /*==========================
     *        DECREASE
     *==========================
     */
    void Decrease(){
		if (value - incrementFactor >= minMax[0]) {
			value = value - incrementFactor;
        }
        UpdateVal();
    }


    /*==========================
     *          INCREASE
     *==========================
     */
    void Increase() {
		if (value+incrementFactor < minMax[1]) {
            value = value +incrementFactor;
        }
        UpdateVal();
    }

    public int GetValue() {
        return value;
    }
}
