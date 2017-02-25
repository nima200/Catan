using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VPBar : MonoBehaviour {
    
    RawImage[] VP;
    int VPCount; // get from player
    public Color VPActive;
    public Color VPInactive;


	// Use this for initialization
	void Start () {
        VP = gameObject.GetComponentsInChildren<RawImage>();
        VPCount = 0;
    }
	

	public void ActivateVP() {
        if (VPCount < 13) {
            Debug.Log("Activate color");
            VPCount++;
            VP[VPCount-1].color = VPActive;
        }
        Debug.Log("VPCount: "+VPCount);
	}

    public void DeactivateVP() {
        if (VPCount > 0) {
            Debug.Log("Deactivate color");
            VPCount--;
            VP[VPCount].color = VPInactive;
        }
        Debug.Log("VPCount: " + VPCount);
    }
}
