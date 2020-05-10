using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class Affichage_Joueur : MonoBehaviour
    {
        public Text scoreText;

        void Update()
        {
            string test = PlayerNetwork.local.playerState.Status.ToString();
            scoreText.color = Color.magenta;
            switch (test)
            {
                case "HEALED":
                {
                    scoreText.color = Color.green;
                    break;
                }
                case "REVENGE":
                {
                    scoreText.color = Color.red;
                    break;
                }
                case "GHOST":
                {
                    scoreText.color = Color.grey;
                    break;
                }
            }
            scoreText.text = PhotonNetwork.NickName;
        }
    }
}
