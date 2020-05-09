using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Particles : MonoBehaviour
    {
        public static Particles instance;

        public ParticleSystem[] systems;

        [Serializable]
        public class ParticleSystem
        {
            public string id;
            public GameObject system;
        }

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public static void Spawn(string id, Vector3 loc)
        {
            var fx = Array.Find(instance.systems, (system) => system.id == id);

            if (fx == null)
                return;

            Instantiate(fx.system, loc, Quaternion.identity);
        }
    }
}
