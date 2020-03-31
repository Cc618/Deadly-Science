using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To play SFX / Music

namespace ds
{
    public class Audio : MonoBehaviour
    {
        public class Sound
        {
            public string id;
            public AudioClip clip;

            [HideInInspector]
            public AudioSource source;
        }

        // Singleton
        public static Audio instance;

        public Sound[] musics;

        public void Awake()
        {
            instance = this;

            for (int i = 0; i < musics.Length; ++i)
            {
                musics[i].source = gameObject.AddComponent<AudioSource>();
                musics[i].source.clip = musics[i].clip;
            }
        }

        // Change music
        public static void SetMusic(string id)
        {
            Sound snd = Array.Find(instance.musics, s => s.id == id);

            if (snd == null)
                Debug.LogError("Music with id '" + id + "' not found");
            else
                snd.source.Play();
        }

        // Play SFX
        public static void Play(string id)
        {
            // TODO : Implement
            // TODO : Remote
        }
    }
}
