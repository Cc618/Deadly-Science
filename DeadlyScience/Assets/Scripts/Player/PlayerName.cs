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

            SetStatus(PlayerState.PlayerStatus.INFECTED);
        }

        // Changes the color
        public void SetStatus(PlayerState.PlayerStatus status)
        {
            switch (status)
            {
                case PlayerState.PlayerStatus.HEALED:
                    text.color = Game.colors.healed;
                    break;
                case PlayerState.PlayerStatus.INFECTED:
                    text.color = Game.colors.infected;
                    break;
            }
        }

        private Text text;
    }
}
