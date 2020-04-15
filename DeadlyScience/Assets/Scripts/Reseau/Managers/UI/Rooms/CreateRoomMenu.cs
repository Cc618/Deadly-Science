using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ds
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Text _roomName;
        public static int Xm;
        public static int Zm;
        public static int[] where;
        public static CreateRoomMenu instance;

        private RoomCanvases _roomCanvases;

        void Awake()
        {
            instance = this;
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
            //TODO : Permettre au créateur de modifier Xm et Zm
            Xm = 3;
            Zm = 10;
            where = Generation.Aleatoire(7, Xm * Zm);
            int g = 0;
            while (g < 3)
            {
                var newCube = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Serum"),
                    new Vector3((float) (4 * (where[g+4] % Xm) + 2.5), 2, (float) (4 * (where[g+4] / Xm) + 2.5)),
                    new Quaternion(0, 0, 0, 0));
                g += 1;
            }
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