﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class CreatePlayer : MonoBehaviourPunCallbacks
    {
        void Start()
        {
            bool not_found = true;
            Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
            int i = 0;
            while (not_found && i < PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.PlayerList[i] == player)
                {
                    not_found = false;
                }
                else
                {
                    i++;
                }
            }
            int coorp = CreateRoomMenu.where[i];
            //int coorp = CreateRoomMenu.where[PhotonNetwork.CountOfPlayers % 4];
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Attente"), new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), (float) 3.75, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)), Quaternion.identity, 0);
            var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), 
                new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), 3, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)),
                Quaternion.identity, 0);
            _player.GetComponent<PlayerNetwork>().isLocal = true;
            if (PhotonNetwork.IsMasterClient)
            {
                int g = CreateRoomMenu.PlayerNumber -1;
                if (g == 0)
                {
                    g = 1;
                }

                print("Sérums :");
                while (g > 0)
                {
                    g -= 1;
                    print(CreateRoomMenu.where[g + 4]);
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Serum"),
                        new Vector3((float) (4 * (CreateRoomMenu.where[g+4] % CreateRoomMenu.Xm) + 2.5), 2, (float) (4 * (CreateRoomMenu.where[g+4] / CreateRoomMenu.Xm) + 2.5)),
                        new Quaternion(0, 0, 0, 0));
                }
            }
            print("Le joueur a été instancié");
        }
    }
}

