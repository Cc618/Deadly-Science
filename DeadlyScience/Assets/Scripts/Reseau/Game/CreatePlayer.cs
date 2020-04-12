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
        private PhotonView PV;
        private Photon.Realtime.Player _player = PhotonNetwork.LocalPlayer;


        void Start()
        {
            string[] tableau = new string[4];
            int i = 0;
            //Determine which position to spawn each player
            foreach (var roomPlayer in PhotonNetwork.CurrentRoom.Players)
            {
                tableau[i] = roomPlayer.Value.NickName;
                i++;
            }
            print(_player.NickName);
            print("BLANK");
            print(tableau[0]);
            print(tableau[1]);

            if (tableau[0] == _player.NickName)
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 6), Quaternion.identity, 0);
                player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (tableau[1] == _player.NickName)
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 4), Quaternion.identity, 0);
                player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (tableau[2] == _player.NickName)
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 2), Quaternion.identity, 0);
                player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else
            {
                var player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 0), Quaternion.identity, 0);
                player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            

            // TODO : LEANDRE : Position

            print("Le joueur a été instancié");
        }
    }
}

