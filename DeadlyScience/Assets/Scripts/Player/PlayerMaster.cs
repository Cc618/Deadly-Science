// Executed when the player is the master player
// Handle every players and the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    public class PlayerMaster : MonoBehaviour
    {
        // TODO : For multiple games, reset values
        public static PlayerMaster instance;

        private void Awake()
        {
            instance = this;
            net = GetComponent<PlayerNetwork>();
        }

        private int playersReady = 0;
        public int PlayersReady
        {
            set
            {
                ++playersReady;

                // TMP
                print($"{playersReady} ready");

                if (playersReady >= PlayerNetwork.Players.Count)
                {
                    net.SendFirstPhase();
                }
            }
            get => playersReady;
        }

        private static int collectedSerums = 0;
        public static int serumCount = -1;
        public static int CollectedSerums
        {
            set
            {
                collectedSerums = value;

                Debug.Log(collectedSerums.ToString() + " / " + serumCount.ToString() + " collected serums");

                // Change phase for all players
                if (collectedSerums == serumCount)
                {
                    // Change phase remotely
                    instance.net.SendSecondPhase();
                }
            }
            get => collectedSerums;
        }

        private PlayerNetwork net;
    }
}
