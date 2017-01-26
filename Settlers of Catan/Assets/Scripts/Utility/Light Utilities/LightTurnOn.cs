using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class LightTurnOn : MonoBehaviour {
    public new AudioSource audio;
    public new Light light;
    public bool hovered;
    bool played;
    void Start()
    {
        light.enabled = false;
    }

    void Update()
    {
        toggleLight();
    }

    void OnMouseEnter()
    {
        hovered = true;
        played = true;
        playSound();
        Debug.Log("On");
    }

    void OnMouseExit()
    {
        hovered = false;
        played = true;
        playSound();
        Debug.Log("Off");
    }

    void toggleLight()
    {
        if (hovered)
        {
            light.enabled = true;
            
        } else
        {
            light.enabled = false;
            
        }
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
