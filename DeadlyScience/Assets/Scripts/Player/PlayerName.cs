// Used to change the color of the name

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerName : MonoBehaviour
    {
        void Start()
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
                    text.color = Game.colors.healed;
                    break;
                case Player.PlayerStatus.INFECTED:
                    text.color = Game.colors.infected;
                    break;
            }
        }

        private Text text;
    }
}
