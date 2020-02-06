using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructeur : MonoBehaviour
{
    public Transform _prefab;
    public Quaternion Rotate;

    public int xm;

    public int zm;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int x = 0;
        while (x < xm)
        {
            int z = 0;
            while (z < zm)
            {
                z += 1;
                Transform newCube = (Transform)Instantiate(_prefab, new Vector3((float)(x+0.5),(float)0.7,(float)(z+0.5)), Rotate);
            }
            x += 1;
        }
    }
}
