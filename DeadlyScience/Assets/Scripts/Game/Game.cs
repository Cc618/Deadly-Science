// Class to gather all informations and properties of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Game : MonoBehaviour
    {
        public static Settings settings;
        public static Inputs inputs;
        public static Colors colors;

        private void Awake()
        {
            settings = GetComponent<Settings>();
            inputs = GetComponent<Inputs>();
            colors = GetComponent<Colors>();
        }

        private void Start()
        {
            // Lock and hide cursor
            Cursor.visible = false;

            // TODO : Mode confined in menu / pause
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // TODO : Pause
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Unlock and show cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
