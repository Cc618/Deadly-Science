using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Random = UnityEngine.Random;

namespace ds
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TMP_InputField inputfield;
        [SerializeField] 
        private GameObject labsettings;
        [SerializeField] 
        private GameObject roommenu;
        public static int Xm = 10;
        public static int Zm = 10;
        public static int PlayerNumber = 4;
        public static int[] where;
        public static int Mode = 0;
        public static CreateRoomMenu instance;

        private RoomCanvases _roomCanvases;

        void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && PhotonNetwork.IsConnected && !labsettings.activeSelf
                && !roommenu.activeSelf)
            {
                if (inputfield.text.Length == 0)
                    inputfield.text = "Salle" + Random.Range(0, 9999);
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
            //Partie Leandre
            //TODO : Déterminer le Ratio de Power-Ups
            int max = Xm * Zm / 25;
            //print(max);
            if (max < PlayerNumber)
                max = PlayerNumber - 1;
            if (PlayerNumber == 1)
                max = 1;
            //print(max);
            where = Generation.Aleatoire(4+max, Xm * Zm);
            int a = 0;
            while (a < max + 4)
            {
                //print(where[a]);
                a++;
            }
            //print("Retiré :");
            a = 4;
            while (a != PlayerNumber)
            {
                a--;
                //print(where[a]);
                where[a] = where[0];
            }
            //Fin de partie Leandre
            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = (byte)PlayerNumber;
            options.PublishUserId = true;
            PhotonNetwork.JoinOrCreateRoom(inputfield.text, options, TypedLobby.Default);
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