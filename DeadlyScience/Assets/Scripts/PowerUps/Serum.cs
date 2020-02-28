using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        protected override bool OnCollect(GameObject player)
        {
            var p = player.GetComponent<PlayerState>();

            if (p.Status == PlayerState.PlayerStatus.INFECTED)
            {
                // Heal player
                p.Status = PlayerState.PlayerStatus.HEALED;
                return true;
            }

            return false;
        }
    }
}
