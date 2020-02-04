using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpForce;

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
            x = -speed;

        if (Input.GetKey(Game.inputs.right))
            x = speed;

        if (Input.GetKey(Game.inputs.forward))
            z = speed;

        if (Input.GetKey(Game.inputs.backward))
            z = -speed;

        // Update position
        controller.Move((velocity + x * transform.right + z * transform.forward) * Time.deltaTime);
        animator.SetBool("moving", x * x + z * z > .2f);
    }

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity = new Vector3();
    private bool grounded = false;
}
