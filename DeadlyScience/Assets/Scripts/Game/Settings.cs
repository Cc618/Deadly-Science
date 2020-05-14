using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ds
{
    public class Settings : MonoBehaviour
    {
        [Range(0, 0.7f)]
        public float mouseSensivity;

        public Slider MusiqueSlider;
        public Slider SFXSlider;
        public Slider MouseSlider;
        public TMP_InputField Pseudo;
        public TMP_Dropdown language;
        Localization_SOURCE Localization_SOURCE;
        Audio Audio;

        public void Awake()
        {
            Audio = GetComponent<Audio>();
            Localization_SOURCE = GetComponent<Localization_SOURCE>();

            if (PlayerPrefs.HasKey("mouseSensivity"))
                mouseSensivity = PlayerPrefs.GetFloat("mouseSensivity");
            else
            {
                mouseSensivity = 0.5f;
                PlayerPrefs.SetFloat("mouseSensivity", mouseSensivity);
            }

            if (PlayerPrefs.HasKey("musiqueVolume"))
                Audio.musicVolume = PlayerPrefs.GetFloat("musiqueVolume");
            else
            {
                Audio.musicVolume = 0.2f;
                PlayerPrefs.SetFloat("musiqueVolume", Audio.musicVolume);
            }

            if (PlayerPrefs.HasKey("sfxVolume"))
                Audio.sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            else
            {
                Audio.sfxVolume = 0.8f;
                PlayerPrefs.SetFloat("sfxVolume", Audio.sfxVolume);
            }

            if (Pseudo && PlayerPrefs.HasKey("pseudo"))
                Pseudo.text = PlayerPrefs.GetString("pseudo");
            else if (Pseudo)
            {
                Pseudo.text = "Sujet" + UnityEngine.Random.Range(0, 9999);
                PlayerPrefs.SetString("pseudo", Pseudo.text);
            }

            if (language && PlayerPrefs.HasKey("language"))
            {
                language.value = PlayerPrefs.GetInt("language");
                Localization_SOURCE.PUBLIC_LoadLanguage(language.value);
            }
            else if (language)
            {
                PlayerPrefs.SetInt("language", language.value);
            }

            PlayerPrefs.Save();
        }

        public void Start()
        {
            MouseSlider.value = mouseSensivity;
            MusiqueSlider.normalizedValue = Audio.musicVolume;
            SFXSlider.normalizedValue = Audio.sfxVolume;
        }

        public void OnMouseSensitivityValueChange(float value)
        {
            mouseSensivity = value;
            PlayerPrefs.SetFloat("mouseSensivity", value);
            PlayerPrefs.Save();
        }

        public void OnMusiqueValueChange(float value)
        {
            Audio.musicVolume = value;
            PlayerPrefs.SetFloat("musiqueVolume", value);
            PlayerPrefs.Save();
        }

        public void OnSFXValueChange(float value)
        {
            Audio.sfxVolume = value;
            PlayerPrefs.SetFloat("sfxVolume", value);
            PlayerPrefs.Save();
        }

        public void OnPseudoChange(string pseudo)
        {
            Pseudo.text = pseudo;
            PlayerPrefs.SetString("pseudo", pseudo);
            PlayerPrefs.Save();
        }

        public void OnRandomPseudo()
        {
            Pseudo.text = "Sujet" + UnityEngine.Random.Range(1000, 9999);
            PlayerPrefs.SetString("pseudo", Pseudo.text);
            PlayerPrefs.Save();
        }

        public void OnLanguageChange(int val)
        {
            Localization_SOURCE.PUBLIC_LoadLanguage(val);
            PlayerPrefs.SetInt("language", val);
            PlayerPrefs.Save();
        }
    }
}
