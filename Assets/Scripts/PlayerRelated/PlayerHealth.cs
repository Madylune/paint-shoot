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
        Invoke("Respawn", 2f);
    }

    private void Respawn()
    {
        player.position = GameManager.MyInstance.spawnPoint.position;
    }
}
