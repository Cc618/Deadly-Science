using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ds
{
    public class Settings : MonoBehaviour
    {
        [Range(0, 1)]
        public float mouseSensivity;

        public Slider MusiqueSlider;
        public Slider SFXSlider;
        public Slider MouseSlider;
        public TMP_InputField Pseudo;
        Audio Audio;

        public void Awake()
        {
            Audio = GetComponent<Audio>();
            
            if (PlayerPrefs.HasKey("mouseSensivity"))
                mouseSensivity = PlayerPrefs.GetFloat("mouseSensivity");
            else
            {
                mouseSensivity = (float)0.5;
                PlayerPrefs.SetFloat("mouseSensivity", mouseSensivity);
            }

            if (PlayerPrefs.HasKey("musiqueVolume"))
                Audio.musicVolume = PlayerPrefs.GetFloat("musiqueVolume");
            else
            {
                Audio.musicVolume = (float)0.5;
                PlayerPrefs.SetFloat("musiqueVolume", Audio.musicVolume);
            }

            if (PlayerPrefs.HasKey("sfxVolume"))
                Audio.sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            else
            {
                Audio.sfxVolume = (float)0.5;
                PlayerPrefs.SetFloat("sfxVolume", Audio.sfxVolume);
            }

            if (PlayerPrefs.HasKey("pseudo"))
                Pseudo.text = PlayerPrefs.GetString("pseudo");
            else
            {
                Pseudo.text = "Sujet" + Random.Range(0, 9999);
                PlayerPrefs.SetString("pseudo", Pseudo.text);
            }

            PlayerPrefs.Save();
        }

        public void Start()
        {
            MouseSlider.normalizedValue = mouseSensivity;
            MusiqueSlider.normalizedValue = Audio.musicVolume;
            SFXSlider.normalizedValue = Audio.sfxVolume;
        }

        public void OnMouseSensitivityValueChange(float value)
        {
            mouseSensivity = value;
            PlayerPrefs.SetFloat("mouseSensivity", mouseSensivity);
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
            Pseudo.text = "Sujet" + Random.Range(1000, 9999);
            PlayerPrefs.SetString("pseudo", Pseudo.text);
            PlayerPrefs.Save();
        }
    }
}
