using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    public class PlayerNetwork : MonoBehaviour
    {
        [HideInInspector]
        // Whether this player is handle by the client
        public bool isLocal;

        // Start is called before the first frame update
        void Start()
        {
            if (!isLocal)
            {
                // Remove camera and behaviour
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(GetComponent<Player>());
                Destroy(GetComponent<PlayerMaster>());

                // TODO : Set labels' camera (after all players have spawned)
            }
            else
            {
                // Remove labels
                Destroy(GetComponentInChildren<LookToCam>().gameObject);

                // Start player component
                var p = GetComponent<Player>();
                p.net = this;
                p.StartAfterPlayerNetwork();

                // TODO : Remove labels
            }

            PlayerMaster.RegisterPlayer(gameObject);
        }

        // Called when all players are in game before OnGameBegin
        public void PrepareGame()
        {
            // Look to cam
            //if (!isLocal)
            //    GetComponentInChildren<LookToCam>().cam = Camera.main.transform;
        }
    }
}
