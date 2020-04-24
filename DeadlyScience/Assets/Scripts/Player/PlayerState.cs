﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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

        // In seconds
        public static int revengeTime = 60 * 3;

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
            net.SendSetStatus(PlayerStatus.HEALED);
        }

        public void BeginFirstPhase()
        {
            AffichagePhase.phase=1;
        }

        public void EndFirstPhase()
        {
            string s = "Echappez au Condamné !\n";
            // Revenge mode
            if (status == PlayerStatus.INFECTED)
            {
                net.SendSetStatus(PlayerStatus.REVENGE);
                s = "Infectez les Joueurs Guéris !\n";
            }
            AffichagePhase.Objectif = s;
            AffichagePhase.phase=2;
            AffichagePhase.temps = revengeTime;
        }

        // Status when we end the first phase
        PlayerStatus firstPhaseStatus;

        public void EndOfGame(bool forceWin=false, bool win=false)
        {
            AffichagePhase.phase = 3;
            if (forceWin)
                EndGame.EndOfGame(win);
            else
                EndGame.EndOfGame(Status == PlayerStatus.HEALED);
        }

        // Called on master
        public IEnumerator SecondPhase()
        {

            if (PhotonNetwork.IsMasterClient)
                firstPhaseStatus = Status;

            WaitForSecondsRealtime countTime = new WaitForSecondsRealtime(1);
            int startTime = (int) revengeTime;
            while (startTime > 0)
            {
                startTime -= 1;
                AffichagePhase.temps = startTime;
                yield return countTime;
            }
            AffichagePhase.phase = 3;
            if (PhotonNetwork.IsMasterClient)
                net.SendEndOfGame();
        }
    }
}
