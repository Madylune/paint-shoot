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
        player.Rotate(90, 0, 0);
        Invoke("Respawn", 5f);
    }

    private void Respawn()
    {
        player.position = GameManager.MyInstance.spawnPoint.position;
        player.rotation = Quaternion.identity;
    }
}
