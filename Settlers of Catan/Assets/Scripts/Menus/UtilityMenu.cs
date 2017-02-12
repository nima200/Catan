using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class UtilityMenu : MonoBehaviour {

    public new Light light;
    public new AudioSource audio;
    public GameObject buildMenu;
    public GameObject buttonMenu;

    bool played;
    bool inBuildMenu;

    void Start() {
        inBuildMenu = false;
    }

    void OnMouseEnter()
    {
        if (!inBuildMenu) {
            transform.localPosition = new Vector3(-1428.0f, -65.0f, -12.0f);
            light.enabled = true;
            played = true;
            playSound();
        }
    }
    void OnMouseExit()
    {
        if (!inBuildMenu) {
            transform.localPosition = new Vector3(-1673.0f, -65.0f, -12.0f);
            light.enabled = false;
            played = true;
            playSound();
        }
    }

    public void ShowBuildMenu() {
        inBuildMenu = true;
        buttonMenu.SetActive(false);
        buildMenu.SetActive(true);
    }

    public void HideBuildMenu() {
        inBuildMenu = false;
        buttonMenu.SetActive(true);
        buildMenu.SetActive(false);
    }


    void playSound() {
        if (played) {
            audio.Play();
            played = false;
        }

    }
}
