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

    [SerializeField] private Text gameInfo;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState.ToString() != "Joined")
        {
            gameInfo.text = PhotonNetwork.NetworkClientState.ToString();
        }
        else
        {
            gameInfo.text = "Player(s) online: " + PhotonNetwork.CurrentRoom.PlayerCount + " / MasterClient: " + PhotonNetwork.IsMasterClient;
        }
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
    }

    void SpawnMyPlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
        MyPlayer.AddComponent<Rigidbody>();
        MyPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyPlayer.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (PhotonNetwork.IsMasterClient)
        {
            MyPlayer.transform.Find("Gun").GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            MyPlayer.transform.Find("Gun").GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Joined Failed: " + returnCode + "Message: " + message);
    }
}
