using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public enum Status
    {
        INFECTED,
        HEALED,
        // TODO : GHOST for dead players (No collisions with others)
    }

    // Movement speed
    public float speed;
    public float jumpForce;

    public Material infectedMaterial;
    public Material healedMaterial;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        groundSensor = GetComponentInChildren<PlayerGroundSensor>();

        // Set default material
        meshRenderer.material = infectedMaterial;
    }

    void Update()
    {
        // Movement
        if (Input.GetKey(Game.inputs.left))
            body.AddForce(new Vector3(-1, 0) * speed);

        if (Input.GetKey(Game.inputs.right))
            body.AddForce(new Vector3(1, 0) * speed);

        if (groundSensor.isGrounded && Input.GetKeyDown(Game.inputs.jump))
            body.AddForce(new Vector3(0, 1) * jumpForce);
    }

    public void SetStatus(Status status)
    {
        // Update material
        // TODO : Update also anim...
        switch (status)
        {
            case Status.HEALED:
                meshRenderer.material = healedMaterial;
                break;
            case Status.INFECTED:
                meshRenderer.material = infectedMaterial;
                break;
        }

        // Update status
        this.status = status;
    }

    private Rigidbody body;
    private MeshRenderer meshRenderer;
    private PlayerGroundSensor groundSensor;
    private Status status = Status.INFECTED;
}
