using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class CreatePlayer : MonoBehaviour
    {
        private PhotonView PV;

        void Start()
        {
            PV = GetComponent<PhotonView>();
            // TODO : LEANDRE : Position
            var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 6), Quaternion.identity, 0);
            player.GetComponent<PlayerNetwork>().isLocal = true;

            print("Le joueur a été instancié");
        }
    }
}

