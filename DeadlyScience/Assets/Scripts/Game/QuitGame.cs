using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("OK");

            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
