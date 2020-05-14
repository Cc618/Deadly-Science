using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class SizeSettings : MonoBehaviour
    {

        [SerializeField]
        private Text _text;

        public void OnClick_Button()
        {
            Audio.Play("click");
            CreateRoomMenu.Xm = Convert.ToInt32(_text.text.Remove(2));
            CreateRoomMenu.Zm = Convert.ToInt32(_text.text.Remove(0, 3));
            print(CreateRoomMenu.Xm);
            print(CreateRoomMenu.Zm);
        }
    }
}

