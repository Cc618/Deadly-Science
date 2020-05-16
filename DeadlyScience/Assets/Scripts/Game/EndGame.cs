using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class EndGame : MonoBehaviour
    {

        public static Canvas EndScreen;
        public static GameObject Victory;
        public static GameObject Defeat;
        public static GameObject Panel;
        public static TMP_Text Text;
        public static GameObject content;
        private static int time = 0;
        private static string Time = "";
        private static bool chrono;

        void Start()
        {
            EndScreen = (Canvas) GetComponent("EndScreen");
            Panel = GameObject.Find("EndScreen/Panel");
            Victory = GameObject.Find("EndScreen/Panel/Victory");
            Defeat = GameObject.Find("EndScreen/Panel/Defeat");
            Text = GameObject.Find("EndScreen/Panel/ScrollView/Viewport/content/Recap").GetComponent<TMP_Text>();
            content = GameObject.Find("EndScreen/Panel/ScrollView/Viewport/content");
            Panel.SetActive(false);
            Victory.SetActive(false);
            Defeat.SetActive(false);
            Text.text = "Entrée dans le laboratoire";
        }

        private void Update()
        {
            if (!chrono)
                StartCoroutine(Attente());
        }

        public static void EndOfGame(bool victory)
        {
            Audio.Play(victory ? "win" : "loose");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Panel.SetActive(true);

            if (victory)
                Victory.SetActive(true);
            else
                Defeat.SetActive(true);
        }

        public static void AddRecap(string Event, bool objet=false)
        {
            TMP_Text go = (TMP_Text) Instantiate(Text);
            go.transform.SetParent(content.transform);
            go.transform.localScale = Vector3.one;

            if (objet)
            {
                switch(Event)
                {
                    case "Carte":
                    case "Protection":
                    case "Décharge":
                    case "Paralysie":
                    case "Disparition":
                        go.text = $"{Event} ramassée à {Time}\n";
                        break;
                    case "Bottes de Plomb":
                    case "Bottes de Pégase":
                    case "Casque de CRS":
                        go.text = $"{Event} ramassé à {Time}\n";
                        break;
                }
            }
            else
            {
                go.text = $"{Event} à {time}s\n";
            }
        }

        IEnumerator Attente()
        {
            chrono = true;
            yield return new WaitForSeconds(1);
            time += 1;
            if (time < 60)
                Time = time.ToString() + "s";
            else
                Time = (time / 60).ToString() + "m" + (time % 60).ToString() + "s";
            chrono = false;
        }
    }
}
