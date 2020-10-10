using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Gun : MonoBehaviourPunCallbacks
{
    private PhotonView playerView;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        playerView = transform.parent.gameObject.GetComponent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();

        //Set gun's color
        switch (playerView.Owner.NickName)
        {
            case "Blue":
                meshRenderer.material.color = Color.blue;
                break;
            case "Red":
                meshRenderer.material.color = Color.red;
                break;
            case "Green":
                meshRenderer.material.color = Color.green;
                break;
            case "Yellow":
                meshRenderer.material.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
