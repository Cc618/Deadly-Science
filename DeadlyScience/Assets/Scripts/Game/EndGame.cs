using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ds
{
    public class EndGame : MonoBehaviour
    {

        public static Canvas EndScreen;
        private static GameObject Victory;
        private static GameObject Defeat;

        void Start()
        {
            EndScreen = (Canvas) GetComponent("EndScreen");
            Victory = GameObject.Find("EndScreen/Victory");
            Defeat = GameObject.Find("EndScreen/Defeat");
            Victory.SetActive(false);
            Defeat.SetActive(false);
        }

        public static void EndOfGame(bool victory)
        {
            Audio.Play(victory ? "win" : "loose");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (victory)
                Victory.SetActive(true);
            else
                Defeat.SetActive(true);
        } 
    }
}
