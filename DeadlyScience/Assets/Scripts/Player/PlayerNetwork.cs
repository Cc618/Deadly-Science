using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ds
{
    public class PlayerNetwork : MonoBehaviour
    {
        [HideInInspector]
        // Whether this player is the handle by the client
        public bool isLocal;

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

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
