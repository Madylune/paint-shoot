using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager instance;

    public static NetworkManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NetworkManager>();
            }
            return instance;
        }
    }

    [SerializeField] private Text teamInfoText;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject canvas, loadingScreen;

    private ExitGames.Client.Photon.Hashtable _playerCustomProps = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() //Master Server
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        SpawnMyPlayer();

        string teamColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["TeamColor"];
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

        loadingScreen.SetActive(false);
        canvas.SetActive(true);
    }

    void SpawnMyPlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
        MyPlayer.AddComponent<Rigidbody>();
        MyPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyPlayer.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        int teamIndex = MyPlayer.GetComponent<PhotonView>().OwnerActorNr;
        JoinTeam(teamIndex);
    }

    public void JoinTeam(int _team)
    {
        string color = string.Empty;
        switch (_team)
        {
            case 1:
                color = "Blue";
                break;
            case 2:
                color = "Red";
                break;
            case 3:
                color = "Green";
                break;
            case 4:
                color = "Yellow";
                break;
            default:
                break;
        }

        // Already have team
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("TeamColor"))
        {
            //Switch team
            PhotonNetwork.LocalPlayer.CustomProperties["TeamColor"] = color;
        }
        else
        {
            _playerCustomProps["TeamColor"] = color;
            PhotonNetwork.LocalPlayer.CustomProperties = _playerCustomProps;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Joined Failed: " + returnCode + "Message: " + message);
    }
}
