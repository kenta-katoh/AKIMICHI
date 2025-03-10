using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingTest : MonoBehaviour
{
    [SerializeField]
    private List<MatchingPlayerContents> playerList = new List<MatchingPlayerContents>();

    private void Awake()
    {
        NetworkManager.Instance().SetCallbackOnLeaveRoom(() => 
        {
            TestSceneManager.Instance().ChangeScene("LobbyScene");
        });

        NetworkManager.Instance().SetCallbackOnPlayerEnteredRoom(OnPlayerEnteredRoom);
        NetworkManager.Instance().SetCallbackOnPlayerLeftRoom(OnPlayerLeftRoom);
    }

    private void Start()
    {
        UpdateRoomData();
    }

    /// <summary>
    /// ルーム退出
    /// </summary>
    public void LeaveRoom()
    {
        NetworkManager.Instance().LeaveRoom();
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomData();
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomData();
    }

    private void UpdateRoomData()
    {
        foreach (var player in playerList)
        {
            player.ClearData();
        }

        int index = 0;
        foreach(var item in PhotonNetwork.CurrentRoom.Players)
        {
            if(index < this.playerList.Count)
            {
                this.playerList[index].SetPlayerData(item.Value.NickName);
                index++;
            }
        }
    }
}
