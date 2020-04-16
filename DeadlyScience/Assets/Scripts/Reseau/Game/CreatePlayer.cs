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
            if (PhotonNetwork.IsMasterClient)
            {
                int g = 0;
                while (g < 3)
                {
                    var newCube = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Serum"),
                        new Vector3((float) (4 * (CreateRoomMenu.where[g+4] % CreateRoomMenu.Xm) + 2.5), 2, (float) (4 * (CreateRoomMenu.where[g+4] / CreateRoomMenu.Xm) + 2.5)),
                        new Quaternion(0, 0, 0, 0));
                    g += 1;
                }
            }
            print("Le joueur a été instancié");
        }
    }
}

