using Akimichi;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingScene : MonoBehaviour
{
    [SerializeField]
    private List<MatchingPlayerContents> playerList = new List<MatchingPlayerContents>();

    [SerializeField]
    private TMP_InputField inputField = null;

    private void Awake()
    {
        NetworkManager.Instance().DeleteCallBack();
        NetworkManager.Instance().SetCallbackOnLeaveRoom(() => 
        {
            SceneManager.LoadScene(SceneConst.Lobby);
        });

        NetworkManager.Instance().SetCallbackOnPlayerEnteredRoom(OnPlayerEnteredRoom);
        NetworkManager.Instance().SetCallbackOnPlayerLeftRoom(OnPlayerLeftRoom);
    }

    private void Start()
    {
        NetworkManager.Instance().SetSysncScene(true);
        UpdateRoomData();
    }

    /// <summary>
    /// ルーム退出
    /// </summary>
    public void LeaveRoom()
    {
        NetworkManager.Instance().LeaveRoom();
    }

    /// <summary>
    /// 同期遷移
    /// </summary>
    public void LoadScene()
    {
        if(NetworkManager.Instance().IsMasterClient())
        {
            NetworkManager.Instance().SysncLoadScene(SceneConst.Game);
        }
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
        var dic = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList();
        foreach (var item in dic)
        {
            if(index < this.playerList.Count)
            {
                this.playerList[index].SetPlayerData(index + 1, item.Value);
                index++;
            }
        }
    }

    public void ChangeName(string name)
    {
        NetworkManager.Instance().SetName(inputField.text);
    }
}
