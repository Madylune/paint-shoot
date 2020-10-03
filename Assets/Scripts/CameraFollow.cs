using System.Collections;
using System.Collections.Generic;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
        CheckForShooting();
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        inputValues = Vector2.Scale(inputValues, new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));

        smoothVelocity.x = Mathf.Lerp(smoothVelocity.x, inputValues.x, 1f / smoothing);
        smoothVelocity.y = Mathf.Lerp(smoothVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPosition += smoothVelocity;

        //transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
    }

    private void CheckForShooting()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, Mathf.Infinity))
            {
                Debug.Log("shoot: " + raycastHit.collider.name);
            }
        }
    }
}
