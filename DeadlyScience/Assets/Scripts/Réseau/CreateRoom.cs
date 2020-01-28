using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    private Text _roomName;

    private Text RoomName
    {
        get { return _roomName;  }
    }

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomName.text);
        print("La demande de création de salle a bien été envoyé");
    }




    public override void OnCreatedRoom()
    {
        print("La salle a bien pu être crée.");
    }
}
