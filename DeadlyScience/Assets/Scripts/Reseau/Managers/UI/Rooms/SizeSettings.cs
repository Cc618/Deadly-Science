using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class SizeSettings : MonoBehaviour
    {
        private int xm;
        private int zm;
        [SerializeField]
        private Text _text;
        public void OnClick_Button()
        {
            Audio.Play("click");
            xm = Convert.ToInt32(_text.text.Remove(2));
            zm = Convert.ToInt32(_text.text.Remove(0, 3));
        }
    }
}

