using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

namespace ds
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Text _roomName;
        [SerializeField]
        private InputField inputfield;
        public static int Xm = 5;
        public static int Zm = 5;
        public static int PlayerNumber;
        public static int[] where;
        public static CreateRoomMenu instance;

        private RoomCanvases _roomCanvases;

        void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsConnected)
            {
                if (_roomName.text.Length == 0)
                {
                    inputfield.text = "Test" + Random.Range(0, 9999);
                }
                OnClick_CreateRoom();
            }
        }

        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public void OnClick_CreateRoom()
        {
            Audio.Play("click");
            if (!PhotonNetwork.IsConnected)
                return;
            RoomOptions options = new RoomOptions();
            
            //Partie Leandre
            PlayerNumber = 2;
            //TODO : Permettre au créateur de modifier Xm et Zm
<<<<<<< HEAD
=======
            Xm = 5;
            Zm = 10;
>>>>>>> 3d79733be3e7e7e99ee5fa766d0cf55efd8c5599
            //TODO : Déterminer le Ratio de Power-Ups
            int max = (Xm * Zm) / 25;
            //print(max);
            if (max < PlayerNumber)
            {
                max = PlayerNumber-1;
            }

            if (PlayerNumber == 1)
            {
                max = 1;
            }
            //print(max);
            where = Generation.Aleatoire(4+max, Xm * Zm);
            int a = 0;
            while (a < max+4)
            {
                //print(where[a]);
                a += 1;
            }
            //print("Retiré :");
            a = 4;
            while (a != PlayerNumber)
            {
                a -= 1;
                print(where[a]);
                where[a] = where[0];
            }
            //Fin de partie Leandre
            
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 4;
            options.PublishUserId = true;
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            print("La salle a bien pu etre creee.");
            _roomCanvases.CurrentRoomCanvas.Show();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            print("La creation de la salle a ratee." + message);
        }
    }
}