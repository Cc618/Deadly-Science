using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serum : PowerUp
{
    protected override void OnCollect(GameObject player)
    {
        // Heal player
        player.GetComponent<PlayerBehaviour>()
            .SetStatus(PlayerBehaviour.Status.HEALED);
    }
}
