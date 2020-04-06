using System.Collections;
using System.Collections.Generic;
using ds;
using Photon.Pun;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomCanvases _roomCanvases;
    public void FirstInitiliaze(RoomCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    
    public void OnClic_LeaveRoom()
    {
        Audio.Play("click");

        PhotonNetwork.LeaveRoom(true);
        _roomCanvases.CurrentRoomCanvas.Hide();
    }
}
