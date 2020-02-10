using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class CurrentRoomCanvas : MonoBehaviour
    {
        [SerializeField] 
        private PlayerListingMenu _playerListingMenu;

        [SerializeField] 
        private LeaveRoomMenu _leaveRoomMenu;
        public LeaveRoomMenu LeaveRoomMenu { get { return _leaveRoomMenu; } }
            
        private RoomCanvases _roomCanvases;
        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
            _playerListingMenu.FirstInitialize(canvases);
            _leaveRoomMenu.FirstInitiliaze(canvases);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
