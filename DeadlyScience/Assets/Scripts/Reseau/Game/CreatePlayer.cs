using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;

namespace ds
{
    public class CreatePlayer : MonoBehaviourPunCallbacks
    {
        private PhotonView pv;
        void Start()
        {
            pv = GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    int coorp = CreateRoomMenu.where[0];
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Attente"), new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), (float) 3.8, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)), Quaternion.identity, 0);
                    GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), 
                        new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), 3, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)),
                        Quaternion.identity, 0);
                    player.GetComponent<PlayerNetwork>().isLocal = true;
                    print("Le MasterClient a été instancié");
                    
                    int g =0;
                    int serumn = CreateRoomMenu.PlayerNumber-1;
                    if (serumn == 0)
                    {
                        serumn = 1;
                    }

                    print(g + " " + serumn + " " + (CreateRoomMenu.where.Length - 4));
                    print("Génération des sérums :");
                    while (g < serumn)
                    {
                        print(CreateRoomMenu.where[g + 4]);
                        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Serum"),
                            new Vector3((float) (4 * (CreateRoomMenu.where[g+4] % CreateRoomMenu.Xm) + 2.5), 2, (float) (4 * (CreateRoomMenu.where[g+4] / CreateRoomMenu.Xm) + 2.5)),
                            new Quaternion(0, 0, 0, 0));
                        g += 1;
                    }
                    print("Fin de génération des sérums");
                    while (g + 4 < (CreateRoomMenu.where.Length))
                    {
                        print(CreateRoomMenu.where[g + 4]);
                        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"),
                            new Vector3((float) (4 * (CreateRoomMenu.where[g+4] % CreateRoomMenu.Xm) + 2.5), 2, (float) (4 * (CreateRoomMenu.where[g+4] / CreateRoomMenu.Xm) + 2.5)),
                            new Quaternion(0, 0, 0, 0));
                        g += 1;
                    }
                    print("Fin de génération des Poxer-Up");
                    pv.RPC("CreateOtherPlayers", RpcTarget.Others);
                }
            }
        }

        [PunRPC]
        void CreateOtherPlayers()
        {
            Photon.Realtime.Player localplayer = PhotonNetwork.LocalPlayer;
            bool not_found = true;
            int i = 1;
            while (not_found && i < PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.PlayerList[i] == localplayer)
                {
                    not_found = false;
                }
                else
                {
                    i++;
                }
            }
            int coorp = CreateRoomMenu.where[i];
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Attente"), new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), (float) 3.75, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)), Quaternion.identity, 0);
            GameObject player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), 
                new Vector3((float) (4 * (coorp % CreateRoomMenu.Xm) + 2.5), 3, (float) (4 * (coorp / CreateRoomMenu.Xm) + 2.5)),
                Quaternion.identity, 0);
            player.GetComponent<PlayerNetwork>().isLocal = true;
            print("Le joueur a été instancié");
        }
    }
}

