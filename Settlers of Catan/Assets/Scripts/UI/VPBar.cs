using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VPBar : MonoBehaviour {

    public GameObject vpBar;
    public RawImage[] VP;
    public int VPCount;
    public Color VPActive;
    public Color VPInactive;


	// Use this for initialization
	void Start () {
        VP = vpBar.GetComponentsInChildren<RawImage>();
        for (int i = 0; i < 13; i++) {
            Debug.Log(VP[i].name);
//            Debug.Log(VP[i].transform.name);
        }
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
