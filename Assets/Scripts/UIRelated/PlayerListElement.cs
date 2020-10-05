using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListElement : MonoBehaviour
{
    [SerializeField] private Text playerScore;

    [SerializeField] private Image playerImage;

    private Player player;
    public Player MyPlayer { get => player; set => player = value; }

    private void Update()
    {

    }

    public void SetPlayerInfo(Player _player)
    {
        MyPlayer = _player;
        playerScore.text = "0 pts";

        string teamColor = (string)_player.CustomProperties["TeamColor"];

        switch (teamColor)
        {
            case "Blue":
                playerImage.color = Color.blue;
                break;
            case "Red":
                playerImage.color = Color.red;
                break;
            case "Green":
                playerImage.color = Color.green;
                break;
            case "Yellow":
                playerImage.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    //[PunRPC]
    //void UpdateScore()
    //{
    //    float _score = (float)player.CustomProperties["PlayerScore"];
    //    playerScore.text = _score.ToString() + " pts";
    //    MyPlayer. RPC("DestroyEnemy", RpcTarget.AllBuffered, viewID);
    //}

    //[PunRPC]
    //void DestroyEnemy(int viewID)
    //{
    //    GameObject go = PhotonView.Find(viewID).gameObject;
    //    Destroy(go);
    //    PhotonNetwork.Destroy(go);
    //    _photonView.RPC("DestroyEnemy", RpcTarget.AllBuffered, viewID);
    //}
}
