using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Recap : MonoBehaviour
{
    public static TMP_Text Text;
    public static GameObject content;
    private static int time = 0;
    private static string Time = "";
    private static bool chrono;
    private int lang = PlayerPrefs.GetInt("language");

    private void Awake()
    {
        Text = GameObject.Find("EndScreen/Panel/ScrollView/Viewport/content/Recap").GetComponent<TMP_Text>();
        content = GameObject.Find("EndScreen/Panel/ScrollView/Viewport/content");
        if (lang == 1)
            Text.text = "Entrée dans le laboratoire";
        else
            Text.text = "Entered the lab";
    }

    private void Update()
    {
        if (!chrono)
            StartCoroutine(Attente());
    }

    public void AddRecap(string Event, bool objet = false)
    {
        TMP_Text go = (TMP_Text)Instantiate(Text);
        go.transform.SetParent(content.transform);
        go.transform.localScale = Vector3.one;

        if (objet)
        {
            if (lang == 1)
            {
                switch (Event)
                {
                    case "Carte":
                    case "Protection":
                    case "Décharge":
                    case "Paralysie":
                    case "Disparition":
                    case "Herbe Bleue":
                        go.text = $"{Event} ramassée à {Time}\n";
                        break;
                    case "Casque de CRS":
                    case "Ressort":
                    case "Champignon":
                    case "Sérum d'Urgence":
                    case "Catalyseur":
                        go.text = $"{Event} ramassé à {Time}\n";
                        break;
                    case "Bottes de Plomb":
                    case "Bottes de Pégase":
                        go.text = $"{Event} ramassées à {Time}\n";
                        break;
                }
            }
            else
                go.text = $"{Event} picked up at {Time}\n";
            
        }
        else
        {
            if (lang == 1)
                go.text = $"{Event} à {Time}\n";
            else
                go.text = $"{Event} at {Time}\n";
        }
    }

    IEnumerator Attente()
    {
        chrono = true;
        yield return new WaitForSeconds(1);
        time += 1;
        if (time < 60)
            Time = time.ToString() + "s";
        else
            Time = (time / 60).ToString() + "m" + (time % 60).ToString() + "s";
        chrono = false;
    }
}

