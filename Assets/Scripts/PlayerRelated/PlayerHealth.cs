using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = gameObject.transform;
    }

    public void Die()
    {
        transform.Rotate(0, 90, 0);
        Invoke("Respawn", 2f);
    }

    private void Respawn()
    {
        player.position = GameManager.MyInstance.spawnPoint.position;
        transform.Rotate(0, 0, 0);
    }
}
