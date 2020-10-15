using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float lookSensitivity, smoothing;

    [SerializeField]
    private GameObject player;

    private Vector2 smoothVelocity, currentLookingPosition;

    private void Start()
    {
        player = transform.parent.gameObject;
    }

    private void Update()
    {
        if (player.GetComponent<PhotonView>().IsMine)
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        inputValues = Vector2.Scale(inputValues, new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));

        smoothVelocity.x = Mathf.Lerp(smoothVelocity.x, inputValues.x, 1f / smoothing);
        smoothVelocity.y = Mathf.Lerp(smoothVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPosition += smoothVelocity;
    }
}
