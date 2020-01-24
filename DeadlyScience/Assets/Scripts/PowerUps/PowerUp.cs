using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO : Filter by layers
        if ((other.gameObject.layer & playerLayer) != 0)
        {
            OnCollect(other.gameObject);
            Destroy(gameObject);
        }
    }

    // When a player hits the power up
    // TODO : Pass the player as param
    protected abstract void OnCollect(GameObject player);
    private int playerLayer;
}
