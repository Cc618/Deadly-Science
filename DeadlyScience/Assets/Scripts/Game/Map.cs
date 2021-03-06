﻿using System;
using System.Collections;
using System.Collections.Generic;
using ds;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    private int x = 0;
    private int y = 0;
    public static Texture2D aTexture;
    public Image carte;
    private Texture2D curseur;
    public static Map instance;
    private static bool afficher = false;

    private void Awake()
    {
        instance = this;
    }

    public void Change(bool affich)
    {
        gameObject.SetActive(affich);
        afficher = affich;
        print("Carte mise à " + affich);
    }

    public void Chargement(Texture2D m)
    {
        aTexture = m;
        aTexture.Apply();
        carte.rectTransform.sizeDelta = new Vector2(aTexture.width,aTexture.height);
        carte.sprite = Sprite.Create(aTexture, new Rect(0, 0, aTexture.width, aTexture.height), new Vector2(0.5f, 0.5f));
    }
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(x+5, Screen.height-y-10, 5, 5), curseur, ScaleMode.StretchToFill);
    }

    private void Start()
    {
        curseur = new Texture2D(5,5);
        curseur = Generation.Bloc(curseur,0,0,Color.red);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (afficher)
        {
            x = (int) (PlayerNetwork.local.gameObject.transform.position.x-0.5)/4*10;
            y = (int) (PlayerNetwork.local.gameObject.transform.position.z-0.5)/4*10;
        }
        else
        {
            x = -5;
            y = -10;
        }
    }
}