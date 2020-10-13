using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private GameObject loadingScreen, connectScreen;

    public void StartGame()
    {
        loadingScreen.SetActive(true);
        connectScreen.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() //Master Server
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        loadingScreen.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Joined Failed: " + returnCode + "Message: " + message);
    }

    public void BackToMainMenu()
    {
        PhotonNetwork.LeaveRoom(true);
        connectScreen.SetActive(true);
    }
}
