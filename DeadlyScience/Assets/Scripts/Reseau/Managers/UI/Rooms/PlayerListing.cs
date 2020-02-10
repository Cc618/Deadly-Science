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
            int result = -1;
            if (player.CustomProperties.ContainsKey("RandomNumber"))
                result = (int)player.CustomProperties["RandomNumber"];
            _text.text = result.ToString() + ", " + player.NickName;
        }
    }
}
