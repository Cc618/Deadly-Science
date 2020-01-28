using System;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        print("Se connecte au serveur...");
        PhotonNetwork.ConnectUsingSettings();

    }


    public override void OnConnectedToMaster()
    {
        print("Connecté au master.");
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        print("A joint le lobby.");
    }
}
