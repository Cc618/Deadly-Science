using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundSensor : MonoBehaviour
{
    [HideInInspector]
    public bool isGrounded = false;

    private void Start()
    {
        groundLayer = LayerMask.NameToLayer("Walls");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Filter layer
        if ((other.gameObject.layer & groundLayer) != 0)
            isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Filter layer
        if ((other.gameObject.layer & groundLayer) != 0)
            isGrounded = false;
    }

    private int groundLayer;
}
