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

        public void OnClick_Button()
        {
            Audio.Play("click");
            if (_text != null)
            {
                CreateRoomMenu.Xm = Convert.ToInt32(_text.text.Remove(2));
                CreateRoomMenu.Zm = Convert.ToInt32(_text.text.Remove(0, 3));
            }
            else
            {
                CreateRoomMenu.Xm = Convert.ToInt32(_inputField.text.Remove(2));
                CreateRoomMenu.Zm = Convert.ToInt32(_inputField.text.Remove(0, 3));
            }
        }
    }
}

