// Class to gather all informations and properties of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Game : MonoBehaviour
    {
        public static Inputs inputs;
        public static Colors colors;

        private void Awake()
        {
            inputs = GetComponent<Inputs>();
            colors = GetComponent<Colors>();
        }
    }
}
