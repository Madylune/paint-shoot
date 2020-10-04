using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float range = 100f, force;

    [SerializeField] private Camera cam;

    [SerializeField] private GameObject bulletPrefab, crossHair;

    [SerializeField] private Transform bulletSpawn;

    private PhotonView playerView;

    private void Start()
    {
        playerView = transform.parent.gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (playerView.IsMine)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            crossHair.SetActive(true);
            crossHair.transform.TransformVector(hit.point);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(InstantiateBullet());
            }
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    IEnumerator InstantiateBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Prefabs/" + bulletPrefab.name, bulletSpawn.position, Quaternion.identity, 0);
        bullet.GetComponent<Rigidbody>().velocity = cam.transform.TransformDirection(Vector3.forward) * force;

        yield return new WaitForSeconds(2f);

        PhotonNetwork.Destroy(bullet.GetComponent<PhotonView>());
    }
}
