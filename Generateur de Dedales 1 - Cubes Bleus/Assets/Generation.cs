using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public int xm;
    public int zm;
    public bool newGame;
    public Transform _prefab;
    
    public static int[] Aleatoire()
    {
        int[] Directions = new int[] {-1, -1, -1, -1};
        int[] Avendre = new int[] {0, 1, 2, 3};
        int a = 0;
        while (a < 4)
        {
            int b = Random.Range(0, 3 - a);
            while (Avendre[b] == -1)
            {
                b += 1;
                if (b == 4)
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
        /*Création des variables*/
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
        /*Quand toutes les cases n'ont pas été connectées...*/
        while (CaseRemplies < Plan.Length)
        {
            while (CaseRemplies < Plan.Length - 1 && Plan[CaseRemplies] != -1)
            {
                CaseRemplies += 1;
            }
            //Affichage(x,y,Plan);
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
                Plan[CaseRemplies] = 3;
                CaseActuelle = CaseRemplies;
                Chemin = new List<int> {CaseActuelle};
                n += 1;
                //Console.WriteLine("Chemin "+n+", "+CaseActuelle);
                Precedent = -1;
                int Distance = 0;
                while (CaseActuelle > -1)
                {
                    Directions = Aleatoire();
                    bool valide = false;
                    int analysea = 0;
                    //Console.WriteLine(CaseActuelle);
                    while (!valide && analysea < 4)
                    {
                        if ((Directions[analysea] == 0 && CaseActuelle % x == x - 1) ||
                            (Directions[analysea] == 1 && CaseActuelle >= x * (y-1)) ||
                            (Directions[analysea] == 2 && CaseActuelle % x == 0) ||
                            (Directions[analysea] == 3 && CaseActuelle < x)||Directions[analysea]==(Precedent+2)%4) 
                        {
                            analysea += 1;
                        }
                        else
                        {
                            //Console.WriteLine("Direction : " + Directions[analysea]+", "+CaseActuelle);
                            valide = true;
                            CaseCible = CaseActuelle + (new int [] {1,x,-1,-x})[Directions[analysea]];
                            int analyseb = 0;
                            //Console.WriteLine("Cible : " + CaseCible + ", "+ Chemin[0]);
                            while (valide && analyseb < Chemin.Count)
                            {
                                if (CaseCible == Chemin[analyseb])
                                {
                                    valide = false;
                                }
                                analyseb += 1;
                            }
                            //Console.WriteLine("Résultat : "+valide);
                            if (!valide)
                            {
                                analysea += 1;
                            }
                            else
                            {
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
                                    if (Plan[analysea]==3||Plan[analysea]==modif)
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
                        CaseActuelle =- 1;
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
        Case = 0;
        while (Case < y-1)
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
                        Plan[Case * x + a+1] += 1;
                    }
                    if (b == 3)
                    {
                        Plan[(Case+1) * x + a] += 2;
                    }
                }

                a += 1;
            }

            Case += 1;
        }
        return Plan;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Ca marche !"); 
    }
    // Update is called once per frame
    void Update()
    {
        if (newGame)
        {
            Debug.Log("Construction...");
            newGame = false;
            int[] Plan = Generateur(xm, zm);
            int[] Planf = new int[] {0, 2, 0, 1, 3, 3, 0, 3, 1, 2, 0, 2, 0, 0, 3, 2, 1, 3, 2, 2, 1, 1, 3, 1, 3};
            int x = 0;
            while (x < xm)
            {
                Transform newCube1 = (Transform)Instantiate(_prefab, new Vector3((float)(2*x+1.5),(float)0.7,(float)(0.5)), new Quaternion(0,0,0,0));
                Transform newCube2 = (Transform)Instantiate(_prefab, new Vector3((float)(2*x+2.5),(float)0.7,(float)(0.5)), new Quaternion(0,0,0,0));
                x += 1;
            }
            Transform newCube3 = (Transform)Instantiate(_prefab, new Vector3((float)(0.5),(float)0.7,(float)(0.5)), new Quaternion(0,0,0,0));
            x = 0;
            int z = 0;
            while (z < zm)
            {
                Transform newCube4 = (Transform)Instantiate(_prefab, new Vector3((float)(0.5),(float)0.7,(float)(2*z+1.5)), new Quaternion(0,0,0,0));
                Transform newCube5 = (Transform)Instantiate(_prefab, new Vector3((float)(0.5),(float)0.7,(float)(2*z+2.5)), new Quaternion(0,0,0,0));
                z += 1;
            }
            while (x < xm)
            {
                z = 0;
                while (z < zm)
                {
                    Transform newCube = (Transform)Instantiate(_prefab, new Vector3((float)(2*x+2.5),(float)0.7,(float)(2*z+2.5)), new Quaternion(0,0,0,0));
                    if (Plan[x*zm+z] > 1)
                    {
                        Transform newCube6 = (Transform)Instantiate(_prefab, new Vector3((float)(2*x+1.5),(float)0.7,(float)(2*z+2.5)), new Quaternion(0,0,0,0));
                    }if (Plan[x*zm+z]%2 == 1)
                    {
                        Transform newCube7 = (Transform)Instantiate(_prefab, new Vector3((float)(2*x+2.5),(float)0.7,(float)(2*z+1.5)), new Quaternion(0,0,0,0));
                    }
                    z += 1;
                }
                x += 1;
            }
        }
    }
}
