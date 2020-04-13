using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace ds
{
    public class PlayerListing : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Text _text;

        public Photon.Realtime.Player Player { get; private set; }
        public bool Ready = false;
        public void SetPlayerInfo(Photon.Realtime.Player player)
        {
            Player = player;
            SetPlayerText(player);

            // Set the unique id of this player
            PlayerNetwork.localId = Player.ActorNumber;
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(target, changedProps);
            if (target != null && target == Player)
            {
                if (changedProps.ContainsKey("RandomNumber"))
                    SetPlayerText(target);
            }
        }

        private void SetPlayerText(Photon.Realtime.Player player)
        {
            /*bool ready = GetComponent<PlayerListingMenu>()._ready;
            string is_ready = "";
            if (ready)
            {
                is_ready += "R ";
            }
            else
            {
                is_ready += "N ";
            }*/
                _text.text = player.NickName;
        }
    }
}
