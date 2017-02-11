using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VPBar : MonoBehaviour {

    public Transform[] VP;
    public int VPCount;
    public Material VPActive;
    public Material VPInactive;


	// Use this for initialization
	void Start () {
        VP = new Transform[13];
        for (int i = 0; i < 13; i++) {
            VP[i] = gameObject.transform.GetChild(i);
            Debug.Log(VP[i].transform.name);
        }
        VPCount = 0;
        VPActive = (Material)Resources.Load("VPActive");
        VPInactive = (Material)Resources.Load("VPInactive");
    }
	

	public void ActivateVP() {
        if (VPCount < 13) {
            Debug.Log("Activate color");
            VPCount++;
            VP[VPCount-1].GetComponent<Renderer>().material = VPActive;
        }
        Debug.Log("VPCount: "+VPCount);
	}

    public void DeactivateVP() {
        if (VPCount > 0) {
            Debug.Log("Deactivate color");
            VPCount--;
            VP[VPCount].GetComponent<Renderer>().material = VPInactive;
        }
        Debug.Log("VPCount: " + VPCount);
    }
}
