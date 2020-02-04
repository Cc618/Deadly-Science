using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerBehaviour : MonoBehaviour
{
    public enum Status
    {
        INFECTED,
        HEALED,
        // TODO : GHOST for dead players (No collisions with others)
    }

    // Movement speed
    [Range(0, 100)]
    public float acceleration;
    public float jumpForce;
    [Range(0, 25)]
    public float maxSpeed;

    // TODO : Mouse Speed in settings
    [Range(0, 10)]
    public float camSpeed;

    public Material infectedMaterial;
    public Material healedMaterial;
    public MeshRenderer stateIndicator;

    // TODO : Private
    public Animator animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        groundSensor = GetComponentInChildren<PlayerGroundSensor>();

        // Set default material
        stateIndicator.material = infectedMaterial;
    }

    void Update()
    {
        Vector3 movementForce = new Vector3();

        // Translation
        if (Input.GetKey(Game.inputs.left))
            movementForce = -transform.right;

        if (Input.GetKey(Game.inputs.right))
            movementForce += transform.right;

        if (Input.GetKey(Game.inputs.forward))
            movementForce += transform.forward;

        if (Input.GetKey(Game.inputs.backward))
            movementForce -= transform.forward;

        // Add force if we haven't reached the max speed
        // or if the movement is against the current velocity
        if (body.velocity.sqrMagnitude < maxSpeed * maxSpeed || Vector3.Dot(body.velocity, movementForce) < 0)
            body.AddForce(movementForce * acceleration);

        // Jump
        if (groundSensor.isGrounded && Input.GetKeyDown(Game.inputs.jump))
            body.AddForce(new Vector3(0, 1) * jumpForce);

        // Rotation
        body.rotation = Quaternion.Euler(
            body.rotation.eulerAngles.x,
            body.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * camSpeed,
            body.rotation.eulerAngles.z
        );

        // Animation
        animator.SetBool("grounded", groundSensor.isGrounded);
        animator.SetBool("moving", body.velocity.x * body.velocity.x + body.velocity.z * body.velocity.z > .2f);

        // TODO : rm
        if (Input.GetKey(KeyCode.X))
            animator.SetBool("moving", true);
        else
            animator.SetBool("moving", false);

    }

    public void SetStatus(Status status)
    {
        // Update material
        // TODO : Update also anim...
        switch (status)
        {
            case Status.HEALED:
                stateIndicator.material = healedMaterial;
                break;
            case Status.INFECTED:
                stateIndicator.material = infectedMaterial;
                break;
        }

        // Update status
        this.status = status;
    }

    private Rigidbody body;
    private PlayerGroundSensor groundSensor;
    private Status status = Status.INFECTED;
}
