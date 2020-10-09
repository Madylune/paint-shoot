using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PhotonView playerView;
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
        playerView = GetComponent<PhotonView>();
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

        if (playerView.IsMine)
        {
            damageOverlay.SetActive(true);
        }

        player.Rotate(90, 0, 0);
        Invoke("Respawn", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            switch (other.GetComponent<BulletScript>().bulletColor)
            {
                case "Blue":
                    damageOverlay.GetComponent<Image>().sprite = overlays[0];
                    break;
                case "Red":
                    damageOverlay.GetComponent<Image>().sprite = overlays[1];
                    break;
                case "Green":
                    damageOverlay.GetComponent<Image>().sprite = overlays[2];
                    break;
                case "Yellow":
                    damageOverlay.GetComponent<Image>().sprite = overlays[3];
                    break;
                default:
                    break;
            }
        }
    }

    public void Respawn()
    {
        damageOverlay.SetActive(false);
        player.position = GameManager.MyInstance.spawnPoint.position;
        player.rotation = Quaternion.identity;

        IsDead = false;
    }
}
