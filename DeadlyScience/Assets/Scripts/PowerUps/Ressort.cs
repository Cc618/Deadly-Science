using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class Ressort : MonoBehaviour
    {
        private static bool afficher = false;
        public static int time;
        public static bool chrono;
        // Start is called before the first frame update

        public static void Change(bool affich)
        {
            print("Sauts : " + affich);
            afficher = affich;
            if (affich)
            {
                time = 10;
                chrono = false;
            }
            else
            {
                Player.alterations[6] = false;
                AffichagePowerUpJoueur.MaJ(Player.alterations);
            }
        }
        // Update is called once per frame
        private void Update()
        {
            if (time > 0)
            {
                if (!chrono)
                {
                    print(time);
                    StartCoroutine(Attente());
                }
            }
            else
            {
                if (afficher)
                {
                    print("STOP");
                    Change(false);
                }
            }
        }
        IEnumerator Attente()
        {
            print(time);
            chrono = true;
            yield return new WaitForSeconds(1);
            time -= 1;
            chrono = false;
        }
    }
}