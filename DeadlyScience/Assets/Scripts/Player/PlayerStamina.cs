using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerStamina : MonoBehaviour
    {
        public Image fill;

        void Start()
        {
            slider = GetComponent<Slider>();

            ChangeStunned(false);
        }

        // Changes the color of the fill image
        public void ChangeStunned(bool stunned)
        {
            fill.color = stunned ? Game.colors.staminaStunned : Game.colors.stamina;
        }

        public float Value { get => slider.value; set => slider.value = value; }

        private Slider slider;
    }
}
