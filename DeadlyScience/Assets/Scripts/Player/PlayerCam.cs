﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class PlayerCam : MonoBehaviour
    {
        void Update()
        {
            // The new rotation
            float rot = transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * Game.settings.mouseSensivity * Time.deltaTime;

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