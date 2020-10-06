using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Transform player;
    private bool isDead;

    [SerializeField]
    private GameObject damageOverlay;

    [SerializeField]
    private Sprite[] overlays;

    public bool IsDead { get => isDead; set => isDead = value; }

    private void Start()
    {
        player = gameObject.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Die();
        }
    }

    public void Die()
    {
        IsDead = true;

        damageOverlay.SetActive(true);

        player.Rotate(90, 0, 0);
        Invoke("Respawn", 5f);
    }

    private void Respawn()
    {
        damageOverlay.SetActive(false);
        player.position = GameManager.MyInstance.spawnPoint.position;
        player.rotation = Quaternion.identity;

        IsDead = false;
    }
}
