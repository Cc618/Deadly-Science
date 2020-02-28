using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InputManager : MonoBehaviour
{
    Dictionary<string, KeyCode> buttonKeys;

    void Awake()
    {
        buttonKeys = new Dictionary<string, KeyCode>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Read the defaults from a user pref file
        buttonKeys["Jump"] = KeyCode.Space;
        buttonKeys["Forward"] = KeyCode.W;
        buttonKeys["Backward"] = KeyCode.S;
        buttonKeys["Left"] = KeyCode.A;
        buttonKeys["Right"] = KeyCode.D;

        //try
        //{
        //    buttonKeys["Jump"] = (KeyCode)PlayerPrefs.GetInt("Jump");
        //    buttonKeys["Forward"] = (KeyCode)PlayerPrefs.GetInt("Forward");
        //    buttonKeys["Backward"] = (KeyCode)PlayerPrefs.GetInt("Backward");
        //    buttonKeys["Left"] = (KeyCode)PlayerPrefs.GetInt("Left");
        //    buttonKeys["Right"] = (KeyCode)PlayerPrefs.GetInt("Right");
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //    Debug.LogWarning("No userPref file for keybindigs found");
        //    Debug.Log("Keybindigs take default values");

        //    foreach (string key in buttonKeys.Keys)
        //    {
        //        PlayerPrefs.SetInt(key, (int)buttonKeys[key]);
        //    }

        //    PlayerPrefs.Save();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsButtonDown(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("InputManager::GetButtonDown -- No button named: " + buttonName);
            return false;
        }

        return Input.GetKey(buttonKeys[buttonName]);
    }

    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }

    public string GetKeyNameForButton( string buttonName )
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("InputManager::GetKeyNameForButton -- No button named: " + buttonName);
            return "N/A";
        }

        return buttonKeys[buttonName].ToString();
    }

    public void SetButtonForKey( string buttonName, KeyCode keyCode)
    {
        buttonKeys[buttonName] = keyCode;
    }
}
