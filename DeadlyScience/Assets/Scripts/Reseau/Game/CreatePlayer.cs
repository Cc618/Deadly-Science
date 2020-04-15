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
            int coorp = CreateRoomMenu.where[((int)PhotonNetwork.CountOfPlayers) % 4];
            var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), 
                new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), 2, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)),
                Quaternion.identity, 0);
            _player.GetComponent<PlayerNetwork>().isLocal = true;
            print("Le joueur a été instancié");
        }
    }
}

