using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class PlayerCam : MonoBehaviour
    {
        void Update()
        {
            if (!Game.EscapeMenuOpen)
            {
                // The new rotation
#if UNITY_EDITOR
                float rot = transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * Game.settings.mouseSensivity * Time.deltaTime * Screen.width / 2;
#else
                float rot = transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * Game.settings.mouseSensivity * Time.deltaTime * Screen.width;
#endif
                // Clip rot
                if (rot > 90 && rot < 180)
                    rot = 90;
                else if (rot < 360 - 90 && rot > 180)
                    rot = -90;

                transform.rotation = Quaternion.Euler(
                    rot,
                    transform.rotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z
                );
            }
        }
    }
}
