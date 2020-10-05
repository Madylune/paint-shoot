using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField] private ParticleSystem[] bulletEffects;

    private void Start()
    {
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

            Destroy(gameObject);

            if (other.transform.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerHealth>().Die();
            }
        }
    }


}
