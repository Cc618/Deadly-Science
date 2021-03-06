﻿using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro.Examples;
using System.IO;
using Photon.Realtime;
using UnityEngine.UI;

namespace ds
{
    // PIPELINE
    // OnPhotonInstantiate appelee quand le joueur spawn
    // Apres Start est appelee et initialise les autres composants
    // Ensuite ces fonctions principales s'appelent
    // PrepareGame -> OnGameBegin -> FirstPhase -> SecondPhase -> RevengeWin
    // Les autres fonctions sont appelees au cours de la partie

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
                Destroy(GameObject.Find("Camera a detruire"));
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
            
            GetComponentInChildren<PlayerName>().GetComponent<Text>().text = info.Sender.NickName;
        }

        // Called when all players are in game before OnGameBegin
        public void PrepareGame()
        {
            if (isLocal)
                playerState.BeginFirstPhase();
        }

        // Whether the priority is higher for this player
        public bool HasPriority(PlayerNetwork player)
        {
            // < because we want that the master has always the priority
            return id < player.id;
        }

        public PlayerState playerState;
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

        // Registers a new player in the players list
        public static void RegisterPlayer(GameObject p)
        {
            players.Add(p);

            if (players.Count == PhotonNetwork.PlayerList.Length)
                OnAllPlayersInGame();
        }

        // All phases are elapsed
        public static void OnGameEnd()
        {}

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
            Audio.Play(from == localPlayer.net.id ? "serum" : "serum_long");
            if (from == id)
            {
                playerState.OnSerum();
            }
            // Find and destroy the serum
            if (PhotonNetwork.IsMasterClient)
            {
                Serum serum = Serum.instances.Find((Serum s) => s.id == serumId);
                ++PlayerMaster.CollectedSerums;
                PhotonNetwork.Destroy(serum.GetComponent<PhotonView>());
            }
        }

        // Send to each player OnSerum events
        // Serum id serves to destroy the serum
        public void SendOnSerum(int serumId)
        {
            // This event triggers PlayerNetwork.OnSerum
            view.RPC("OnSerum", RpcTarget.All, id, serumId);
        }

        public void PowerUpPris(int Id)
        {
            view.RPC("OtherPowerUp", RpcTarget.All, Id);
        }
        [PunRPC]
        public void OtherPowerUp(int Id)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int[] a = CreateRoomMenu.where;
                int[] l = Generation.Aleatoire(a.Length - 3, CreateRoomMenu.Xm * CreateRoomMenu.Zm);
                int b = 4;
                int s = 4;
                while (a[s] != Id)
                    s++;
                bool v = false;
                while (b < a.Length && !v)
                {
                    int z = 4;
                    v = true;
                    while (z < a.Length)
                    {
                        v &= a[z] != l[b - 4];
                        z++;
                    }
                    if (!v)
                        b++;
                }
                CreateRoomMenu.where[s] = l[b - 4];
                print("Nouveau : " + s + " = " + Id + " devient " + l[b - 4]);
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"),
                    new Vector3((float) (4 * (CreateRoomMenu.where[s] % CreateRoomMenu.Xm) + 2.5), 2,
                        (float) (4 * (CreateRoomMenu.where[s] / CreateRoomMenu.Xm) + 2.5)), Quaternion.identity);
            }
        }

        // Game play //ggPhoyo
        // A player changes status
        [PunRPC]
        public void SetStatus(int from, PlayerState.PlayerStatus status)
        {
            // Change for the target player only
            if (from == id)
            {
                playerState.Status = status;
                if (PhotonNetwork.IsMasterClient && status == PlayerState.PlayerStatus.REVENGE)
                    PlayerMaster.UpdateRevengePlayers(id);
            }
        }
        
        public IEnumerator SerumUrgence()
        {
            yield return new WaitForSeconds(5);
            SendSetStatus(PlayerState.PlayerStatus.HEALED,true);
        }

        public void SendSetStatus(PlayerState.PlayerStatus status,bool s = false)
        {
            if (status == PlayerState.PlayerStatus.REVENGE && Player.alterations[8])
            {
                Player.alterations[8] = false;
                AffichagePowerUpJoueur.MaJ(Player.alterations);
                SendSetStatus(PlayerState.PlayerStatus.GHOST);
                StartCoroutine(SerumUrgence());
            }
            else
            {
                /*if (status == PlayerState.PlayerStatus.HEALED)
                {
                    if (!s)
                    {
                        EndGame.AddRecap("Soigné");
                    }
                }
                if (status == PlayerState.PlayerStatus.REVENGE)
                {
                    //EndGame.AddRecap("Condamné");
                }*/
                view.RPC("SetStatus", RpcTarget.All, id, status);
            }
        }
        // Called every third of second
        [PunRPC]
        public void SyncNet(int from, float stamina, bool stunned)
        {
            if (!isLocal && playerState.staminaUi)
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
            PlayerMaster.instance.NewPlayerReady();
        }

        public void SendPlayerReady()
        {
            view.RPC("PlayerReady", RpcTarget.MasterClient, id);
        }

        // First phase, after game init
        [PunRPC]
        public void FirstPhase()
        {
            if (CreateRoomMenu.Mode == 1)
            {
                foreach (Luminosite l in Luminosite.instance)
                {
                    l.Change();
                }
            }
            local.GetComponent<Player>().OnGameBegin();
        }
        public void SendFirstPhase()
        {
            StartCoroutine(SendFirstPhaseDelay());
        }
        IEnumerator SendFirstPhaseDelay()
        {
            // Wait a bit
            yield return new WaitForSeconds(1);
            view.RPC("FirstPhase", RpcTarget.All);
        }
        [PunRPC]
        public void SecondPhase()
        {
            if (isLocal)
                Audio.Play("phase_change");

            if (CreateRoomMenu.Mode == 0)
            {
                foreach (Luminosite l in Luminosite.instance)
                {
                    l.Change();
                }
            }
            localPlayer.net.playerState.EndFirstPhase();
            StartCoroutine(playerState.SecondPhase());
        }

        public void SendSecondPhase()
        {
            view.RPC("SecondPhase", RpcTarget.All);
        }

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

        public void SendHerbeBleue()
        {
            view.RPC("HerbeBleue", RpcTarget.All);
        }

        [PunRPC]
        public IEnumerator HerbeBleue()
        {
            PlayerState.startTime += 30;
            if (!AffichagePowerUp.affich)
            {
                if(PlayerPrefs.GetInt("language") == 1)
                    AffichagePowerUp.Nature = "Quelqu'un a obtenu l'Objet Herbe Bleue !";
                else
                    AffichagePowerUp.Nature = "Someone took the Blue Grass !";
                AffichagePowerUp.affich = true;
                yield return new WaitForSeconds(5);
                AffichagePowerUp.affich = false;
            }
        }
        [PunRPC]
        public void RevengeWin(int winnerId)
        {
            local.GetComponent<Player>().canMove = false;
            local.playerState.EndOfGame(true, localPlayer.net.id == winnerId);
        }

        public void SendRevengeWin(int winnerId)
        {
            view.RPC("RevengeWin", RpcTarget.All, winnerId);
        }

        // When the stamina has to be decreased
        [PunRPC]
        void Hit(float strength)
        {
            if (isLocal)
            {
                var player = GetComponent<Player>();
                player.Stamina -= strength;
            }
        }

        public void SendHit(float strength)
        {
            view.RPC("Hit", RpcTarget.All, strength);
        }
    }
}
