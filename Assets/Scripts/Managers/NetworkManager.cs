using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() //Master Server
    {
        Debug.Log("Connected to Photon");
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
        //SceneManager.LoadScene("Game");
    }
}
