using System.Collections;
using System.Collections.Generic;
using ds;
using Photon.Pun;
using UnityEngine;
using Player = Photon.Realtime.Player;

public abstract class PowerUp : MonoBehaviour
{
    private PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        // TODO : Player sensor collides with this layer
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO : Change remote
        if ((other.gameObject.layer & playerLayer) != 0)
        {
            int a = OnCollect(other.gameObject);
            if (a>0)
            {
                int x = (int) (PlayerNetwork.local.gameObject.transform.position.x-0.5)/4;
                int y = (int) (PlayerNetwork.local.gameObject.transform.position.z-0.5)/4;
                print("Pris : "+x + " " + y);
                int s = y * CreateRoomMenu.Xm + x;
                print(s);
                PlayerNetwork.local.PowerUpPris(s);
                if (a == 1)
                {
                    pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                    PhotonNetwork.Destroy(gameObject);
                    pv.TransferOwnership(PhotonNetwork.MasterClient);
                }
            }
        }
    }

    // When a player hits the power up
    // Returns whether we must remove the power up
    protected abstract int OnCollect(GameObject player);

    private int playerLayer;
}
