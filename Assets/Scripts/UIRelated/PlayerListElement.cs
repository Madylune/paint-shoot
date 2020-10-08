using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListElement : MonoBehaviour
{
    [SerializeField] private Text playerScore, playerStatus;

    [SerializeField] private Image playerImage;

    private Player player;
    public Player MyPhotonPlayer { get => player; set => player = value; }

    private void Update()
    {
        if (MyPhotonPlayer.CustomProperties.ContainsKey("PlayerScore"))
        {
            float _score = (float)MyPhotonPlayer.CustomProperties["PlayerScore"];
            playerScore.text = _score.ToString() + " pts";
        }
    }

    public void SetPlayerInfo(Player _player)
    {
        MyPhotonPlayer = _player;
        playerScore.text = "0 pts";

        if (PhotonNetwork.LocalPlayer == _player)
        {
            playerStatus.text = "(me)";
        }

        switch (_player.NickName)
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
}
