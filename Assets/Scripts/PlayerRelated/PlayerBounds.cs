using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [SerializeField]
    private float minY;

    private bool outOfBounds;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        Vector2 tmp = transform.position;

        if (tmp.y <= minY)
        {
            if (!outOfBounds)
            {
                outOfBounds = true;
                playerHealth.IsDead = true;
                playerHealth.Respawn();
            }
        }
        else
        {
            outOfBounds = false;
        }
    }
}
