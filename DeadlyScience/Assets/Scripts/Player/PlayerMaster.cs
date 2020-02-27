// Executed when the player is the master player
// Handle every players and the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ds
{
    // Static part
    public partial class PlayerMaster : MonoBehaviour
    {
        // The player controlled by the client
        public static Player localPlayer;

        // List of all players
        private static List<GameObject> players = new List<GameObject>();
        
        // Registers a new player in the players list
        public static void RegisterPlayer(GameObject p)
        {
            players.Add(p);

            if (players.Count == PhotonNetwork.PlayerList.Length)
                OnAllPlayersInGame();
        }

        // When all players are in game
        static void OnAllPlayersInGame()
        {
            Debug.Log("PlayerMaster : All players in game");

            localPlayer.OnGameBegin();
        }
    }

    // Instance part
    public partial class PlayerMaster : MonoBehaviour
    {
        // TODO
    }
}
