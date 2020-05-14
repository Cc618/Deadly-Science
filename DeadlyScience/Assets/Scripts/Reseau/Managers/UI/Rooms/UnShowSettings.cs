using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class UnShowSettings : MonoBehaviour
    {
        public GameObject canvas;
            
        public void OnClick_Button()
        {
            Audio.Play("click");
            canvas.SetActive(false);
        }
    }
}

