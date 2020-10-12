using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public Transform[] spawnPoints;

    public float blueScore = 0, redScore = 0, greenScore = 0, yellowScore = 0;

    private float maxScore = 100;

    [SerializeField] private GameObject winnerText;

    private void Update()
    {
        if (PhotonRoom.MyInstance.MyPhotonPlayers != null)
        {
            UpdatePlayersScores();
        }

        if (blueScore >= maxScore)
        {
            winnerText.SetActive(true);
            winnerText.GetComponent<Text>().text = "Blue Team wins !";
        }
        else if (redScore >= maxScore)
        {
            winnerText.SetActive(true);
            winnerText.GetComponent<Text>().text = "Red Team wins !";
        }
        else if (greenScore >= maxScore)
        {
            winnerText.SetActive(true);
            winnerText.GetComponent<Text>().text = "Green Team wins !";
        }
        else if (yellowScore >= maxScore)
        {
            winnerText.SetActive(true);
            winnerText.GetComponent<Text>().text = "Yellow Team wins !";
        }
    }

    private void UpdatePlayersScores()
    {
        foreach (Player photonPlayer in PhotonRoom.MyInstance.MyPhotonPlayers)
        {
            if (photonPlayer.NickName == "Blue" && photonPlayer.CustomProperties.ContainsKey("PlayerScore"))
            {
                blueScore = (float)photonPlayer.CustomProperties["PlayerScore"];
            }
            else if (photonPlayer.NickName == "Red" && photonPlayer.CustomProperties.ContainsKey("PlayerScore"))
            {
                redScore = (float)photonPlayer.CustomProperties["PlayerScore"];
            }
            else if (photonPlayer.NickName == "Green" && photonPlayer.CustomProperties.ContainsKey("PlayerScore"))
            {
                greenScore = (float)photonPlayer.CustomProperties["PlayerScore"];
            }
            else if (photonPlayer.NickName == "Yellow" && photonPlayer.CustomProperties.ContainsKey("PlayerScore"))
            {
                yellowScore = (float)photonPlayer.CustomProperties["PlayerScore"];
            }
        }
    }
}
