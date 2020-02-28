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
    }
}
