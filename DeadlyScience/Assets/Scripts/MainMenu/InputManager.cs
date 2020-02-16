using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    Dictionary<string, KeyCode> buttonKeys;

    void Awake()
    {
        DontDestroyOnLoad(this);

        buttonKeys = new Dictionary<string, KeyCode>();

        
        //TODO: Read the defaults from a user pref file
        buttonKeys["Jump"] = KeyCode.Space;
        buttonKeys["Forward"] = KeyCode.W;
        buttonKeys["Backward"] = KeyCode.S;
        buttonKeys["Left"] = KeyCode.A;
        buttonKeys["Right"] = KeyCode.D;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsButtonDown(string buttonName)
    {
        if(buttonKeys.ContainsKey(buttonName) == false)
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
