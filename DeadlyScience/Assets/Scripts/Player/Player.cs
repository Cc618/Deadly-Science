// The behaviour of the player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace ds
{
    public class Player : MonoBehaviour
    {
        public enum PlayerStatus
        {
            INFECTED,
            HEALED,
            // TODO : GHOST for dead players (No collisions with others)
        }

        public float gravity;
        [Range(0, 100)]
        public float acceleration;
        [Range(0, 25)]
        public float maxSpeed;
        [Range(0, 25)]
        public float jumpForce;
        // Ground friction
        [Range(0, 10)]
        public float friction;
        // Air friction
        [Range(0, 10)]
        public float damping;

        // TODO : In settings
        public float mouseSensivity;

        public Transform groundSensor;
        public LayerMask groundMask;

        public PlayerName nameLabel;
        public Slider staminaSlider;

        private PlayerStatus status = PlayerStatus.INFECTED;
        public PlayerStatus Status
        {
            set
            {
                // Update status
                status = value;
                nameLabel.SetStatus(status);

                // Update material
                // TODO : Update also anim / sound...
                switch (status)
                {
                    case PlayerStatus.HEALED:
                        Debug.Log("Player has status HEALED");
                        //stateIndicator.material = healedMaterial;
                        break;
                    case PlayerStatus.INFECTED:
                        Debug.Log("Player has status INFECTED");
                        //stateIndicator.material = infectedMaterial;
                        break;
                }
            }

            get => status;
        }

        // Between 0 and 1
        private float stamina = 1;
        public float Stamina
        {
            get => stamina;
            set
            {
                if (value <= 0)
                {
                    value = 0;
                    stunned = true;

                    // TODO: rm
                    Debug.Log("Player -> Stunned");
                }
                else if (value >= 1)
                {
                    value = 1;
                    stunned = false;
                }

                stamina = value;
                staminaSlider.value = value;
            }
        }

        // How many stamina removes the player for a hit
        [Range(0, 1)]
        public float strength;
        // How many stamina the player gain
        [Range(0, 1)]
        public float regeneration;
        // TODO
        //// In seconds, how many time the player is stunned when stamina is 0
        //[Range(0, 1)]
        //public float stunnedTime;


        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            // TODO : test
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Stamina -= strength;


            // Health regeneration
            Stamina += Time.deltaTime * regeneration;

            // On ground check
            grounded = Physics.CheckSphere(groundSensor.position, .1f, groundMask);
            animator.SetBool("grounded", grounded);

            if (grounded)
            {
                // Jump
                if (!stunned && Input.GetKeyDown(Game.inputs.jump))
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

            // Squared tangent speed
            float tangentSpeed = velocity.x * velocity.x + velocity.z * velocity.z;

            // Update movements if they serve to brake or they are within speed bounds
            if (!stunned && movements.sqrMagnitude > .1 && (tangentSpeed < maxSpeed * maxSpeed || velocity.x * movements.x + velocity.z * movements.z < 0))
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
            animator.SetBool("moving", tangentSpeed > 1.6f);
        }

        private CharacterController controller;
        private Animator animator;
        private Vector3 velocity = new Vector3();
        private bool grounded = false;
        // When stamina = 0, can't move
        private bool stunned = false;
    }
}
