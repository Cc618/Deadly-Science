﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        public static int lastId = 1;
        public static List<Serum> instances = new List<Serum>();

        // Used to destroy this serum by the master
        [HideInInspector]
        public int id;

        public void Awake()
        {
            id = ++lastId;
            instances.Add(this);
        }

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
                client.OnSerumCollect(id);

                // We return false because this is done remotely
                return false;
            }

            return false;
        }
    }
}
