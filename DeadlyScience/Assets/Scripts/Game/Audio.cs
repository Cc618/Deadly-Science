using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To play SFX / Music

namespace ds
{
    public class Audio : MonoBehaviour
    {
        [Serializable]
        public class Sound
        {
            public string id;
            public AudioClip clip;

            [HideInInspector]
            public AudioSource source;
        }

        // Singleton
        public static Audio instance;

        // TODO : Yann settings
        public static float musicVolume = .5F;
        public static float sfxVolume = .5F;

        public Sound[] musics;

        public GameObject audioSample;

        public void Awake()
        {
            DontDestroyOnLoad(this);

            instance = this;

            for (int i = 0; i < musics.Length; ++i)
            {
                musics[i].source = gameObject.AddComponent<AudioSource>();
                musics[i].source.clip = musics[i].clip;

                // TODO : Volume
                musics[i].source.volume = musicVolume;
            }
        }

        // Change music (wrapper)
        public static void SetMusic(string id)
        {
            instance.setMusic(id);
        }

        // Play SFX
        public static void Play(string id)
        {
            // TODO : Implement
            // TODO : Remote
        }

        private void setMusic(string id)
        {
            Sound snd = Array.Find(instance.musics, s => s.id == id);

            if (snd == null)
                Debug.LogError("Music with id '" + id + "' not found");
            else
            {
                // Stop old music
                if (currentMusic != null)
                    currentMusic.GetComponent<Animator>().SetTrigger("Fade");

                // Create new music
                currentMusic = Instantiate(audioSample);
                DontDestroyOnLoad(currentMusic);

                // Change source and play
                var src = currentMusic.GetComponent<AudioSource>();
                src.clip = snd.source.clip;
                src.volume = 0;
                src.Play();
            }
        }

        static GameObject currentMusic;
    }
}
