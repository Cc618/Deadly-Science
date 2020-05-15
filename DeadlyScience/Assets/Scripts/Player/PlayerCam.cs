using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class PlayerCam : MonoBehaviour
    {
        public Material postFx;
        [Range(0, 15)]
        public float heartSpeed;

        void Update()
        {
            if (!(Game.EscapeMenuOpen || EndGame.Victory.activeSelf || EndGame.Defeat.activeSelf))
            {
                // The new rotation
                float rot = transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * Game.settings.mouseSensivity * Screen.width / 60;
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

        private void LateUpdate()
        {
            time += Time.deltaTime * heartSpeed;
            postFx.SetFloat("_AnimRatio", .5f + Mathf.Sin(time) * .5f);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, postFx);
        }

        float time = 0f;
    }
}
