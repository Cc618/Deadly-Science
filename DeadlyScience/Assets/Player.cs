using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public float gravity;
    [Range(0, 25)]
    public float acceleration;
    [Range(0, 25)]
    public float maxSpeed;
    [Range(0, 25)]
    public float jumpForce;
    // Ground friction
    [Range(0, 100)]
    public float friction;
    // Air friction
    [Range(0, 100)]
    public float damping;

    // TODO : In settings
    public float mouseSensivity;

    public Transform groundSensor;
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        grounded = Physics.CheckSphere(groundSensor.position, .1f, groundMask);
        animator.SetBool("grounded", grounded);

        if (grounded)
        {
            // Jump
            if (Input.GetKeyDown(Game.inputs.jump))
                velocity.y += jumpForce;
            else
                velocity.y = -.5f;
        }
        // Gravity
        else
            velocity.y += gravity * Time.deltaTime;

        // TODO : Cam
        // Rotation
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime,
            transform.rotation.eulerAngles.z
        );

        float x = 0;
        float z = 0;

        // Translation
        if (Input.GetKey(Game.inputs.left))
            x -= acceleration;

        if (Input.GetKey(Game.inputs.right))
            x += acceleration;

        if (Input.GetKey(Game.inputs.forward))
            z += acceleration;

        if (Input.GetKey(Game.inputs.backward))
            z -= acceleration;

        // Compute movement force
        Vector3 movements = x * transform.right + z * transform.forward;

        // Update movements if they serve to brake or they are within speed bounds
        if (movements.sqrMagnitude > .1 && (velocity.x * velocity.x + velocity.y * velocity.y < maxSpeed * maxSpeed || Vector3.Dot(velocity, movements) < 0))
            velocity += movements * Time.deltaTime;
        // If no movements
        else
        {
            // Friction
            if (grounded)
            {
                velocity.x -= velocity.x * Time.deltaTime * friction;
                velocity.z -= velocity.z * Time.deltaTime * friction;
            }
            else
            {
                velocity.x -= velocity.x * Time.deltaTime * damping;
                velocity.z -= velocity.z * Time.deltaTime * damping;
            }
        }

        // Update position
        controller.Move(velocity * Time.deltaTime);
        animator.SetBool("moving", velocity.x * velocity.x + velocity.z * velocity.z > 2f);
    }

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity = new Vector3();
    private bool grounded = false;
}
