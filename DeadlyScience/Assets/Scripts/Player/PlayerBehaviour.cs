using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Movement speed
    public float speed;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement
        if (Input.GetKey(Game.inputs.left))
            body.AddForce(new Vector3(-1, 0) * speed);

        if (Input.GetKey(Game.inputs.right))
            body.AddForce(new Vector3(1, 0) * speed);

    }

    private Rigidbody body;
}
