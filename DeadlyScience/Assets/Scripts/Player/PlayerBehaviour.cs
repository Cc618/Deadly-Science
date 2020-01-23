using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void Update()
    {
        // Test for input
        // TODO : Remove
        if (Input.GetKeyDown(Game.inputs.left))
            Debug.Log("Left");
        if (Input.GetKeyDown(Game.inputs.forward))
            Debug.Log("Forward");
    }
}
