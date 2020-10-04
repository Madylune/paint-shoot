using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private PhotonView bulletView;

    private void Start()
    {
        bulletView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.name == "Platform")
        {
            PhotonNetwork.Destroy(bulletView);
        }
    }
}
