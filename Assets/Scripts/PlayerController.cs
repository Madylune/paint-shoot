using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f, rotateSpeed = 150f, jumpForce = 4f, jumpRaycastDistance = 1.1f;

    private PhotonView photonView;
    private Rigidbody rb;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Jump();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime);
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
