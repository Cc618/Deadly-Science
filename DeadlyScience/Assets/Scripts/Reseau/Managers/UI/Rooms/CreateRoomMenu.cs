using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class CreateRoomMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Text _roomName;

        private RoomCanvases _roomCanvases;
        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public void OnClick_CreateRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            RoomOptions options = new RoomOptions();
            options.BroadcastPropsChangeToAll = true;
            options.MaxPlayers = 4;
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
