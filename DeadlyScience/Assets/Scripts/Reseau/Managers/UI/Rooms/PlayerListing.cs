﻿using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerListing : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        public Player Player { get; private set; }
        public void SetPlayerInfo(Player player)
        {
            Player = player;
            _text.text = player.NickName;
        }
    }
}
