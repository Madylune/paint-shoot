using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField]
    private GameObject playerListElementPrefab;

    private List<PlayerListElement> playerList = new List<PlayerListElement>();

    public List<PlayerListElement> MyPlayerList { get => playerList; set => playerList = value; }

    public Player MyPhotonPlayer { get; private set; }

    public void UpdatePlayerList(Player[] photonPlayers)
    {
        if (photonPlayers != null)
        {
            for (int i = 0; i < photonPlayers.Length; i++)
            {
                PlayerJoinedRoom(photonPlayers[i]);
            }
        }
    }

    public void PlayerJoinedRoom(Player photonPlayer)
    {
        if (photonPlayer == null)
            return;

        PlayerLeftRoom(photonPlayer);

        GameObject element = Instantiate(playerListElementPrefab);
        element.transform.SetParent(transform, false);

        PlayerListElement playerListElement = element.GetComponent<PlayerListElement>();
        playerListElement.SetPlayerInfo(photonPlayer);

        MyPlayerList.Add(playerListElement);

    }

    public void PlayerLeftRoom(Player photonPlayer)
    {
        int index = MyPlayerList.FindIndex(x => x.MyPhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(MyPlayerList[index].gameObject);
            MyPlayerList.RemoveAt(index);
        }
    }
}
