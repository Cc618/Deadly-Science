using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class Serum : PowerUp
    {
        public static int lastId = 0;
        public static List<Serum> instances = new List<Serum>();

        // Used to destroy this serum by the master
        [HideInInspector]
        public int id;
        public static int rank = 0;

        public void Awake()
        {
            id = ++lastId;
            instances.Add(this);
            print(id);
        }

        protected override int OnCollect(GameObject player)
        {
            var client = player.GetComponent<Player>();
            // If the player is not controlled by the client
            if (!client)
                return 0;
            var p = player.GetComponent<PlayerState>();
            if (p.Status == PlayerState.PlayerStatus.INFECTED)
            {
                // Update client / other players
                client.OnSerumCollect(id);
                // We return false because this is done remotely
                return 2;
            }
            return 0;
        }
    }
}
