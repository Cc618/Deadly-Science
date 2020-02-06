using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serum : PowerUp
{
    protected override bool OnCollect(GameObject player)
    {
        var p = player.GetComponent<PlayerBehaviour>();

        if (p.Status == PlayerBehaviour.PlayerStatus.INFECTED)
        {
            // Heal player
            p.Status = PlayerBehaviour.PlayerStatus.HEALED;
            return true;
        }

        return false;
    }
}
