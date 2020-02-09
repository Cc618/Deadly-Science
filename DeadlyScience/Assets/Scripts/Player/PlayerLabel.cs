// Used to rotate a label towards a player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class PlayerLabel : MonoBehaviour
    {
        public Transform cam;
        public Color healedColor;
        public Color infectedColor;

        private void Start()
        {
            text = GetComponent<Text>();

            SetStatus(Player.PlayerStatus.INFECTED);
        }

        void LateUpdate()
        {
            Vector3 target = transform.position - cam.position;
            transform.rotation = Quaternion.LookRotation(target);
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
