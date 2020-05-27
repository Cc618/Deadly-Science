using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ds
{
    public class InputManager : MonoBehaviour
    {
        Dictionary<string, KeyCode> buttonKeys;

        void Awake()
        {
            buttonKeys = new Dictionary<string, KeyCode>();

            buttonKeys["Jump"] = KeyCode.Space;
            buttonKeys["Forward"] = KeyCode.Z;
            buttonKeys["Backward"] = KeyCode.S;
            buttonKeys["Left"] = KeyCode.Q;
            buttonKeys["Right"] = KeyCode.D;
        }

        // Start is called before the first frame update
        void Start()
        {
            List<string> keys = new List<string>();

            foreach (string key in buttonKeys.Keys)
            {
                keys.Add(key);
            }

            for (int i = 0; i < buttonKeys.Count; i++)
            {
                if (PlayerPrefs.HasKey(keys[i]))
                    buttonKeys[keys[i]] = (KeyCode)PlayerPrefs.GetInt(keys[i]);
            }

            foreach (string key in buttonKeys.Keys)
            {
                PlayerPrefs.SetInt(key, (int)buttonKeys[key]);
            }

            PlayerPrefs.Save();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsButtonDown(string buttonName, bool justInGame = true)
        {
            // No input when the escape menu is open
            if (!Game.EscapeMenuOpen || !justInGame)
            {
                if (buttonKeys.ContainsKey(buttonName) == false)
                {
                    Debug.LogError("InputManager::GetButtonDown -- No button named: " + buttonName);
                    return false;
                }

                return Input.GetKey(buttonKeys[buttonName]);
            }

            return false;
        }

        public string[] GetButtonNames()
        {
            return buttonKeys.Keys.ToArray();
        }

        public string GetKeyNameForButton(string buttonName)
        {
            if (buttonKeys.ContainsKey(buttonName) == false)
            {
                Debug.LogError("InputManager::GetKeyNameForButton -- No button named: " + buttonName);
                return "N/A";
            }

            return buttonKeys[buttonName].ToString();
        }

        public void SetButtonForKey(string buttonName, KeyCode keyCode)
        {
            buttonKeys[buttonName] = keyCode;
        }
    }
}