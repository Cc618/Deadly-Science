using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        protected override bool OnCollect(GameObject player)
        {
            var client = player.GetComponent<Player>();

            Debug.Log("Serum : Collected");

            // If the player is not controlled by 
            if (!client)
                return false;

            Debug.Log("Serum : Collected by client");

            var p = player.GetComponent<PlayerState>();

            if (p.Status == PlayerState.PlayerStatus.INFECTED)
            {
                // Heal player
                p.Status = PlayerState.PlayerStatus.HEALED;

                // Update client
                client.OnSerumCollect();

                return true;
            }

            return false;
        }
    }
}
