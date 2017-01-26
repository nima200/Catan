using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class UtilityMenu : MonoBehaviour {

    public new Light light;
    public new AudioSource audio;
    bool played;

    void OnMouseEnter()
    {
        transform.localPosition = new Vector3(-1428.0f, -65.0f, -12.0f);
        light.enabled = true;
        played = true;
        playSound();
    }
    void OnMouseExit()
    {
        transform.localPosition = new Vector3(-1673.0f, -65.0f, -12.0f);
        light.enabled = false;
        played = true;
        playSound();
    }
    void playSound()
    {
        if (played)
        {
            audio.Play();
            played = false;
        }

    }
}
