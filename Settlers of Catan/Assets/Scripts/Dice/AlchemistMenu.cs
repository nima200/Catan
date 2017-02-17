using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemistMenu: MonoBehaviour {
    public CountDisp Ydie;
    public CountDisp Rdie;

    public int GetRedDieValue() {
        return Rdie.GetValue();
    }

    public int GetYellowDieValue(){
        return Ydie.GetValue();
    }
}
