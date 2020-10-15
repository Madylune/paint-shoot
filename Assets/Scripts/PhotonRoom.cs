using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom MyInstance;
    private PhotonView roomView;

    [SerializeField] private Player[] photonPlayers;

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

    public override void OnJoinedRoom() //Call on my side
    {
        base.OnJoinedRoom();

        JoinTeam(PhotonNetwork.LocalPlayer);

        MyPhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = MyPhotonPlayers.Length;

        if (GameManager.MyInstance != null)
        {
            GameManager.MyInstance.DisplayPlayerTeam(PhotonNetwork.LocalPlayer.NickName);
            GameManager.MyInstance.AddPlayerOnPlayerList(MyPhotonPlayers);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //Call on other clients side
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
