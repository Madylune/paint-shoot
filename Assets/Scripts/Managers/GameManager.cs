using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    public bool IsGameEnd { get => isGameEnd; set => isGameEnd = value; }

    public Transform[] spawnPoints;

    public float blueScore = 0, redScore = 0, greenScore = 0, yellowScore = 0;

    private float maxScore = 100;

    private bool isGameEnd = false;

    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private Text winnerText;
    [SerializeField] private PlayerList playerList;
    [SerializeField] private Text teamInfoText;

    private void Start()
    {
        SpawnMyPlayer();
    }

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

    void SpawnMyPlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + "Player", spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0);
        MyPlayer.AddComponent<Rigidbody>();
        MyPlayer.GetComponent<Rigidbody>().mass = 20;
        MyPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyPlayer.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
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
        IsGameEnd = true;
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
