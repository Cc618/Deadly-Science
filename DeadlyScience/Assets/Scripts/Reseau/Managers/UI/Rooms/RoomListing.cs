using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        public RoomInfo RoomInfo { get; private set; }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;
            _text.text = roomInfo.Name + ", " + roomInfo.PlayerCount;
        }

        public void OnClick_Button()
        {
            Audio.Play("click");
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        }
    }
}
