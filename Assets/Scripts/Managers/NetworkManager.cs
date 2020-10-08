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

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject canvas, loadingScreen;

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
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        SpawnMyPlayer();

        loadingScreen.SetActive(false);
        canvas.SetActive(true);
    }

    void SpawnMyPlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
        MyPlayer.AddComponent<Rigidbody>();
        MyPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyPlayer.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Joined Failed: " + returnCode + "Message: " + message);
    }
}
