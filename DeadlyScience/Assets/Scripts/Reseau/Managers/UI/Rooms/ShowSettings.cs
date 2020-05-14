using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class ShowSettings : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvas;
        public void OnClick_Button()
        {
            Audio.Play("click");
            canvas.SetActive(true);
        }
    }
}

