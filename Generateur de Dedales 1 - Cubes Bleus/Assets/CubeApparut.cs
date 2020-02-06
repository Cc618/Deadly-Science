using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform _prefab;
    public int x;
    public int z;
    public Quaternion Rotate;

    // Update is called once per frame
    void Update()
    {
        Transform newCube = (Transform)Instantiate(_prefab, new Vector3((float)(x+0.5),(float)0.6,(float)(z+0.5)), Rotate);
    }
}