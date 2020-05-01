using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class AudioSample : MonoBehaviour
    {
        public float volume = 0;
        public float Volume
        {
            get => volume;
            set
            {
                GetComponent<AudioSource>().volume = value * Audio.musicVolume;
            }
        }

        private void Update()
        {
            Volume = volume;
        }
    }
}
