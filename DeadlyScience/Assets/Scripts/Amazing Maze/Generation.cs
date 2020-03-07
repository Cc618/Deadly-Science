using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro.Examples;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [Range(0, 4)] 
    public PhotonView PV;
    public float serumHeight;
    public int xm;
    public int zm;
    public Transform _prefab;
    public Transform Tourner;
    public Transform Tout_droit;
    public Transform Bifurc;
    public Transform Bifurcc;
    public Transform Carrefour;
    public Transform Impasse;
    public Transform Serum;
    public bool version;

    public static int[]
        Aleatoire(int n,
            int m) //Cette fonction génère une liste de quatres termes aléatoires (de 0 à 3) sans répétition. En gros, elle mélange...
    {
        int[] Directions = new int[n];
        int[] Avendre = new int[m];
        int a = 0;
        while (a < m)
        {
            Avendre[a] = a;
            a += 1;
        }

        a = 0;
        while (a < n)
        {
            Directions[a] = -1;
            a += 1;
        }

        a = 0;
        while (a < n)
        {
            int b = Random.Range(0, m - a -1);
            while (Avendre[b] == -1)
            {
                b += 1;
                if (b == m)
                {
                    b = 0;
                }
            }
            Avendre[b] = -1;
            Directions[a] = b;
            a += 1;
        }
        return Directions;
    }

    public static int[] Generateur(int x, int y)
    {
        //Création des variables
        int[] Plan = new int[x * y];
        int a = 0;
        while (a < Plan.Length)
        {
            Plan[a] = -1;
            a += 1;
        }

        int Case = 0;
        int n = 0;
        int CaseRemplies = 0;
        int CaseActuelle = 0;
        int CaseCible = 0;
        int[] Directions = {0, 0, 0, 0};
        int Precedent = -1;
        List<int> Chemin = new List<int>();
        //Tant que toutes les cases n'ont pas été connectées à d'autres, on continue de créer des voies.
        while (CaseRemplies < Plan.Length)
        {
            //Si on n'est pas sur la première case, on choisit une case encore inexplorée et on la connecte directement aux chemins déjà formés.
            while (CaseRemplies < Plan.Length - 1 && Plan[CaseRemplies] != -1)
            {
                CaseRemplies += 1;
            }

            if (CaseRemplies > 0)
            {
                if (CaseRemplies < x)
                {
                    Plan[CaseRemplies - 1] -= 2;
                }
                else
                {
                    if (Plan[CaseRemplies - x] % 2 == 1)
                    {
                        Plan[CaseRemplies - x] -= 1;
                    }
                }
            }

            if (CaseRemplies < Plan.Length - 1)
            {
                //On crée un nouveau trajet. Le trajet prend fin jusqu'à rencontrer une case déjà visitée ou ne plus pouvoir se déplacer ; il ne peut pas repasser deux fois par la meme case.
                Plan[CaseRemplies] = 3;
                CaseActuelle = CaseRemplies;
                Chemin = new List<int> {CaseActuelle};
                n += 1;
                Precedent = -1;
                int Distance = 0;
                while (CaseActuelle > -1)
                {
                    Directions = Aleatoire(4,4);
                    bool valide = false;
                    int analysea = 0;
                    //A chaque case du trajet, on choisit un ordre de priorité pour les directions. On choisit alors la direction prioritaire possible (ne sont pas possibles les directions qui mènent sur une case du trajet ou qui sortent des limites du dédale).
                    while (!valide && analysea < 4)
                    {
                        if ((Directions[analysea] == 0 && CaseActuelle % x == x - 1) ||
                            (Directions[analysea] == 1 && CaseActuelle >= x * (y - 1)) ||
                            (Directions[analysea] == 2 && CaseActuelle % x == 0) ||
                            (Directions[analysea] == 3 && CaseActuelle < x) ||
                            Directions[analysea] == (Precedent + 2) % 4)
                        {
                            analysea += 1;
                        }
                        else
                        {
                            valide = true;
                            CaseCible = CaseActuelle + (new int[] {1, x, -1, -x})[Directions[analysea]];
                            int analyseb = 0;
                            ;
                            while (valide && analyseb < Chemin.Count)
                            {
                                if (CaseCible == Chemin[analyseb])
                                {
                                    valide = false;
                                }

                                analyseb += 1;
                            }

                            if (!valide)
                            {
                                analysea += 1;
                            }
                            else
                            {
                                //La case est valide, on rajoute alors le chemin pour la connecter au trajet. Et on recommence sur la nouvelle case, jusqu'à tomber sur une case déjà ouverte ou ne plus pouvoir se déplacer.
                                analysea = Directions[analysea];
                                Precedent = analysea;
                                int modif = 2 - (analysea % 2);
                                if (analysea < 2)
                                {
                                    analysea = CaseActuelle;
                                }
                                else
                                {
                                    analysea = CaseCible;
                                }

                                if (Plan[CaseCible] == -1)
                                {
                                    Distance += 1;
                                    if (Distance == x + y)
                                    {
                                        Chemin = new List<int>();
                                        Distance = 0;
                                    }

                                    Plan[CaseCible] = 3;
                                    Plan[analysea] -= modif;
                                    CaseActuelle = CaseCible;
                                    Chemin.Add(CaseActuelle);
                                    analysea = 0;
                                }
                                else
                                {
                                    if (Plan[analysea] == 3 || Plan[analysea] == modif)
                                    {
                                        Plan[analysea] -= modif;
                                    }

                                    CaseActuelle = -1;
                                }
                            }
                        }
                    }

                    if (analysea == 4)
                    {
                        CaseActuelle = -1;
                    }
                }
            }
            else
            {
                Plan[CaseRemplies] = 3;
                if (CaseRemplies != x * y - 1)
                {
                    Plan[CaseRemplies] = 4;
                }

                CaseRemplies += 1;
            }
        }

        //Ici, on a déjà un Dédale convenable. Mais on peut parfaitement trouver des configurations de "piliers"... Les lignes suivantes permettent de dédecter les piliers et colmatent l'un des quatre murs (aléatoirement, bien sur).
        Case = 0;
        while (Case < y - 1)
        {
            a = 0;
            while (a < x - 1)
            {
                if (Plan[Case * x + a] == 0 && Plan[Case * x + a + 1] % 2 == 0 && Plan[(Case + 1) * x + a] < 2)
                {
                    int b = Random.Range(0, 3);
                    if (b == 0)
                    {
                        Plan[Case * x + a] += 2;
                    }

                    if (b == 1)
                    {
                        Plan[Case * x + a] += 1;
                    }

                    if (b == 2)
                    {
                        Plan[Case * x + a + 1] += 1;
                    }

                    if (b == 3)
                    {
                        Plan[(Case + 1) * x + a] += 2;
                    }
                }

                a += 1;
            }

            Case += 1;
        }

        //J'ai également pensé à ce petit dédale. Il est déjà défini, il faudra juste effectuer un changement de variables si l'on veut le retourner. Ce sera pratique pour les tests...
        int[] Planf = new int[] {0, 2, 0, 1, 3, 3, 0, 3, 1, 2, 0, 2, 0, 0, 3, 2, 1, 3, 2, 2, 1, 1, 3, 1, 3};

        return Planf;

        // TODO
        //return Plan;
    }

    // Start is called before the first frame update
    void Start()
    {
        //On redéfinit la taille et la position du sol.
        int[] Plan = Generateur(xm, zm);
        int x = 0;
        int z = 0;
        if (version)
        {
            transform.localScale += new Vector3(2 * xm, 0, 2 * zm);
            transform.position = transform.position + new Vector3((float) (xm + 0.5), 0, (float) (zm + 0.5));
            //On prend le dédale créé par la fonction Générateur. Au début, on crée deux murs extérieurs.
            while (x < xm)
            {
                Transform newCube1 = (Transform) Instantiate(_prefab,
                    new Vector3((float) (2 * x + 1.5), (float) 0.7, (float) (0.5)), new Quaternion(0, 0, 0, 0));
                Transform newCube2 = (Transform) Instantiate(_prefab,
                    new Vector3((float) (2 * x + 2.5), (float) 0.7, (float) (0.5)), new Quaternion(0, 0, 0, 0));
                x += 1;
            }

            Transform newCube3 = (Transform) Instantiate(_prefab,
                new Vector3((float) (0.5), (float) 0.7, (float) (0.5)), new Quaternion(0, 0, 0, 0));
            x = 0;
            z = 0;
            while (z < zm)
            {
                Transform newCube4 = (Transform) Instantiate(_prefab,
                    new Vector3((float) (0.5), (float) 0.7, (float) (2 * z + 1.5)), new Quaternion(0, 0, 0, 0));
                Transform newCube5 = (Transform) Instantiate(_prefab,
                    new Vector3((float) (0.5), (float) 0.7, (float) (2 * z + 2.5)), new Quaternion(0, 0, 0, 0));
                z += 1;
            }
        }
        //Ensuite, pour chaque case, on va créer les deux murs opposés aux murs que l'on vient de construire.
        bool room = false;
        while (x < xm)
        {
            z = 0;
            while (z < zm)
            {
                //if (version && PV.IsMine)
                if (version)
                {
                    Transform newCube = (Transform) Instantiate(_prefab,
                        new Vector3((float) (2 * x + 2.5), (float) 0.7, (float) (2 * z + 2.5)),
                        new Quaternion(0, 0, 0, 0));
                    if (Plan[x * zm + z] > 1)
                    {
                        Transform newCube6 = (Transform) Instantiate(_prefab,
                            new Vector3((float) (2 * x + 1.5), (float) 0.7, (float) (2 * z + 2.5)),
                            new Quaternion(0, 0, 0, 0));
                    }

                    if (Plan[x * zm + z] % 2 == 1)
                    {
                        Transform newCube7 = (Transform) Instantiate(_prefab,
                            new Vector3((float) (2 * x + 2.5), (float) 0.7, (float) (2 * z + 1.5)),
                            new Quaternion(0, 0, 0, 0));
                    }
                }
                else
                {
                    bool[] passages = {false,false,false,false};
                    if (Plan[x * zm + z] > 1)
                    {
                        passages[1] = true;
                    }

                    if (Plan[x * zm + z] % 2 == 1)
                    {
                        passages[2] = true;
                    }

                    if (x == 0)
                    {
                        passages[0] = true;
                    }
                    else
                    {
                        if (Plan[(x - 1) * zm + z] %2 == 1)
                        {
                            passages[0] = true;
                        }
                    }
                    if (z == 0)
                    {
                        passages[3] = true;
                    }
                    else
                    {
                        if (Plan[x * zm + z-1] > 1)
                        {
                            passages[3] = true;
                        }
                    }
                    int p = 0;
                    int q = 0;
                    while (q < 4)
                    {
                        if (passages[q])
                        {
                            p += 1;
                        }
                        q += 1;
                    }
                    int rotate = 0;
                    if (p == 2)
                    {
                        if (passages[0] == passages[2])
                        {
                            if (passages[0])
                            {
                                rotate = 90;
                            }
                            Transform newCube = (Transform) Instantiate(Tout_droit,
                                new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                new Quaternion(0, rotate, 0, rotate));
                        }
                        else
                        {
                            if (passages[0])
                            {
                                if (passages[1])
                                {
                                    Transform newCube = (Transform) Instantiate(Tourner,
                                        new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                        new Quaternion(0, -90, 0, 90));
                                }
                                else
                                {
                                    Transform newCube = (Transform) Instantiate(Tourner,
                                        new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                        new Quaternion(0, 180, 0, 0));
                                }
                            }
                            else
                            {
                                if (passages[1])
                                {
                                    Transform newCube = (Transform) Instantiate(Tourner,
                                        new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                        new Quaternion(0, 0, 0, 0));
                                }
                                else
                                {
                                    Transform newCube = (Transform) Instantiate(Tourner,
                                        new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                        new Quaternion(0, 90, 0, 90));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (p == 3)
                        {
                            int a = 0;
                            Transform newCube = (Transform) Instantiate(Impasse,
                                new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                new Quaternion(0, 90, 0, 90));
                            newCube.transform.Rotate(0, 90, 0);
                            while (passages[a])
                            {
                                a += 1;
                                newCube.transform.Rotate(0, 90, 0);
                            }
                        }

                        if (p == 1)
                        {
                            int a = 0;
                            Transform newCube;
                            if (room)
                            {
                                newCube = (Transform) Instantiate(Bifurc,
                                    new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                    new Quaternion(0, 90, 0, 90));
                            }
                            else
                            {
                                newCube = (Transform) Instantiate(Bifurcc,
                                    new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                    new Quaternion(0, 90, 0, 90));
                            }
                            newCube.transform.Rotate(0, -90, 0);
                            while (!passages[a])
                            {
                                a += 1;
                                newCube.transform.Rotate(0, 90, 0);
                            }
                            room = !room;
                        }

                        if (p == 0)
                        {
                            Transform newCube = (Transform) Instantiate(Carrefour,
                                new Vector3((float) (4 * x + 2.5), (float) 0.7, (float) (4 * z + 2.5)),
                                new Quaternion(0, 0, 0, 0));
                        }
                    }
                }
                z += 1;
            }
            x += 1;
        }
        //On rajoute trois sérums.
        int[] seru = { 3, 9, 20 }; // Aleatoire(3, xm * zm);
        int g = 0;
        while (g < 3)
        {
            Transform newCube = (Transform) Instantiate(Serum,
                new Vector3((float) (4 * (seru[g]%xm) + 2.5), serumHeight, (float) (4 * (seru[g]/xm) + 2.5)),
                new Quaternion(0, 0, 0, 0));
            g += 1;
        }
        //Et voila, c'est fini !
    }
}
