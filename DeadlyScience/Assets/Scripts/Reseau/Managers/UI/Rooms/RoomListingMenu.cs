using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ds
{
    public class RoomListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Transform _content;
        [SerializeField]
        private RoomListing _roomListing;

        private List<RoomListing> _listings = new List<RoomListing>();
        private RoomCanvases _roomCanvases;

        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public override void OnJoinedRoom()
        {
            _roomCanvases.CurrentRoomCanvas.Show();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                //A été enlevé de RoomList
                if (info.RemovedFromList)
                {
                    int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                    if (index != -1)
                    {
                        Destroy((_listings[index].gameObject));
                        _listings.RemoveAt(index);
                    }
                }
                //A été ajouté à la RoomList
                else
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
            }
        }
    }
}
