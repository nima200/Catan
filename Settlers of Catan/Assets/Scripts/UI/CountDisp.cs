using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDisp : MonoBehaviour
{

    private Text display;
    public int value;
    public int[] minMax;  // <--- TODO : have a reference to a GAMEOBJECT holding a min and max val, (implement a getMin() and getMax() methods)
    public int incrementFactor;

    private void Awake()
    {
        display = gameObject.GetComponentInChildren<Text>();
        display.text = value.ToString();
    }


    public int GetValue()
    {
        return value;
    }

    public void SetValue()
    {
        display.text = value.ToString();
    }

    public void Decrease()
    {
        if (value - incrementFactor >= minMax[0])
        {
            value = value - incrementFactor;
        }
        SetValue();
    }

    public virtual void Increase()
    {
        if (value + incrementFactor <= minMax[1])
        {
            value = value + incrementFactor;
        }
        SetValue();
    }

    
}
