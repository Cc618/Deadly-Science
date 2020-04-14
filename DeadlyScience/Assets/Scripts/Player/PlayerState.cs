﻿using System.Collections;
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
        }        

        private void Update()
        {
            // TODO : test
            if (Input.GetKeyDown(KeyCode.Mouse0))
                if (player)
                    player.Hit();

            // TODO : test
            // Toggle infected
            if (Input.GetKeyDown(KeyCode.K) && player)
                Status = PlayerStatus.INFECTED;
        }

        private Player player;
    }

    // This part is executed only if this script is owned
    // by the client
    public partial class PlayerState : MonoBehaviour
    {
        // When a player takes a serum
        public void OnSerum()
        {
            // TODO : Remote
            // if (PhotonNetwork.IsMasterClient)
                //++PlayerMaster.CollectedSerums;
        }

        public void BeginFirstPhase()
        {
            Debug.Log("PlayerState : 1st phase has begun");
        }

        public void EndFirstPhase()
        {
            Debug.Log("PlayerState : 1st phase ended");

            // TODO : Update status ...

            // Start next phase
            StartCoroutine(SecondPhase());
        }

        IEnumerator SecondPhase()
        {
            PlayerStatus startSecondPhase = Status;
            Debug.Log("PlayerState : 2nd phase has begun");

            yield return new WaitForSeconds(5);//revengeTime * 60);

            Debug.Log("PlayerState : 2nd phase ended");

            
            if (startSecondPhase != Status)
            {
                EndGame.EndOfGame(false);
            }
            else
            {
                EndGame.EndOfGame(true);
            }

      
            PlayerNetwork.OnGameEnd();
        }
    }
}
