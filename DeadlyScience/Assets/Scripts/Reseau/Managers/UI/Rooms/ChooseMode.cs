using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class ChooseMode : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        public void OnClick_Button()
        {
            Audio.Play("click");
            int a = 0;
            while (_text.text != new[] {"CLASSIQUE", "CLASSIC", "NOCTURNE", "NOCTURN"}[a])
            {
                a += 1;
            }
            CreateRoomMenu.Mode = a /2;
            print(CreateRoomMenu.Mode);
        }
    }
}