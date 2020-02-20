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

            var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), transform.position, Quaternion.identity, 0);
            player.GetComponent<PlayerNetwork>().isLocal = true;
            //player.GetComponentInChildren<Camera>().gameObject.SetActive(true);

            print("Le joueur a été instancié");

        }
    }
}

