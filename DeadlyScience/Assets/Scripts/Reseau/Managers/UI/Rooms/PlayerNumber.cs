using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerNumber : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        public void OnClick_Button()
        {
            Audio.Play("click");
            int a = 0;
            while (_text.text != new[] {"1", "2", "3", "4"}[a])
                a++;
            CreateRoomMenu.PlayerNumber = a + 1;
            print(CreateRoomMenu.PlayerNumber);
        }
    }
}