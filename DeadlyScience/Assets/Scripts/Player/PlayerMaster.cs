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
        public static PlayerMaster instance;

        void Awake()
        {
            net = GetComponent<PlayerNetwork>();
        }

        private int playersReady = 0;
        public void NewPlayerReady()
        {
            ++playersReady;

            if (playersReady == CreateRoomMenu.PlayerNumber)
                net.SendFirstPhase();
        }

        private static int collectedSerums = 0;
        public static int serumCount = CreateRoomMenu.PlayerNumber-1;
        public static int CollectedSerums
        {
            set
            {
                collectedSerums = value;

                // Change phase for all players
                if (collectedSerums ==  CreateRoomMenu.PlayerNumber-1)
                {
                    /* Change phase remotely
                    foreach (GameObject light in Generation.lights)
                    {
                        PhotonNetwork.Destroy(light);
                    }*/
                    instance.net.SendSecondPhase();
                }
                else
                {
                    if (collectedSerums >  CreateRoomMenu.PlayerNumber-1)
                    {
                        EndGame.EndOfGame(true);
                    }
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
                instance.net.SendRevengeWin(firstRevengePlayer);
            }
        }

        private PlayerNetwork net;
    }
}
