using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Luminosite : MonoBehaviour
{
    public Light l;
    public Color alarme;
    public static List<Luminosite> instance = new List<Luminosite>();

    public void Start()
    {
        instance.Add(this);
    }
    public void Change()
    {
        l.color = alarme;
    }
}
