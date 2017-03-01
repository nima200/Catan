﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuManager : MonoBehaviour
{

    public RawImage cardInfoImg;
    private GameObject cardInfoObject;

    public GameObject resourceCommodityDecks;
    public GameObject progressCardDecks;
    private bool resourceCommodityShown;


    //Initialization
    void Start()
    {
        Instantiate(cardInfoImg);
        cardInfoObject = cardInfoImg.gameObject;
        resourceCommodityShown = true;
    }

    //Hide the information of the given progress card
    public void HideProgressCardInfo()
    {
        cardInfoObject.SetActive(false);
    }

    //Show the information of the given progress card
    public void ShowProgressCardInfo(GameObject progressCardObj)
    {
        cardInfoObject.SetActive(true);
        RawImage ri = progressCardObj.gameObject.GetComponent<RawImage>();
        if (ri)
        {
            string cardTexture = ri.texture.ToString();
            int length = cardTexture.Length;
            string cardInfoTexture = cardTexture.Substring(0, length - 25) + "1";
            cardInfoImg.texture = (Texture2D)Resources.Load(cardInfoTexture);
            Debug.Log(cardInfoTexture);
        }
        else Debug.Log("Nah pandeja");
    }

    //Switch to Resources/Commodities deck or to the progress card deck
    public void Shift()
    {
        if (resourceCommodityShown)
        {
            ShowProgressCardDeck();
            resourceCommodityShown = false;
        }
        else
        {
            ShowResourceCommodityDeck();
            resourceCommodityShown = true;
        }
        Debug.Log("WOHOO ya choose meh !");
    }

    //Show Res/Comm deck
    void ShowResourceCommodityDeck()
    {
        resourceCommodityDecks.SetActive(true);
        progressCardDecks.SetActive(false);

    }

    //Show Progress cards deck
    void ShowProgressCardDeck()
    {
        resourceCommodityDecks.SetActive(false);
        progressCardDecks.SetActive(true);

    }
}