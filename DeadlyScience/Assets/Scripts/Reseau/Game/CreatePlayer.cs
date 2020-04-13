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
        private Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;


        void Start()
        {
            int[] tableau = new int[4];
            int i = 0;
            int id = player.ActorNumber;
            //Determine which position to spawn each player
            foreach (var roomPlayer in PhotonNetwork.CurrentRoom.Players)
            {
                tableau[i] = roomPlayer.Value.ActorNumber;
                i++;
            }
            
            print(id);
            print("BLANK");
            print(tableau[0]);
            print(tableau[1]);

            if (tableau[0] == id)
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 6), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (tableau[1] == id)
            {
                var _player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(-5, 5, 4), Quaternion.identity, 0);
                _player.GetComponent<PlayerNetwork>().isLocal = true;
            }
            else if (tableau[2] == id)
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

