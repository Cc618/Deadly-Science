using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class CurrentRoomCanvas : MonoBehaviour
    {
        private RoomCanvases _roomCanvases;
        public void FirstInitialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
