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

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        playButton.interactable = false;
    }

    public override void OnConnectedToMaster() //Master Server
    {
        Debug.Log("Connected to Photon");
        playButton.interactable = true;
    }

    public void OnClickPlayGame()
    {
        JoinRoom();
    }

    public void JoinRoom()
    {
        loadingScreen.SetActive(true);
        connectScreen.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("Room Joined Failed: " + returnCode + "Message: " + message);
        CreateRoom();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        loadingScreen.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
