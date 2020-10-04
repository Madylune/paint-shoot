using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f, rotateSpeed = 150f, jumpForce = 4f, jumpRaycastDistance = 1.1f;

    [SerializeField]
    private Camera mainCam;

    private PhotonView playerView;
    private Rigidbody rb;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        rb = gameObject.GetComponent<Rigidbody>();

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
}
