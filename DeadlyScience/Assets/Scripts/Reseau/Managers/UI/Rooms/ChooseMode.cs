﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class ChooseMode : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        public void OnClick_Button()
        {
            Audio.Play("click");
            int a = 0;
            string s = _text.text;
            s = s.Remove(_text.text.Length - 1);
            while (s != new[] {"CLASSIQUE", "CLASSIC", "NOCTURNE", "NOCTURN"}[a])
                a++;
            CreateRoomMenu.Mode = a / 2;
            print(CreateRoomMenu.Mode);
        }
    }
}