// Used to rotate a label towards a player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ds
{
    public class LookToCam : MonoBehaviour
    {
        void LateUpdate()
        {
            // Only if we are rendering
            if (Camera.current)
            {
                Vector3 target = transform.position - Camera.current.transform.position;
                transform.rotation = Quaternion.LookRotation(target);
            }
        }
    }
}
