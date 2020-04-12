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

            // If the player is not controlled by 
            if (!client)
                return false;

            var p = player.GetComponent<PlayerState>();

            if (p.Status == PlayerState.PlayerStatus.INFECTED)
            {
                // Heal player
                p.Status = PlayerState.PlayerStatus.HEALED;

                // Update client / other players
                client.OnSerumCollect();

                // Destroy serum
                // TODO : BY MASTER
                PhotonNetwork.Destroy(gameObject);

                return true;
            }

            return false;
        }
    }
}
