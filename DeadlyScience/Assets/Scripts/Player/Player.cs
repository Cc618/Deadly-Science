// The behaviour of the player
// This script is executed only if
// this is the player controlled by the user
// not network players

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Photon.Pun;

namespace ds
{
    public class Player : MonoBehaviour
    {
        
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

        public Transform groundSensor;
        public LayerMask groundMask;

        // TODO : Move these fields in another class
        public PlayerName nameUi;
        public PlayerStamina staminaUi;

        // TODO : Move in PlayerState
        // Between 0 and 1
        private float stamina = 1;
        public float Stamina
        {
            get => stamina;
            set
            {
                // TODO : Update stamina by network
                if (value <= 0)
                {
                    value = 0;
                    stunned = true;
                    staminaUi.ChangeStunned(stunned);

                    // TODO: rm
                    Debug.Log("Player -> Stunned");
                }
                else if (value >= 1)
                {
                    value = 1;
                    stunned = false;
                    staminaUi.ChangeStunned(stunned);
                }

                stamina = value;
                staminaUi.Value = value;
            }
        }

        // How many stamina removes the player for a hit
        [Range(0, 1)]
        public float strength;
        // How many stamina the player gain
        [Range(0, 1)]
        public float regeneration;
        // The speed is affected when the player is tunned by this factor
        [Range(0, 1)]
        public float stunnedSpeedFactor;

        InputManager inputManager;
        
        [HideInInspector]
        public PlayerNetwork net;

        // Called after the script PlayerNetwork
        public void StartAfterPlayerNetwork()
        {
            inputManager = FindObjectOfType<InputManager>();
            
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            Stamina = stamina;

            // Disable render only if this is the current player
            GetComponentInChildren<Renderer>().enabled = false;

            // TODO
            //if (PhotonNetwork.IsMasterClient)
            //    Debug.Log("MASTER");
            //// TODO : Wait when all players are connected
            //// Begin game
            //    if (net.isMaster)
            //    StartPhases();
            
            // Update player network
            PlayerNetwork.localPlayer = this;

            
        }

        // Called by PlayerMaster when all players are in game
        // (Phases have begun)
        public void OnGameBegin()
        {
            Debug.Log("Player : OnGameBegin");
        }

        void Update()
        {
            // Health regeneration
            Stamina += Time.deltaTime * regeneration;

            // On ground check (only if the velocity is almost negative)
            grounded = velocity.y <= .1f && Physics.CheckSphere(groundSensor.position, controller.radius, groundMask);
            animator.SetBool("grounded", grounded);

            if (grounded)
            {
                // Jump only if not stunned
                if (!stunned && inputManager.IsButtonDown("Jump"))
                    velocity.y += jumpForce;
                else
                    // If grounded and not on jump add a small force
                    velocity.y = -.1f;
            }
            // Gravity
            else
                velocity.y += gravity * Time.deltaTime;

            // Rotation
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * Game.settings.mouseSensivity * Time.deltaTime,
                transform.rotation.eulerAngles.z
            );

            float x = 0;
            float z = 0;

            // Translation
            if (inputManager.IsButtonDown("Left"))
                x -= acceleration;

            if (inputManager.IsButtonDown("Right"))
                x += acceleration;

            if (inputManager.IsButtonDown("Forward"))
                z += acceleration;

            if (inputManager.IsButtonDown("Backward"))
                z -= acceleration;

            // Compute movement force
            Vector3 movements = x * transform.right + z * transform.forward;

            // Squared tangent speed
            float tangentSpeed = velocity.x * velocity.x + velocity.z * velocity.z;

            // The speed ratio
            float speedFactor = stunned ? stunnedSpeedFactor : 1;

            // Update movements if they serve to brake or they are within speed bounds
            if (movements.sqrMagnitude > .1 && (tangentSpeed < maxSpeed * maxSpeed * speedFactor * speedFactor || velocity.x * movements.x + velocity.z * movements.z < 0))
                velocity += movements * Time.deltaTime * speedFactor;
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

        // Player to player hit
        // TODO : Hit with other player when network
        public void Hit(/* TODO Player player */)
        {
            // TODO : status == player.status
            bool sameStatus = true;

            if (sameStatus)
                if (!stunned)
                    // TODO : player.strength
                    Stamina -= strength;
            // TODO : Else if infected...
        }

        private CharacterController controller;
        private Animator animator;
        private Vector3 velocity = new Vector3();
        private bool grounded = false;
        // When stamina = 0, can't move
        private bool stunned = false;
    }
}
