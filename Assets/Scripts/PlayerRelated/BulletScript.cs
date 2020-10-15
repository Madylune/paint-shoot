using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private PlayerController owner;
    private float points = 10f;

    [SerializeField]
    private float lifeTime;

    [SerializeField] private ParticleSystem[] bulletEffects;

    public PlayerController MyOwner { get => owner; set => owner = value; }
    public string bulletColor;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bulletColor = MyOwner.GetComponent<PhotonView>().Owner.NickName;

        Destroy(gameObject, lifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.name == "Platform" || other.transform.tag == "Barrel")
        {
            //Set splash effects color
            GameObject particle;

            if (meshRenderer.material.color == Color.blue)
            {
                particle = Instantiate(bulletEffects[0], transform.position, Quaternion.identity).gameObject;
            }
            else if (meshRenderer.material.color == Color.red)
            {
                particle = Instantiate(bulletEffects[1], transform.position, Quaternion.identity).gameObject;
            }
            else if (meshRenderer.material.color == Color.green)
            {
                particle = Instantiate(bulletEffects[2], transform.position, Quaternion.identity).gameObject;
            }
            else
            {
                particle = Instantiate(bulletEffects[3], transform.position, Quaternion.identity).gameObject;
            }

            Destroy(particle, 2f);

            Destroy(gameObject);

            if (other.transform.tag == "Player" && !other.gameObject.GetComponent<PlayerController>().IsInSafeZone && !other.gameObject.GetComponent<PlayerHealth>().IsDead && !GameManager.MyInstance.IsGameEnd)
            {
                other.gameObject.GetComponent<PlayerHealth>().Die();

                MyOwner.MyScore += points;
            }
        }
    }

}
