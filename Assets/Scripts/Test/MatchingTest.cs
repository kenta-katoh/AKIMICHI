using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingTest : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.Instance().SetCallbackOnLeaveRoom(() => 
        {
            TestSceneManager.Instance().ChangeScene("LobbyScene");
        });

        NetworkManager.Instance().SetCallbackOnPlayerEnteredRoom(OnPlayerEnteredRoom);
        NetworkManager.Instance().SetCallbackOnPlayerLeftRoom(OnPlayerLeftRoom);
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
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
    }
}
