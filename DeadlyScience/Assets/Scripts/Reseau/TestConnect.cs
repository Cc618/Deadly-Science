using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ds
{
    public class TestConnect : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            print("Se connecte au serveur...");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            print("Est connecté au serveur.");
            print(PhotonNetwork.LocalPlayer.NickName);
            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            print("Deconnecté du serveur à cause de " + cause);
        }
    }
}
