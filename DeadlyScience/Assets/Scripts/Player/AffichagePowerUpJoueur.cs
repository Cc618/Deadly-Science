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
        string[] contents = new string[] {"Carte", "Protection", "PowerUp 3", "PowerUp 4"};
        int a = 0;
        while (a < 4)
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
