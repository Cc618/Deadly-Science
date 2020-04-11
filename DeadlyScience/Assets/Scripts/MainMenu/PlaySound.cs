// Just a wrapper to make more readable a sound event in UIs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class PlaySound : MonoBehaviour
    {
        public void Play(string sfx)
        {
            Audio.Play(sfx);
        }
    }
}
