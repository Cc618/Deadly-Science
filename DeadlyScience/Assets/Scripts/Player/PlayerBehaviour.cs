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

    // TODO : Mouse Speed in settings
    [Range(0, 10)]
    public float camSpeed;

    public Material infectedMaterial;
    public Material healedMaterial;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        groundSensor = GetComponentInChildren<PlayerGroundSensor>();

        // Set default material
        meshRenderer.material = infectedMaterial;
    }

    void Update()
    {
        // Translation
        if (Input.GetKey(Game.inputs.left))
            body.AddForce(-transform.right * speed);

        if (Input.GetKey(Game.inputs.right))
            body.AddForce(transform.right * speed);

        if (Input.GetKey(Game.inputs.forward))
            body.AddForce(transform.forward * speed);

        if (Input.GetKey(Game.inputs.backward))
            body.AddForce(-transform.forward * speed);

        // Jump
        if (groundSensor.isGrounded && Input.GetKeyDown(Game.inputs.jump))
            body.AddForce(new Vector3(0, 1) * jumpForce);

        // Rotation
        body.rotation = Quaternion.Euler(
            body.rotation.eulerAngles.x,
            body.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * camSpeed,
            body.rotation.eulerAngles.z
        );
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
