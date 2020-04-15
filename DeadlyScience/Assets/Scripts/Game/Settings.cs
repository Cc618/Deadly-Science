﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class Settings : MonoBehaviour
    {
        [Range(0, 1)]
        public float mouseSensivity;

        public Slider volumeSlider;
        public Slider MouseSlider;
        Audio Audio;

        public void Awake()
        {
            if (PlayerPrefs.HasKey("mouseSensivity"))
                mouseSensivity = PlayerPrefs.GetFloat("mouseSensivity");
            else
            {
                mouseSensivity = (float)0.5;
                PlayerPrefs.SetFloat("mouseSensivity", mouseSensivity);
                PlayerPrefs.Save();
            }
        }

        public void Start()
        {
         
            MouseSlider.normalizedValue = mouseSensivity;


            Audio = GetComponent<Audio>();
        }

        public void OnMouseSensitivityValueChange(float value)
        {
            mouseSensivity = value;
            PlayerPrefs.SetFloat("mouseSensivity", mouseSensivity);
            PlayerPrefs.Save();
        }

        public void OnVolumeValueChange(float value)
        {
            Audio.sfxVolume = value;
        }
    }
}
