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

                if (playersReady >= PlayerNetwork.Players.Count)
                {
                    net.SendFirstPhase();
                }
            }
            get => playersReady;
        }

        private static int collectedSerums = 0;
        public static int serumCount = 3;
        public static int CollectedSerums
        {
            set
            {
                collectedSerums = value;

                // Change phase for all players
                if (collectedSerums == serumCount)
                {
                    // Change phase remotely
                    instance.net.SendSecondPhase();
                }
            }
            get => collectedSerums;
        }

        private static int revengePlayers = 0;
        private static int firstRevengePlayer = -1;
        public static void UpdateRevengePlayers(int id)
        {
            ++revengePlayers;

            if (revengePlayers == 1)
                firstRevengePlayer = id;

            if (revengePlayers == PhotonNetwork.PlayerList.Length)
            {
                // TODO : Only the first revenge win
                instance.net.SendRevengeWin(firstRevengePlayer);
            }
        }

        private PlayerNetwork net;
    }
}
