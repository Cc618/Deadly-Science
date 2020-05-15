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
        private static double timeStart;

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
            timeStart = PhotonNetwork.Time;
            Text.text = "Entrée dans le laboratoire";
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
            int time = (int) (PhotonNetwork.Time - timeStart);

            TMP_Text go = (TMP_Text) Instantiate(Text);
            go.transform.SetParent(content.transform);
            go.transform.localScale = Vector3.one;

            if (objet)
            {
                go.text = $"{Event} rammasé à {time}\n";
            }
            else
            {
                go.text = $"{Event} à {time}\n";
            }
        }
    }
}
