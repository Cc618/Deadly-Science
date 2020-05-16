using System.Collections;
using System.Collections.Generic;
using ds;
using UnityEngine;

public class Lampe : MonoBehaviour
{
    public Light l;
    // Start is called before the first frame update
    void Start()
    {
        if (CreateRoomMenu.Mode != 1)
        {
            l.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
