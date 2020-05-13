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
using TMPro.Examples;

namespace ds
{
    public partial class Player : MonoBehaviour
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

        public static bool[] alterations = new bool[4];

        public Transform groundSensor;
        public LayerMask groundMask;

        public float Stamina
        {
            get => stamina;
            set
            {
                // Stunned
                if (value <= 0)
                {
                    value = 0;
                    stunned = true;
                }
                else if (value >= 1)
                {
                    value = 1;
                    stunned = false;
                }

                stamina = value;
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

        [Range(0, 5)]
        public float attackRange;

        InputManager inputManager;
        
        [HideInInspector]
        public PlayerNetwork net;

        [HideInInspector]
        public bool canMove = false;

        // Speed modifier
        [HideInInspector]
        public float speedRatio = 1f;

        // Called after the script PlayerNetwork
        public void StartAfterPlayerNetwork()
        {
            networkInit = true;

            inputManager = FindObjectOfType<InputManager>();
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            state = GetComponentInChildren<PlayerState>();
            
            // Disable render only if this is the current player
            GetComponentInChildren<Renderer>().enabled = false;
            
            // Update player network
            PlayerNetwork.localPlayer = this;

            net.SendPlayerReady();
        }

        // Called by PlayerMaster when all players are in game
        // (Phases have begun)
        public void OnGameBegin()
        {
            canMove = true;

            StartCoroutine(SyncNet());
        }

        void Update()
        {
            // Don't update if the network is not set up
            if (!(networkInit && canMove) || Game.EscapeMenuOpen || EndGame.Victory.activeSelf || EndGame.Defeat.activeSelf)
                return;

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

            // Cam virtual rotation
            if (!Game.EscapeMenuOpen)
                transform.rotation = Quaternion.Euler(
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * Game.settings.mouseSensivity * Screen.width / 60,
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
            float speedFactor = stunned ? stunnedSpeedFactor : speedRatio;

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

            // Attack
            if (!stunned && Input.GetKeyDown(KeyCode.Mouse0))
                Attack();

            // TODO : rm Test
            if (Input.GetKeyDown(KeyCode.H))
                net.SendSetStatus(PlayerState.PlayerStatus.HEALED);
            if (Input.GetKeyDown(KeyCode.R))
                net.SendSetStatus(PlayerState.PlayerStatus.REVENGE);
            if (Input.GetKeyDown(KeyCode.G))
            {
                OnPowerUpCollect();
                net.SendSetStatus(PlayerState.PlayerStatus.GHOST);
			}
            if (Input.GetKeyDown(KeyCode.I))
			{
                net.SendSetStatus(PlayerState.PlayerStatus.INFECTED);
			}
        }

        // Player to player hit
        // (This player hurts the other player)
        public void Hit(GameObject player)
        {
            // TODO : Updt
            var pState = player.GetComponent<PlayerState>();
            var pNet = player.GetComponent<PlayerNetwork>();

            if (!pNet)
            {
                Debug.Log(player);
                Debug.LogError("Can't access PlayerNetwork component when hit");
                return;
            }

            if (pState.Status == PlayerState.PlayerStatus.HEALED &&
                state.Status == PlayerState.PlayerStatus.REVENGE)
                // Change status
                pNet.SendSetStatus(PlayerState.PlayerStatus.REVENGE);
#if HARDMODE
            else if (pState.Status == PlayerState.PlayerStatus.HEALED &&
                state.Status == PlayerState.PlayerStatus.INFECTED)
                // Change status
                pNet.SendSetStatus(PlayerState.PlayerStatus.INFECTED);
#endif
            else
            {
                // Change stamina
                pNet.SendHit(strength);
            }
        }

        // When the player controlled by the client hits another player
        // and makes a move
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Not a player
            if (hit.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;
            
            var pState = hit.gameObject.GetComponent<PlayerState>();
            var pNet = hit.gameObject.GetComponent<PlayerNetwork>();

            // This player is in revenge mode
            if (state.Status == PlayerState.PlayerStatus.HEALED &&
                pState.Status == PlayerState.PlayerStatus.REVENGE)
                net.SendSetStatus(PlayerState.PlayerStatus.REVENGE);
            // The other player is in revenge mode
            else if (pState.Status == PlayerState.PlayerStatus.HEALED &&
                state.Status == PlayerState.PlayerStatus.REVENGE)
                pNet.SendSetStatus(PlayerState.PlayerStatus.REVENGE);
#if HARDMODE
            // This player is infected
            else if (state.Status == PlayerState.PlayerStatus.HEALED &&
                pState.Status == PlayerState.PlayerStatus.INFECTED)
                net.SendSetStatus(PlayerState.PlayerStatus.INFECTED);
            // The other player is infected
            else if (pState.Status == PlayerState.PlayerStatus.HEALED &&
                state.Status == PlayerState.PlayerStatus.INFECTED)
                pNet.SendSetStatus(PlayerState.PlayerStatus.INFECTED);
#endif
        }

        public void OnSerumCollect(int serumId)
        {
            // Remote call
            net.SendOnSerum(serumId);
        }
        
        public void OnPowerUpCollect()
        {
            int x = 0;
            List<string> contents = new List<string>();
            if (!alterations[0])
            {
                contents.Add("Carte");
                Map.instance.Change(false);
            }
            if (!alterations[1])
            {
                contents.Add("Protection");
            }
            if (contents.Count != 2)
            {
                contents.Add("Décharge");
            }
            contents.Add("Tourelle");
            int a = Random.Range(0,contents.Count);
            AffichagePowerUp.Nature = "l'Objet "+contents[a]+".";
            AffichagePowerUp.affich = true;
            switch (contents[a])
            {
                case "Carte":
                {
                    Map.instance.Change(true);
                    alterations[0] = true;
                    Map.instance.Change(true);
                    break;
                }
                case "Protection":
                {
                    alterations[1] = true;
                    break;
                }
                case "Décharge":
                {
                    if (alterations[1])
                    {
                        alterations[1] = false;
                    }
                    else
                    {
                        a = alterations.Length;
                        while (a > 0)
                        {
                            a -= 1;
                            alterations[a] = false;
                        }
                        Map.instance.Change(false);
                    }
                    break;
                }
                case "Tourelle":
                {
                    if (alterations[1])
                    {
                        alterations[1] = false;
                    }
                    else
                    {
                        Stamina = 0;
                    }
                    break;
                }
                case "Bottes de Plomb":
                {
                    if (alterations[1])
                    {
                        alterations[1] = false;
                    }
                    else
                    {
                        alterations[2] = false;
                        alterations[3] = true;
                        speedRatio = 0.5f;
                    }
                    break;
                }
                case "Bottes de Pégase":
                {
                    alterations[2] = true;
                    alterations[3] = false;
                    speedRatio = 2f;
                    break;
                }
            }
            AffichagePowerUpJoueur.MaJ(alterations);
            StartCoroutine(Attente());
            // Remote call
        }

        IEnumerator Attente()
        {
            yield return new WaitForSeconds(5);
            AffichagePowerUp.affich = false;
        }
        void Attack()
        {
            // Collide only players
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.forward), out hit, attackRange, layerMask))
            {
                Hit(hit.collider.gameObject);
                Audio.Play("hit");
                Particles.Spawn("hit", hit.point);
            }
            else
                Audio.Play("hit_failed");
        }

        private PlayerState state;
        private CharacterController controller;
        private Animator animator;
        private Vector3 velocity = new Vector3();
        private bool grounded = false;
        // Whether we've called StartAfterPlayerNetwork()
        private bool networkInit = false;
    }

    // Net part
    public partial class Player
    {
        static float syncEvery = .3f;

        IEnumerator SyncNet()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(syncEvery);
                net.SendSyncNet(stamina, stunned);
            }
        }

        // Between 0 and 1
        private float stamina = 1;
        // When stamina = 0, can't move
        private bool stunned = false;
    }
}
