using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Luminosite : MonoBehaviour
{
    public Light l;
    public Color alarme;
    public Color nuit;
    public static List<Luminosite> instance = new List<Luminosite>();

    public void Start()
    {
        instance.Add(this);
    }
    public void Change(bool type)
    {
        print("LIIIIIGHT");
        if (type)
        {
            l.color = alarme;
        }
        else
        {
            l.color = nuit;
        }
    }
}
