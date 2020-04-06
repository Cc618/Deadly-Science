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
