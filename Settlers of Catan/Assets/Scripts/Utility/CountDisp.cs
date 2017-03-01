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


    /*==========================
     *          START
     *==========================
     * init min, max and value
     */
    void Start () {
        value = 1;
        minMax = new int[2] { 1, 6 };
        minus.onClick.AddListener(Decrease);
        plus.onClick.AddListener(Increase);
	}

    /*==========================
     *        UPDATE VAL
     *==========================
     * Update value of text (display)
     */
    void UpdateVal() {
        display.text = "" + value;
    }

    /*==========================
     *        DECREASE
     *==========================
     */
    void Decrease(){
        if (value > minMax[0]) {
            value--;
        }
        UpdateVal();
    }


    /*==========================
     *          INCREASE
     *==========================
     */
    void Increase() {
        if (value < minMax[1]) {
            value++;
        }
        UpdateVal();
    }

    public int GetValue() {
        return value;
    }
}
