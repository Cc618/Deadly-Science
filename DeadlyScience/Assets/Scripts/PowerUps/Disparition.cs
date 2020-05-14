using System.Collections;
using System.Collections.Generic;
using ds;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Disparition : MonoBehaviour
{
    private static PlayerState.PlayerStatus s;
    private static bool afficher = false;
    public static int time;
    public static bool chrono;
    // Start is called before the first frame update
    void Start()
    {
        s = PlayerState.PlayerStatus.GHOST;
    }

    public static void Change(bool affich)
    {
        print("Disparition : " + affich);
        afficher = affich;
        if (affich)
        {
            if (PlayerNetwork.local.playerState.Status !=PlayerState.PlayerStatus.GHOST)
            {
                s = PlayerNetwork.local.playerState.Status;
                PlayerNetwork.local.SendSetStatus(PlayerState.PlayerStatus.GHOST);
            }
            time = 30;
            chrono = false;
        }
        else
        {
            PlayerNetwork.local.SendSetStatus(s);
            s = PlayerState.PlayerStatus.GHOST;
            Player.alterations[4] = false;
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
