using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom MyInstance;
    private PhotonView roomView;

    [SerializeField] private GameObject playerPrefab;

    private Player[] photonPlayers;
    public int playersInRoom;

    public Player[] MyPhotonPlayers { get => photonPlayers; set => photonPlayers = value; }

    private ExitGames.Client.Photon.Hashtable _playerCustomProps = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        if (PhotonRoom.MyInstance == null)
        {
            PhotonRoom.MyInstance = this;
        }
        else
        {
            if (PhotonRoom.MyInstance != this)
            {
                Destroy(PhotonRoom.MyInstance.gameObject);
                PhotonRoom.MyInstance = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Start()
    {
        roomView = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        JoinTeam(PhotonNetwork.LocalPlayer);

        GameManager.MyInstance.DisplayPlayerTeam(PhotonNetwork.LocalPlayer.NickName);

        MyPhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = MyPhotonPlayers.Length;

        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.AddPlayerOnPlayerList(MyPhotonPlayers);
        }

        SpawnMyPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        JoinTeam(newPlayer);

        MyPhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.AddPlayerOnPlayerList(MyPhotonPlayers);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        MyPhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom--;

        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.RemovePlayerOnPlayerList(otherPlayer);
        }
    }

    void SpawnMyPlayer()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, GameManager.MyInstance.spawnPoints[Random.Range(0, GameManager.MyInstance.spawnPoints.Length)].position, Quaternion.identity, 0);
        MyPlayer.AddComponent<Rigidbody>();
        MyPlayer.GetComponent<Rigidbody>().mass = 20;
        MyPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyPlayer.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void JoinTeam(Player _player)
    {
        string color = string.Empty;
        switch (_player.ActorNumber)
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
        if (_player.CustomProperties.ContainsKey("TeamColor"))
        {
            //Switch team
            _player.CustomProperties["TeamColor"] = color;
            PhotonNetwork.LocalPlayer.NickName = color;
        }
        else
        {
            _playerCustomProps["TeamColor"] = color;
            _player.CustomProperties = _playerCustomProps;
            _player.NickName = color;
        }
    }
}
