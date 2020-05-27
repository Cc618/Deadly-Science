using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class SizeSettings : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _text;
        [SerializeField] 
        private TMP_InputField _inputField;
        [SerializeField]
        private TMP_Text _test;
        public void OnClick_Button()
        {
            Audio.Play("click");
            string s = "";
            if (_text != null)
            {
                print(_text.text);
                s = _text.text;
                CreateRoomMenu.Xm = (s[0]-48) * 10 + (s[1]-48);
                CreateRoomMenu.Zm =(s[3]-48) * 10 +(s[4]-48);
            }
            else
            {
                s = _inputField.text;
                _test.text = s;
                if (s.Length==5 && s[2] == '*' && s[0] >= '0' && s[0] <= '9' && s[1] >= '0' && s[1] <= '9' && s[3] >= '0' && s[3] <= '9' &&
                    s[4] >= '0' && s[4] <= '9')
                {
                    int x = (s[0]-48) * 10 + s[1]-48;
                    int z = (s[3]-48) * 10 + s[4]-48;
                    print(x);
                    print(z);
                    if (x * z < 8 || x * z > 400)
                    {
                        if(PlayerPrefs.GetInt("language") == 1)
                            _test.text= "Le nombre de salle doit être compris entre 8 et 400.";
                        else
                            _test.text = "Labyrinth size must be between 8 and 400 rooms";
                    }
                    else
                    {
                        CreateRoomMenu.Xm = x;
                        CreateRoomMenu.Zm = z;
                        _test.text = "";
                    }
                }
                else
                {
                    print("Nope");
                    if (PlayerPrefs.GetInt("language") == 1)
                        _test.text =
                            "Le format à respecter est {00*00}, en remplaçant les '0' par les valeurs de votre choix.";
                    else
                        _test.text = "Format of size is {00*00}, replacing '0''s by your values";
                }
            }

            print("X : " + CreateRoomMenu.Xm);
            print("Z : " + CreateRoomMenu.Zm);
        }
    }
}

