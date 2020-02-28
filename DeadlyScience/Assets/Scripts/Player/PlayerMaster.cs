// Executed when the player is the master player
// Handle every players and the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    // Instance part
    public partial class PlayerMaster : MonoBehaviour
    {
        // TODO
    }

    // Static part
    public partial class PlayerMaster : MonoBehaviour
    {
        private static int collectedSerums = 0;
        public static int serumCount;
        public static int CollectedSerums
        {
            set
            {
                collectedSerums = value;

                // Change phase for all players
                if (collectedSerums == serumCount)
                {
                    // TODO : Change phase remotely

                    PlayerNetwork.localPlayer.GetComponent<PlayerState>().EndFirstPhase();    
                }
            }
            get => collectedSerums;
        }

        // Send to each player OnSerum events
        // !!! Check before that the player is the master
        public static void SendOnSerum()
        {
            // TODO : STEVE : Send event
        }

        // Send to each player OnPhaseEnd events
        public static void SendOnPhaseEnd(bool firstPhase)
        {
            // TODO : STEVE : Send event
        }
    }
}
