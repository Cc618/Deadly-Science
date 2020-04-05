using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ds
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            Audio.SetMusic("menu");
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Rooms");
        }

        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}
