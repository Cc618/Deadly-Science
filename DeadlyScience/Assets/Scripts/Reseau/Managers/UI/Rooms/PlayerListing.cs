using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace ds
{
    public class PlayerListing : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TMP_Text _text;

        public Photon.Realtime.Player Player { get; private set; }
        public bool Ready = false;
        public void SetPlayerInfo(Photon.Realtime.Player player)
        {
            Player = player;
            SetPlayerText(player);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(target, changedProps);
            if (target != null && target == Player)
            {
                if (changedProps.ContainsKey("RandomNumber"))
                    SetPlayerText(target);
            }
        }

        public void SetPlayerText(Photon.Realtime.Player player)
        {
            int lang = PlayerPrefs.GetInt("language");
            if (player == PhotonNetwork.MasterClient)
            {
                if(lang == 1)
                    _text.text = "Hôte " + player.NickName;
                else
                    _text.text = "Host " + player.NickName;
            }
            else
            {
                if (Ready)
                {
                    if (lang == 1)
                        _text.text = "Prêt " + player.NickName;
                    else
                        _text.text = "Ready " + player.NickName;
                }
                else
                {
                    if (lang == 1)
                        _text.text = "Non Prêt " + player.NickName;
                    else
                        _text.text = "Not Ready " + player.NickName;
                }
            }
        }
    }
}
