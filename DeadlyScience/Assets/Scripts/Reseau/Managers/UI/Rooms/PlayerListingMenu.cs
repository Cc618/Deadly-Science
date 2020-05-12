using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Transform _content;
        [SerializeField]
        private PlayerListing _playerListing;
        [SerializeField] 
        private Text _readyUpText;

        private List<PlayerListing> _listings = new List<PlayerListing>();
        private RoomCanvases _roomCanvases;
        public bool _ready = false;

        public override void OnEnable()
        {
            base.OnEnable();
            SetReadyUp(false);
            GetCurrentRoomPlayers();
        }

        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        private void SetReadyUp(bool state)
        {
            _ready = state;
            if (_ready)
                _readyUpText.text = "R";
            else
                _readyUpText.text = "N";
        }

        public override void OnDisable()
        {
            base.OnDisable();
            for (int i = 0; i < _listings.Count; i++)
                Destroy(_listings[i].gameObject);

            _listings.Clear();
        }
        
        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            _roomCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClic_LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            AddPlayerListing(newPlayer);
            photonView.RPC("RPC_ChangeReadyState", RpcTarget.All,
                PhotonNetwork.LocalPlayer, _ready);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            int index = _listings.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
            {
                Destroy(_listings[index].gameObject);
                _listings.RemoveAt(index);
            }
        }

        public void OnClick_StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SendVariableUpdate();
                for (int i = 0; i < _listings.Count; i++)
                {
                    if (_listings[i].Player != PhotonNetwork.LocalPlayer)
                    {
                        if (!_listings[i].Ready)
                            return;
                    }
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(2);
            }
        }
        
        private void GetCurrentRoomPlayers()
        {
            if (!PhotonNetwork.IsConnected)
                return;
            if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
                return;
            
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                AddPlayerListing(player);
            }
        }

        private void AddPlayerListing(Photon.Realtime.Player player)
        {
            int index = _listings.FindIndex(x => x.Player == player);
            if (index != -1)
            {
                _listings[index].SetPlayerInfo(player);
            }
            else
            {
                PlayerListing listing = Instantiate(_playerListing, _content);
                if (listing != null)
                {
                    listing.SetPlayerInfo(player);
                    _listings.Add(listing);
                }
            }
        }

        public void OnClick_ReadyUp()
        {
            SetReadyUp(!_ready);
            photonView.RPC("RPC_ChangeReadyState", RpcTarget.All,
                PhotonNetwork.LocalPlayer, _ready);
        }
        
        [PunRPC]
        private void RPC_ChangeReadyState(Photon.Realtime.Player player, bool ready)
        {
            int index = _listings.FindIndex(x => Equals(x.Player,  player));
            if (index != -1)
            {
                _listings[index].Ready = ready;
            }
            AddPlayerListing(player);
        }


        [PunRPC]
        public void VariableUpdate(int xm, int[] where)
        {
            CreateRoomMenu.where = where;
            CreateRoomMenu.Xm = xm;
        }

        public void SendVariableUpdate()
        {
            PhotonView.Get(this).RPC("VariableUpdate", RpcTarget.All, CreateRoomMenu.Xm,
                CreateRoomMenu.where);
        }
    }
}