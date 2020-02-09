using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ds
{
    public class PlayerListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Transform _content;
        [SerializeField]
        private PlayerListing _playerListing;

        private List<PlayerListing> _listings = new List<PlayerListing>();

        public override void OnEnable()
        {
            base.OnEnable();
            GetCurrentRoomPlayers();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            for (int i = 0; i < _listings.Count; i++)
                Destroy(_listings[i].gameObject);

            _listings.Clear();
        }

        private void GetCurrentRoomPlayers()
        {
            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerListing(playerInfo.Value);
            }
        }

        private void AddPlayerListing(Player player)
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayerListing(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            int index = _listings.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
            {
                Destroy((_listings[index].gameObject));
                _listings.RemoveAt(index);
            }
        }
    }
}