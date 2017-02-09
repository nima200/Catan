using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCardUnit : MonoBehaviour {

    public GameObject otherLights;
    public GameObject cardLight;
    public GameObject cardInfo;
    public RawImage cardInfoImg;

    void Start() {
        cardInfoImg = cardInfo.GetComponent<RawImage>();
    }

    public void OnMouseEnter() {
        otherLights.SetActive(false);
        cardLight.SetActive(true);
        ShowProgressCard();
    }

    public void OnMouseExit() {
        otherLights.SetActive(true);
        cardLight.SetActive(false);
        cardInfo.SetActive(false);
    }

    void ShowProgressCard() {
        cardInfo.SetActive(true);
        RawImage ri = gameObject.GetComponent<RawImage>();
        if (ri){
            string cardTexture = ri.texture.ToString();
            int length = cardTexture.Length;
            string cardInfoTexture = cardTexture.Substring(0, length - 25) + "1";
            cardInfoImg.texture = (Texture2D) Resources.Load(cardInfoTexture);
            Debug.Log(cardInfoTexture);
        }
        else Debug.Log("Nah pandeja");
    }
}
