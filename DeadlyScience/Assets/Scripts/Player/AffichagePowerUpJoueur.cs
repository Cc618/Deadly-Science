using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichagePowerUpJoueur : MonoBehaviour
{
    public Text scoreText;
    public static string content = "";
    private static int lang;

    private void Start()
    {
        lang = PlayerPrefs.GetInt("language");
    }

    void Update()
    {
        scoreText.text = content;
    }

    public static void MaJ(bool[] modif)
    {
        content = "";
        string[] contents;
        if (lang == 1)
            contents = new string[] { "Carte", "Protection", "Bottes de Pégase", "Bottes de Plomb", "Casque de CRS", "Disparition", "Ressort", "Champignon", "Sérum d'Urgence", "Catalyseur" };
        else
            contents = new string[] { "Map", "Protection", "Pegasus Boots", "Lead Boots", "Riot police helmet", "Invisibility", "Spring", "Mushroom", "Emergency serum", "Catalyst" };
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
            if (PlayerPrefs.GetInt("language") == 1)
                content = "Power-Up Actuels :" + content;
            else
                content = "Active Power-up :" + content;
        }
    }
}
