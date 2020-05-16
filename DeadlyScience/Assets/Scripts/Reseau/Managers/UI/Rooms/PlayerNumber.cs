using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerNumber : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        public void OnClick_Button()
        {
            Audio.Play("click");
            int a = 0;
            while (_text.text != new[] {"1", "2", "3", "4"}[a])
            {
                a += 1;
            }
            CreateRoomMenu.PlayerNumber = a + 1;
            print(CreateRoomMenu.PlayerNumber);
        }
    }
}