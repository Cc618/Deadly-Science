// Class to gather all informations and properties of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Inputs inputs;

    private void Awake()
    {
        inputs = GetComponent<Inputs>();
    }
}
