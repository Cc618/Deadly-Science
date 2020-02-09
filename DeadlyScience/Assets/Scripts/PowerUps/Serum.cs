using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        protected override bool OnCollect(GameObject player)
        {
            var p = player.GetComponent<PlayerBrain>();

            if (p.Status == PlayerBrain.PlayerStatus.INFECTED)
            {
                // Heal player
                p.Status = PlayerBrain.PlayerStatus.HEALED;
                return true;
            }

            return false;
        }
    }
}
