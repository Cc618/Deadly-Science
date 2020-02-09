using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        protected override bool OnCollect(GameObject player)
        {
            var p = player.GetComponent<Player>();

            if (p.Status == Player.PlayerStatus.INFECTED)
            {
                // Heal player
                p.Status = Player.PlayerStatus.HEALED;
                return true;
            }

            return false;
        }
    }
}
