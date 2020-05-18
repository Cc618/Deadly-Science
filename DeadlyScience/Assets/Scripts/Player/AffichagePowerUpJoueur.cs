using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichagePowerUpJoueur : MonoBehaviour
{
    public Text scoreText;
    public static string content = "";

    void Update()
    {
        scoreText.text = content;
    }

    public static void MaJ(bool[] modif)
    {
        content = "";
        string[] contents = new string[] {"Carte", "Protection", "Bottes de Pégase", "Bottes de Plomb","Casque de CRS","Disparition","Ressort","Champignon","Sérum d'Urgence","Catalyseur"};
        int a = 0;
        while (a < 10)
        {
            if (modif[a])
            {
                content += "\n"+contents[a];
            }
            a += 1;
        }

        if (content != "")
        {
            content = "Power-Up Actuels :" + content;
        }
    }
}
