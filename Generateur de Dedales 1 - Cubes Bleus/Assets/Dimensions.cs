using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimensions : MonoBehaviour
{
    public int xm;
    public int zm;

    public bool creation;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (creation)
        {
            transform.localScale += new Vector3(2*xm, 0, 2*zm);
            transform.position = transform.position + new Vector3((float)(xm + 0.5), 0, (float)(zm + 0.5));
            creation = false;
        }
    }
}
