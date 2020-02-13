// Used to rotate a label towards a player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class LookToCam : MonoBehaviour
    {
        public Transform cam;

        void LateUpdate()
        {
            Vector3 target = transform.position - cam.position;
            transform.rotation = Quaternion.LookRotation(target);
        }
    }
}
