using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerListElement playerListElement;

    private List<PlayerListElement> playerList = new List<PlayerListElement>();

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    private void GetCurrentRoomPlayers()
    {
        Dictionary<int, Player> pList = PhotonNetwork.CurrentRoom.Players;

        foreach (KeyValuePair<int, Player> p in pList)
        {
            AddPlayerIntoList(p.Value);
        }
    }

    private void AddPlayerIntoList(Player player)
    {
        PlayerListElement element = Instantiate(playerListElement, transform);

        if (element != null)
        {
            element.SetPlayerInfo(player);
            playerList.Add(element);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerList.FindIndex(x => x.MyPlayer == otherPlayer);
        if (index != -1)
        {
            Destroy(playerList[index].gameObject);
            playerList.RemoveAt(index);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerIntoList(newPlayer);
    }
}
