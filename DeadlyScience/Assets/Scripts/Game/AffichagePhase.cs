using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class AffichagePhase : MonoBehaviour
    {
        public Text scoreText;
        public static int phase;
        public static int temps;
        public static string Objectif;

        void Update()
        {
            if (phase == 1)
            {
                scoreText.text = "Cherchez les Sérums !";
            }

            if (phase == 2)
            {
                scoreText.text = Objectif+"Temps Restant : "+temps.ToString();
            }

            if (phase == 3)
            {
                scoreText.text = "";
            }
        }
    }
}