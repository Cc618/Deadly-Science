using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    // Instance part
    public partial class PlayerNetwork : MonoBehaviour
    {
        [HideInInspector]
        // Whether this player is handle by the client
        public bool isLocal;

        // The 'constructor' of the player
        // Call 'constructors' in other components
        void Start()
        {
            // The player is not controlled by the client
            if (!isLocal)
            {
                // Remove camera and behaviour
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(GetComponent<Player>());
                Destroy(GetComponent<PlayerMaster>());
                // TODO
                //Destroy(GetComponent<PlayerSlave>());

                // TODO : Set labels' camera (after all players have spawned)
            }
            else
            {
                // Remove labels
                Destroy(GetComponentInChildren<LookToCam>().gameObject);

                // Remove the master if necessary
                if (!PhotonNetwork.IsMasterClient)
                    Destroy(GetComponent<PlayerMaster>());

                // Start player component
                var p = GetComponent<Player>();
                p.net = this;
                p.StartAfterPlayerNetwork();
            }

            // Start phases
            playerState = GetComponent<PlayerState>();
            playerState.StartAfterPlayerNetwork();

            // Append this player to the players in game list
            RegisterPlayer(gameObject);
        }

        // Called when all players are in game before OnGameBegin
        public void PrepareGame()
        {
            // TODO : Even if there is only one player ?
            // Update the number of serums in game
            if (PhotonNetwork.PlayerList.Length == 1)
                PlayerMaster.serumCount = 2;
            else
                PlayerMaster.serumCount = PhotonNetwork.PlayerList.Length - 1;

            // TODO : Begin game...
            if (isLocal)
                playerState.BeginFirstPhase();
        }

        private PlayerState playerState;
    }

    // Static part
    public partial class PlayerNetwork : MonoBehaviour
    {
        // The player controlled by the client
        public static Player localPlayer;

        // List of all players
        private static List<GameObject> players = new List<GameObject>();
        public static List<GameObject> Players { get => players; }

        // Registers a new player in the players list
        public static void RegisterPlayer(GameObject p)
        {
            players.Add(p);

            if (players.Count == PhotonNetwork.PlayerList.Length)
                OnAllPlayersInGame();
        }
        
        // All phases are elapsed
        public static void OnGameEnd()
        {
            Debug.Log("PlayerNetwork : Game end");
        }

        // When all players are in game
        static void OnAllPlayersInGame()
        {
            Debug.Log("PlayerNetwork : All players in game");

            foreach (var player in players)
                player.GetComponent<PlayerNetwork>().PrepareGame();

            localPlayer.OnGameBegin();
        }
    }
}
