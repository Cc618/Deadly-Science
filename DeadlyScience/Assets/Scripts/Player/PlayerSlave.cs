// Active when the player is the client but not the master
// Receive network callbacks

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    public class PlayerSlave : MonoBehaviour
    {
        // A phase ended
        public void OnPhaseEnd(bool firstPhase)
        {
            if (firstPhase)
                Debug.Log("PlayerSlave : OnPhaseEnd (first)");
            else
                Debug.Log("PlayerSlave : OnPhaseEnd (second)");
        }
    }
}
