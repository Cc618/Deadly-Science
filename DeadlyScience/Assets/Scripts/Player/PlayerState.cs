using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace ds
{
    public partial class PlayerState : MonoBehaviour
    {
        // !!! Do NOT change the position of each entry
        public enum PlayerStatus
        {
            INFECTED,
            HEALED,
            REVENGE,
            GHOST
        }

        public static readonly string[] STATUS_STR = new[] { "infected", "healed", "revenge", "ghost"};

        // In seconds
        public static int revengeTime = 60 * 3;

        private PlayerStatus status = PlayerStatus.INFECTED;
        public PlayerStatus Status
        {
            set
            {
                // Post processing
                if (player)
                {
                    if (status != PlayerStatus.REVENGE && value == PlayerStatus.REVENGE)
                        GetComponentInChildren<PlayerCam>().revengeFxEnabled = true;
                    else if (status == PlayerStatus.REVENGE && value != PlayerStatus.REVENGE)
                        GetComponentInChildren<PlayerCam>().revengeFxEnabled = false;
                }
                else
                {
                    bool wasGhost = status == PlayerStatus.GHOST;
                    bool isGhost = value == PlayerStatus.GHOST;

                    // Disable / enable render for ghost
                    if (wasGhost && !isGhost)
                    {
                        GetComponentInChildren<Renderer>().enabled = true;
                        labels.SetActive(true);
                    }
                    else if (isGhost && !wasGhost)
                    {
                        GetComponentInChildren<Renderer>().enabled = false;
                        labels.SetActive(false);
                    }
                }

                // Update status
                status = value;

                // Change the label's color
                if (nameUi)
                    nameUi.SetStatus(status);

                // FX
                Particles.Spawn(STATUS_STR[(int)status], transform.position + new Vector3(0, 1.5f, 0));
            }

            get => status;
        }

        public PlayerName nameUi;
        public PlayerStamina staminaUi;

        private void Start()
        {
            // Get parent
            labels = nameUi.transform.parent.gameObject;
        }

        public void StartAfterPlayerNetwork()
        {
            player = GetComponent<Player>();
            net = GetComponent<PlayerNetwork>();
        }

        private Player player;
        private PlayerNetwork net;
        private GameObject labels;
    }

    // This part is executed only if this script is owned
    // by the client
    public partial class PlayerState : MonoBehaviour
    {
        // When a player takes a serum
        public void OnSerum()
        {
            net.SendSetStatus(PlayerStatus.HEALED);
            EndGame.AddRecap("Soigné");
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
                EndGame.AddRecap("Corrompu");
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
                EndGame.EndOfGame(Status != PlayerStatus.REVENGE);
        }

        public static int startTime;

        // Called on master
        public IEnumerator SecondPhase()
        {
            if (PhotonNetwork.IsMasterClient)
                firstPhaseStatus = Status;

            WaitForSecondsRealtime countTime = new WaitForSecondsRealtime(1);
            startTime = (int) revengeTime;
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
