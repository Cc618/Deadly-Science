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
        public static int phase = 1;
        public static int temps;
        public static string Objectif;
        private static int lang = 0;

        private void Start()
        {
            if(PlayerPrefs.HasKey("language"))
            {
                lang = PlayerPrefs.GetInt("language");
            }
        }

        void Update()
        {
            switch(lang)
            {
                case 0:
                    if (phase == 1)
                    {
                        scoreText.text = "Search the serum !";
                    }

                    if (phase == 2)
                    {
                        scoreText.text = Objectif + "Remaining time : " + temps.ToString();
                    }

                    if (phase == 3)
                    {
                        scoreText.text = "";
                    }
                    break;
                case 1:
                    if (phase == 1)
                    {
                        scoreText.text = "Cherchez les Sérums !";
                    }

                    if (phase == 2)
                    {
                        scoreText.text = Objectif + "Temps Restant : " + temps.ToString();
                    }

                    if (phase == 3)
                    {
                        scoreText.text = "";
                    }
                    break;
            }
            
        }
    }
}