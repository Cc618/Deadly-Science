using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // TODO : Filter by layers
        if (other.gameObject.tag == "Player")
        {
            OnCollect();
            Destroy(gameObject);
        }
    }

    // When a player hits the power up
    // TODO : Pass the player as param
    protected abstract void OnCollect();
}
