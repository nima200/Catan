using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel: MonoBehaviour {
    public CountDisp Ydie;
    public CountDisp Rdie;
    public Button submit;
   
    public int GetRedDieValue() {
        return Rdie.GetValue();
    }

    public int GetYellowDieValue(){
        return Ydie.GetValue();
    }
}
