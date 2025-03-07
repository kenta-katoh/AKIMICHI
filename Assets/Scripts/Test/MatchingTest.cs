using Photon.Pun;
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
    }

    /// <summary>
    /// ルーム退出
    /// </summary>
    public void LeaveRoom()
    {
        NetworkManager.Instance().LeaveRoom();
    }
}
