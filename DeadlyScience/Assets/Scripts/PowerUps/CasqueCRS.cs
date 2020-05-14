using System;
using System.Collections;
using System.Collections.Generic;
using ds;
using Photon.Pun.UtilityScripts;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class CasqueCRS : MonoBehaviour
{
    public Image carte;
    public static CasqueCRS instance;
    private static bool afficher = false;
    public static int time;
    public static bool chrono;

    private void Awake()
    {
        instance = this;
    }

    public void Change(bool affich)
    {
        gameObject.SetActive(affich);
        print("CRS : " + affich);
        afficher = affich;
        if (affich)
        {
            time = 30;
            chrono = false;
        }
    }

    private void Update()
    {
        if (time > 0)
        {
            if (!chrono)
            {
                print(time);
                StartCoroutine(Attente());
            }
        }
        else
        {
            if (afficher)
            {
                print("STOP");
                Change(false);
                Player.alterations[4] = false;
                AffichagePowerUpJoueur.MaJ(Player.alterations);
            }
        }
    }
    IEnumerator Attente()
    {
        chrono = true;
        yield return new WaitForSeconds(1);
        time -= 1;
        chrono = false;
    }
}