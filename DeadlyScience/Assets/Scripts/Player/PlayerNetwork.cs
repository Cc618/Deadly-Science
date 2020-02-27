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
        [HideInInspector]
        // Whether this player handles the game
        public bool isMaster;

        // Start is called before the first frame update
        void Start()
        {
            if (!isLocal)
            {
                // Remove camera and behaviour
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(GetComponentInChildren<Player>());

                // TODO : Set labels' camera (after all players have spawned)
            }
            else
            {
                // Start player component
                GetComponent<Player>().StartAfterPlayerNetwork();

                // TODO : Remove labels
            }
        }
    }
}
