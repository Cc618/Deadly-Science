using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serum : PowerUp
{
    protected override void OnCollect()
    {
        Debug.Log("Player -> Serum");
    }
}
