using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public partial class PlayerState : MonoBehaviour
    {
        public enum PlayerStatus
        {
            INFECTED,
            HEALED,
            // TODO : GHOST for dead players (No collisions with others)
            // TODO : Revenge
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

                // TODO : Change
                if (player && player.nameUi)
                    player.nameUi.SetStatus(status);

                // Update material
                // TODO : Update also anim / sound...
                switch (status)
                {
                    case PlayerStatus.HEALED:
                        Debug.Log("Player has status HEALED");
                        //stateIndicator.material = healedMaterial;
                        break;
                    case PlayerStatus.INFECTED:
                        Debug.Log("Player has status INFECTED");
                        //stateIndicator.material = infectedMaterial;
                        break;
                }
            }

            get => status;
        }

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
            Debug.Log("PlayerState : OnSerum");
        }

        // Launches timer coroutines
        public void StartPhases()
        {
            StartCoroutine(SearchPhase());
        }

        // TODO : Useless ?
        // When all phases are elapsed
        void OnGameEnd()
        {
            Debug.Log("Player : Game end");
        }

        // TODO : Remove timer
        IEnumerator SearchPhase()
        {
            Debug.Log("Player : Search phase has begun");

            yield return new WaitForSeconds(searchTime * 60);

            Debug.Log("Player : Search phase ended");

            // TODO : Update status ...

            // Change phase
            StartCoroutine(RevengePhase());
        }

        IEnumerator RevengePhase()
        {
            Debug.Log("Player : Revenge phase has begun");

            yield return new WaitForSeconds(revengeTime * 60);

            Debug.Log("Player : Revenge phase ended");
        }
    }
}
