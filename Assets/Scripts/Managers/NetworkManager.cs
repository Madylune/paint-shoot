using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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

    [SerializeField] private GameObject loadingScreen, connectScreen;

    [SerializeField] private Button playButton;

    private bool isConnecting = false;

    private const string GameVersion = "0.1";

    private const int MaxPlayersPerRoom = 2;

    public void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster() //Master Server
    {
        if (isConnecting)
        {
            //Debug.Log("Debug: Connected to Master");
            JoinRoom();
        }
    }

    public void SearchPlayers()
    {
        isConnecting = true;

        loadingScreen.SetActive(true);
        connectScreen.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            JoinRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinRoom()
    {
        //Debug.Log("Debug: Try to join room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        loadingScreen.SetActive(false);
        connectScreen.SetActive(true);

        Debug.Log($"Debug Disconnected due to: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("Room Joined Failed: " + returnCode + "Message: " + message);
        CreateRoom();
    }

    public void CreateRoom()
    {
        //Debug.Log("Debug: Create a new room");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = MaxPlayersPerRoom };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            //Debug.Log("Debug: All players are connected");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            LoadGame();
        }
    }

    public void LoadGame()
    {
        //Debug.Log("Debug: Load game");
        PhotonNetwork.LoadLevel(1);
    }
}
