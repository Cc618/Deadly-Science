using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private void Start()
    {
        // TODO : Player sensor collides with this layer
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & playerLayer) != 0)
        {
            OnCollect(other.gameObject);
            Destroy(gameObject);
        }
    }

    // When a player hits the power up
    protected abstract void OnCollect(GameObject player);
    private int playerLayer;
}
