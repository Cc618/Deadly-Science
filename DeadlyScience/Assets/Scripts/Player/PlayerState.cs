using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    public partial class PlayerState : MonoBehaviour
    {
        public enum PlayerStatus
        {
            INFECTED,
            HEALED,
            REVENGE,
            GHOST
        }

        // In seconds TODO : 120
        public static float revengeTime = 1;

        private PlayerStatus status = PlayerStatus.INFECTED;
        public PlayerStatus Status
        {
            set
            {
                // Update status
                status = value;

                // Change the label's color
                if (nameUi)
                    nameUi.SetStatus(status);

                // TODO : Sound
            }

            get => status;
        }

        public PlayerName nameUi;
        public PlayerStamina staminaUi;

        public void StartAfterPlayerNetwork()
        {
            player = GetComponent<Player>();
            net = GetComponent<PlayerNetwork>();
        }

        private Player player;
        private PlayerNetwork net;
    }

    // This part is executed only if this script is owned
    // by the client
    public partial class PlayerState : MonoBehaviour
    {
        // When a player takes a serum
        public void OnSerum()
        {
            // !!! TODO : Can take serum only when infected
            net.SendSetStatus(PlayerStatus.HEALED);
        }

        public void BeginFirstPhase()
        {
        }

        public void EndFirstPhase()
        {
            // Revenge mode
            if (status == PlayerStatus.INFECTED)
                net.SendSetStatus(PlayerStatus.REVENGE);
        }

        // Status when we end the first phase
        PlayerStatus firstPhaseStatus;

        public void EndOfGame(bool forceWin=false, bool win=false)
        {
            if (forceWin)
                EndGame.EndOfGame(win);
            else
                EndGame.EndOfGame(Status == PlayerStatus.HEALED);
        }

        // Called on master
        public IEnumerator SecondPhase()
        {
            firstPhaseStatus = Status;

            yield return new WaitForSeconds(revengeTime);

            net.SendEndOfGame();
        }
    }
}
