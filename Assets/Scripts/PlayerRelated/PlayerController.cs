using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _playerCustomProps = new ExitGames.Client.Photon.Hashtable();

    [SerializeField]
    private float moveSpeed = 5f, rotateSpeed = 150f, jumpForce = 4f, jumpRaycastDistance = 1.1f;

    [SerializeField]
    private Camera mainCam;

    [SerializeField] private float shootRange = 100f, shootForce = 20f;

    [SerializeField] private GameObject bulletPrefab, crossHair;

    private PhotonView playerView;
    private Rigidbody rb;
    private Transform bulletSpawn;

    [SerializeField]
    private float score;

    public float MyScore { get => score; set => score = value; }

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        rb = gameObject.GetComponent<Rigidbody>();
        bulletSpawn = transform.Find("BulletSpawn").transform;

        if (!playerView.IsMine)
        {
            mainCam.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (playerView.IsMine)
        {
            Move();
        }
    }

    void Update()
    {
        if (playerView.IsMine)
        {
            Jump();
            Shoot();

            if (MyScore > 0)
            {
                playerView.RPC("RPC_UpdateScore", RpcTarget.AllBuffered, MyScore);
            }
        }
        else
        {
            return;
        }
    }

    private void Move()
    {
        var x = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance);
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, shootRange) && !GetComponent<PlayerHealth>().IsDead)
        {
            crossHair.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerView.RPC("RPC_InstantiateBullet", RpcTarget.All, bulletSpawn.position);
            }
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    [PunRPC]
    void RPC_InstantiateBullet(Vector3 _position)
    {
        GameObject bullet = Instantiate(bulletPrefab, _position, Quaternion.identity).gameObject;
        bullet.GetComponent<Rigidbody>().velocity = mainCam.transform.TransformDirection(Vector3.forward) * shootForce;
        bullet.GetComponent<BulletScript>().MyOwner = this;

        switch (playerView.Owner.NickName)
        {
            case "Blue":
                bullet.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case "Red":
                bullet.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case "Green":
                bullet.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case "Yellow":
                bullet.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    [PunRPC]
    void RPC_UpdateScore(float score)
    {
        _playerCustomProps["PlayerScore"] = score;
        playerView.Owner.CustomProperties = _playerCustomProps;
    }
}
