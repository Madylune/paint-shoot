using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private PhotonView bulletView;
    private MeshRenderer meshRenderer;

    [SerializeField] private ParticleSystem[] bulletEffects;

    private void Start()
    {
        bulletView = GetComponent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.name == "Platform")
        {
            GameObject particle;
            if (meshRenderer.material.color == Color.blue)
            {
                particle = Instantiate(bulletEffects[0], transform.position, Quaternion.identity).gameObject;
            }
            else
            {
                particle = Instantiate(bulletEffects[1], transform.position, Quaternion.identity).gameObject;
            }


            Destroy(particle, 2f);

            PhotonNetwork.Destroy(bulletView);
        }
    }


}
