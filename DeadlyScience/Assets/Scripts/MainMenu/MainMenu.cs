using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ds
{
    public class MainMenu : MonoBehaviour
    {
        // Menu opened for the first time ?
        static bool firstTime = true;

        private void Start()
        {
            if (firstTime)
            {
                Audio.SetMusic("menu");
                firstTime = false;
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Rooms");
            Audio.Play("click");
        }

        public void QuitGame()
        {
            Audio.Play("click");

            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}
