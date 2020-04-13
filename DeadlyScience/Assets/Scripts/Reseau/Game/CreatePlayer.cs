using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class CreatePlayer : MonoBehaviour
    {
        void Start()
        {
            int countofplayers = PhotonNetwork.CountOfPlayers;
            if (countofplayers % 4 == 0)
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 6), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (countofplayers % 4 == 1)
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 4), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (countofplayers % 4 == 2)
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 2), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 0), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            // TODO : LEANDRE : Position

            print("Le joueur a été instancié");
        }
    }
}

