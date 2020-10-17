using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
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

    private bool isGameEnd = false;
    private float maxScore = 30;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private Text winnerText;
    [SerializeField] private PlayerList playerList;
    [SerializeField] private Text teamInfoText;
    [SerializeField] private GameObject optionsMenu;

    public Transform[] spawnPoints;
    public float blueScore = 0, redScore = 0, greenScore = 0, yellowScore = 0;

    private void Start()
    {
        InstantiatePlayer();

        DisplayPlayerTeam(PhotonNetwork.LocalPlayer.NickName);
        AddPlayerOnPlayerList(PhotonRoom.MyInstance.MyPhotonPlayers);
    }

    private void Update()
    {
        HandleInputs();

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

    void InstantiatePlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0);
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

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
            }
            else
            {
                optionsMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameOverGO.SetActive(true);
        }
    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
