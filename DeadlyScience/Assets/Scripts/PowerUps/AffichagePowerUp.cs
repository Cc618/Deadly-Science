using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichagePowerUp : MonoBehaviour
{
    public Text scoreText;
    public static string Nature;
    public static bool affich=false;

    void Update()
    {
        if (affich)
        {
            scoreText.text = Nature;
        }
        else
        {
            scoreText.text = "";
        }
    }
}
