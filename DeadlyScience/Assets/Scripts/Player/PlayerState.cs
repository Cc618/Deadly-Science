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

        // In minutes
        [Range(0, 5)]
        public float searchTime;
        [Range(0, 5)]
        public float revengeTime;

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

                // Update material
                // TODO : Update also anim / sound...
                Debug.Log($"Player : New status -> {value}");
                //switch (status)
                //{
                //    case PlayerStatus.HEALED:
                //        Debug.Log("Player has status HEALED");
                //        //stateIndicator.material = healedMaterial;
                //        break;
                //    case PlayerStatus.INFECTED:
                //        Debug.Log("Player has status INFECTED");
                //        //stateIndicator.material = infectedMaterial;
                //        break;
                //}
            }

            get => status;
        }

        public PlayerName nameUi;
        public PlayerStamina staminaUi;

        public void StartAfterPlayerNetwork()
        {
            player = GetComponent<Player>();
            net = GetComponent<PlayerNetwork>();

            print($"TMP : STATE : start");
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

            // Start next phase
            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(SecondPhase());
        }

        // Status when we end the first phase
        PlayerStatus firstPhaseStatus;

        public void EndOfGame()
        {
            // TODO : Revenge can win ?
            if (Status == PlayerStatus.HEALED)
            {
                EndGame.EndOfGame(true);
            }
            else
            {
                EndGame.EndOfGame(false);
            }
        }

        // Called on master
        IEnumerator SecondPhase()
        {
            // TMP

            print("MASTER : Second phase 1");

            firstPhaseStatus = Status;

            yield return new WaitForSeconds(2);// TODO : revengeTime * 60);
            
            print("MASTER : Second phase 2");

            net.SendEndOfGame();
        }
    }
}
