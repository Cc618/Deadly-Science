using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class Other : PowerUp
    {
        public static int lastId = 0;
        public static List<Other> instances = new List<Other>();

        // Used to destroy this serum by the master
        [HideInInspector]
        public int id;
        public void Awake()
        {
            id = ++lastId;
            instances.Add(this);
        }
        protected override int OnCollect(GameObject player)
        {
            var client = player.GetComponent<Player>();
            // If the player is not controlled by the client
            if (!client)
                return 0;
            var p = player.GetComponent<PlayerState>();
            //Update client/other players
            client.OnPowerUpCollect();
            print("A");
            // We return false because this is done remotely
            return 1;
        }
    }
}