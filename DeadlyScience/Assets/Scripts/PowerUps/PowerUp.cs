using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
            if (OnCollect(other.gameObject))
                Destroy(gameObject);
                // TODO : PhotonNetwork.Destroy(gameObject);
    }

    // When a player hits the power up
    // Returns whether we must remove the power up
    protected abstract bool OnCollect(GameObject player);

    private int playerLayer;
}
