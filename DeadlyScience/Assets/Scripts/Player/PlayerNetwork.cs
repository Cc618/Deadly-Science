using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    // Instance part
    public partial class PlayerNetwork : MonoBehaviour, IPunInstantiateMagicCallback
    {
        // The PlayerNetwork of the player controlled by the client
        [HideInInspector]
        public static PlayerNetwork local;

        // To recognize the player with network
        // Also used to decide which player send callbacks (priority)
        // * Unique for each player
        [HideInInspector]
        public int id;

        [HideInInspector]
        // Whether this player is handle by the client
        public bool isLocal;

        // The 'constructor' of the player
        // Call 'constructors' in other components
        void Start()
        {
            view = PhotonView.Get(this);

            playerState = GetComponent<PlayerState>();
            playerState.StartAfterPlayerNetwork();
            
            // The player is not controlled by the client
            if (!isLocal)
            {
                // Remove camera and behaviour
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(GetComponent<Player>());
                Destroy(GetComponent<PlayerMaster>());
                Destroy(GetComponent<PlayerSlave>());
            }
            else
            {
                local = this;

                // Remove labels
                Destroy(GetComponentInChildren<LookToCam>().gameObject);

                // Remove the master / slave if necessary
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayerMaster.instance = GetComponent<PlayerMaster>();
                    Destroy(GetComponent<PlayerSlave>());
                }
                else
                    Destroy(GetComponent<PlayerMaster>());

                // Start player component
                var p = GetComponent<Player>();
                p.net = this;
                GetComponent<Player>().StartAfterPlayerNetwork();
            }

            // Append this player to the players in game list
            RegisterPlayer(gameObject);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            id = info.Sender.ActorNumber;
        }

        // Called when all players are in game before OnGameBegin
        public void PrepareGame()
        {
            // TODO : Even if there is only one player ?
            // Update the number of serums in game
            if (PhotonNetwork.PlayerList.Length == 1)
                PlayerMaster.serumCount = 1;
            else
                PlayerMaster.serumCount = PhotonNetwork.PlayerList.Length - 1;

            // TODO : Begin game...
            if (isLocal)
                playerState.BeginFirstPhase();
        }

        // Whether the priority is higher for this player
        public bool HasPriority(PlayerNetwork player)
        {
            // < because we want that the master has always the priority
            return id < player.id;
        }

        private PlayerState playerState;
        private PhotonView view;
    }

    // Static part
    public partial class PlayerNetwork : MonoBehaviour
    {
        // The player controlled by the client
        public static Player localPlayer;

        // List of all players
        private static List<GameObject> players = new List<GameObject>();
        public static List<GameObject> Players { get => players; }

        // TODO : rm
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
        }

        // When all players are in game
        static void OnAllPlayersInGame()
        {
            foreach (var player in players)
                player.GetComponent<PlayerNetwork>().PrepareGame();
        }
    }

    // Network events part
    public partial class PlayerNetwork : MonoBehaviour
    {
        // Items //
        // A serum has been collected
        [PunRPC]
        public void OnSerum(int from, int serumId)
        {
            if (from == id)
                playerState.OnSerum();

            // Find and destroy the serum
            if (PhotonNetwork.IsMasterClient)
            {
                Serum serum = Serum.instances.Find((Serum s) => s.id == serumId);

                PhotonNetwork.Destroy(serum.GetComponent<PhotonView>());

                ++PlayerMaster.CollectedSerums;
            }
        }

        // Send to each player OnSerum events
        // Serum id serves to destroy the serum
        public void SendOnSerum(int serumId)
        {
            // This event triggers PlayerNetwork.OnSerum
            view.RPC("OnSerum", RpcTarget.All, id, serumId);
        }

        // Game play //
        // A player changes status
        [PunRPC]
        public void SetStatus(int from, PlayerState.PlayerStatus status)
        {
            // Change for the target player only
            if (from == id)
                playerState.Status = status;
        }

        public void SendSetStatus(PlayerState.PlayerStatus status)
        {
            view.RPC("SetStatus", RpcTarget.All, id, status);
        }

        // Called every third of second
        [PunRPC]
        public void SyncNet(int from, float stamina, bool stunned)
        {
            if (!isLocal && from == id)
            {
                playerState.staminaUi.Value = stamina;
                playerState.staminaUi.ChangeStunned(stunned);
            }
        }

        public void SendSyncNet(float stamina, bool stunned)
        {
            view.RPC("SyncNet", RpcTarget.All, id, stamina, stunned);
        }

        // Phases //
        // To say that a player is ready
        [PunRPC]
        public void PlayerReady(int from)
        {
            ++PlayerMaster.instance.PlayersReady;
        }

        public void SendPlayerReady()
        {
            view.RPC("PlayerReady", RpcTarget.MasterClient, id);
        }

        // First phase, after game init
        [PunRPC]
        public void FirstPhase()
        {
            if (isLocal)
                GetComponent<Player>().OnGameBegin();
        }

        public void SendFirstPhase()
        {
            view.RPC("FirstPhase", RpcTarget.All);
        }

        [PunRPC]
        public void SecondPhase()
        {
            if (isLocal)
            {
                playerState.EndFirstPhase();
            }
        }

        public void SendSecondPhase()
        {
            view.RPC("SecondPhase", RpcTarget.All);
        }

        // First phase, after game init
        // This method is not always called
        // from the client player net so
        // we use local instead of this
        [PunRPC]
        public void EndOfGame()
        {
            local.playerState.EndOfGame();
        }

        public void SendEndOfGame()
        {
            view.RPC("EndOfGame", RpcTarget.All);
        }
    }
}
