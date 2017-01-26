using UnityEngine;
using System.Collections;

public class DisableLight : MonoBehaviour {

    public new Light light;
	// Use this for initialization
	void Start () {
        light.enabled = false;
	}
}
