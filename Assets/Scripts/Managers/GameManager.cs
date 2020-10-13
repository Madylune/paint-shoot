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

    private float maxScore = 10;

    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private Text winnerText;
    [SerializeField] private PlayerList playerList;
    [SerializeField] private Text teamInfoText;

    private void Update()
    {
        if (PhotonRoom.MyInstance.MyPhotonPlayers != null)
        {
            UpdatePlayersScores();
        }

        if (blueScore >= maxScore)
        {
            StartCoroutine(EndGame("Blue"));
        }
        else if (redScore >= maxScore)
        {
            StartCoroutine(EndGame("Red"));
        }
        else if (greenScore >= maxScore)
        {
            StartCoroutine(EndGame("Green"));
        }
        else if (yellowScore >= maxScore)
        {
            StartCoroutine(EndGame("Yellow"));
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

    private IEnumerator EndGame(string teamName)
    {
        yield return new WaitForSeconds(2f);

        gameOverGO.SetActive(true);
        winnerText.GetComponent<Text>().text = teamName + " Team wins !";
        Time.timeScale = 0f; // Freeze time and game
    }

    public void DisplayPlayerTeam(string teamColor)
    {
        teamInfoText.text = teamColor.ToUpper() + " TEAM";

        switch (teamColor)
        {
            case "Blue":
                teamInfoText.color = Color.blue;
                break;
            case "Red":
                teamInfoText.color = Color.red;
                break;
            case "Green":
                teamInfoText.color = Color.green;
                break;
            case "Yellow":
                teamInfoText.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    public void AddPlayerOnPlayerList(Player[] players)
    {
        playerList.UpdatePlayerList(players);
    }

    public void RemovePlayerOnPlayerList(Player player)
    {
        playerList.PlayerLeftRoom(player);
    }
}
