// Used to change the color of the name

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerName : MonoBehaviour
    {
        public Color healedColor;
        public Color infectedColor;

        void Awake()
        {
            text = GetComponent<Text>();

            SetStatus(Player.PlayerStatus.INFECTED);
        }

        // Changes the color
        public void SetStatus(Player.PlayerStatus status)
        {
            switch (status)
            {
                case Player.PlayerStatus.HEALED:
                    text.color = healedColor;
                    break;
                case Player.PlayerStatus.INFECTED:
                    text.color = infectedColor;
                    break;
            }
        }

        private Text text;
    }
}
