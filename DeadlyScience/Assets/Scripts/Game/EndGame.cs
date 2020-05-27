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
        

        void Start()
        {
            EndScreen = (Canvas) GetComponent("EndScreen");
            Panel = GameObject.Find("EndScreen/Panel");
            Victory = GameObject.Find("EndScreen/Panel/Victory");
            Defeat = GameObject.Find("EndScreen/Panel/Defeat");
            Victory.SetActive(false);
            Defeat.SetActive(false);
            Panel.SetActive(false);
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
    }
}
