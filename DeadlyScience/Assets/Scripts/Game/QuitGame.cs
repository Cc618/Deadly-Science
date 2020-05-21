using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


namespace ds
{
    public class QuitGame : MonoBehaviour
    {

        public void Quitgame()
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            // Avoid pause when returning to game
            Game.EscapeMenuOpen = false;

            PhotonNetwork.LeaveRoom();
            Application.Quit();
        }
    }
}
