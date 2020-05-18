using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ds
{
    public class KeybindDialog : MonoBehaviour
    {

        InputManager inputManager;
        public GameObject keyList;

        string ButtonToRebind;
        Dictionary<string, TMP_Text> buttonToLabel;

        // Start is called before the first frame update

        //ANCIEN START A GARDER AU CAS OU !!!!!!!!!!!!!!!!!!
        /*void Start()
        {
            inputManager = GameObject.FindObjectOfType<InputManager>();

            // Create one "KeyListItem" per button in inputManager
            string[] buttonNames = inputManager.GetButtonNames();

            buttonToLabel = new Dictionary<string, Text>();

            for (int i = 0; i < buttonNames.Length; i++)
            {
                string bn = buttonNames[i];

                GameObject go = (GameObject)Instantiate(keyItemPrefab);
                go.transform.SetParent(keyList.transform);
                go.transform.localScale = Vector3.one;

                TMP_Text buttonNameText = go.transform.Find("ButtonName").GetComponent<TMP_Text>();
                buttonNameText.text = bn;

                Text keyNameText = go.transform.Find("Button/KeyName").GetComponent<Text>();
                keyNameText.text = inputManager.GetKeyNameForButton(bn);
                buttonToLabel[bn] = keyNameText;

                Button keyBindButton = go.transform.Find("Button").GetComponent<Button>();
                keyBindButton.onClick.AddListener(() => StartRebindFor(bn));
            }
        }*/

        private void Start()
        {
            inputManager = GameObject.FindObjectOfType<InputManager>();
            buttonToLabel = new Dictionary<string, TMP_Text>();

            string[] buttonNames = inputManager.GetButtonNames();

            Button[] buttons = keyList.GetComponentsInChildren<Button>();
            TMP_Text[] keys = new TMP_Text[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                keys[i] = buttons[i].GetComponentInChildren<TMP_Text>();

            }

            for (int i = 0; i < keys.Length; i++)
            {
                string bn = buttonNames[i];
                
                //TMP_Text key = keys[i].GetComponent<TMP_Text>();
                keys[i].text = inputManager.GetKeyNameForButton(bn);
                buttonToLabel[bn] = keys[i];

                buttons[i].onClick.AddListener(() => StartRebindFor(bn));
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (ButtonToRebind != null)
            {
                if (Input.anyKeyDown)
                {
                    //Wich key was pressed down?
                    //Loop through all keys to test

                    foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(kc)) //is this key down ?
                        {
                            inputManager.SetButtonForKey(ButtonToRebind, kc);
                            buttonToLabel[ButtonToRebind].text = kc.ToString();
                            PlayerPrefs.SetInt(ButtonToRebind, (int)kc);
                            PlayerPrefs.Save();
                            ButtonToRebind = null;
                            break;
                        }
                    }
                }
            }
        }

        void StartRebindFor(string buttonName)
        {
            Debug.Log("StartRebindFor" + buttonName);

            ButtonToRebind = buttonName;
        }
    }
}