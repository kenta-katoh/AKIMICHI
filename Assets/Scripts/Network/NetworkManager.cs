using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Akimichi.Game;
using ExitGames.Client.Photon;
using Akimichi;
using static Akimichi.Game.GameConst;

public class NetworkManager : ManagerBase<NetworkManager>
{
    public Action onConnectedToMaster { get; private set; } = null;   // サーバー接続時
    public Action onDisconnected { get; private set; } = null;        // サーバー接続解除
    public Action onJoinedLobby { get; private set; } = null;         // ロビー接続時
    public Action onLeaveLobby { get; private set; } = null;          // ロビー接続解除
    public Action onCreateRoom { get; private set; } = null;          // ルーム作成時
    public Action onCreateRoomFailed { get; private set; } = null;    // ルーム作成失敗
    public Action onJoinedRoom { get; private set; } = null;          // ルーム入室時
    public Action onJoinedRoomFailed { get; private set; } = null;    // ルーム入室失敗
    public Action onLeaveRoom { get; private set; } = null;           // ルーム退出時
    public Action<List<RoomInfo>> onRoomListUpdate { get; private set; } = null; // ルーム情報更新時
    public Action<Player> onPlayerEnteredRoom { get; private set; } = null;   // ルームにプレイヤーIn時
    public Action<Player> onPlayerLeftRoom { get; private set; } = null;      // ルームからプレイヤーout時

    private RaiseEventOptions eventOptions = new RaiseEventOptions() 
    {
        Receivers = ReceiverGroup.All,
        CachingOption = EventCaching.AddToRoomCache
    };
    private SendOptions sendOptions = new SendOptions() 
    {
        Reliability = true
    };

    public override void Dispose()
    {
        base.Dispose();
        DeleteCallBack();
    }

    /// <summary>
    /// 各種コールバックの削除
    /// </summary>
    public void DeleteCallBack()
    {
        Debug.Log("delete callback");
        this.onConnectedToMaster = null;
        this.onDisconnected = null;
        this.onJoinedLobby = null;
        this.onLeaveLobby = null;
        this.onCreateRoom = null;
        this.onCreateRoomFailed = null;
        this.onJoinedRoom = null;
        this.onJoinedRoomFailed = null;
        this.onLeaveRoom = null;
        this.onRoomListUpdate = null;
        this.onPlayerEnteredRoom = null;
        this.onPlayerLeftRoom = null;
    }

    /// <summary>
    /// サーバー接続
    /// </summary>
    public void Connect()
    {
        // PhotonServerSettingsの設定内容を使って、マスターサーバーへ接続する
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// サーバー接続解除
    /// </summary>
    public void Disconnect()
    {
        // Photonのサーバーから切断する
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    /// <summary>
    /// ロビー接続
    /// </summary>
    /// <param name="lobbyName"></param>
    public void JoinLobby(string lobbyName)
    {
        if (!PhotonNetwork.InLobby)
        {
            TypedLobby customLobby = new TypedLobby(lobbyName, LobbyType.Default);
            PhotonNetwork.JoinLobby(customLobby);
        }
    }

    /// <summary>
    /// ロビー接続解除
    /// </summary>
    public void LeaveLobby()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    /// <param name="name"></param>
    public void CreateRoom(string name)
    {
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = Enum.GetNames(typeof(PlayerIndex)).Length;
        option.IsVisible = true;
        option.IsOpen = true;
        option.PlayerTtl = 0;
        option.EmptyRoomTtl = 0;
        option.PublishUserId = true;

        PhotonNetwork.CreateRoom(name, option);
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    /// <param name="name"></param>
    /// <param name="op"></param>
    public void CreateRoom(string name, RoomOptions op)
    {
        PhotonNetwork.CreateRoom(name, op);
    }

    /// <summary>
    /// ルーム参加
    /// </summary>
    /// <param name="name"></param>
    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    /// <summary>
    /// ルーム退室
    /// </summary>
    public void LeaveRoom()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// 該当のIDが自分自身か
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool IsMyself(string userId)
    {
        return PhotonNetwork.LocalPlayer.UserId == userId;
    }

    /// <summary>
    /// シーン同期設定
    /// </summary>
    /// <param name="flag"></param>
    public void SetSysncScene(bool flag)
    {
        PhotonNetwork.AutomaticallySyncScene = flag;
    }

    /// <summary>
    /// 同期遷移
    /// </summary>
    /// <param name="sceneName"></param>
    public void SysncLoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    /// <summary>
    /// ホストかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsMasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }

    /// <summary>
    /// プレイヤー取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameConst.PlayerIndex GetPlayerIndex(string userID)
    {
        GameConst.PlayerIndex result = GameConst.PlayerIndex.First;
        int index = 0;
        foreach(Player item in PhotonNetwork.PlayerList)
        {
            if(item.UserId == userID)
            {
                result = (GameConst.PlayerIndex)index;
                break;
            }
            index++;
        }
        return result;
    }

    /// <summary>
    /// 名前取得
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public string GetName(string userID)
    {
        string result = "";
        foreach (Player item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == userID)
            {
                result = item.NickName;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// ユーザーID取得
    /// </summary>
    /// <returns></returns>
    public string GetUserID()
    {
        return PhotonNetwork.LocalPlayer.UserId;
    }

    /// <summary>
    /// 名前設定
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public bool IsAllocateViewID(PhotonView view)
    {
        return PhotonNetwork.AllocateViewID(view);
    }

    /// <summary>
    /// イベント送信
    /// </summary>
    public void SendEvent(EventConst.Event _event, SendObjectData data)
    {
        if(_event == EventConst.Event.None) return;
        PhotonNetwork.RaiseEvent(EventConst.ConvertEvent(_event), data.Datas, eventOptions, sendOptions);
        data.IsUse = false;
    }

    /// <summary>
    /// ルーム人数取得
    /// </summary>
    /// <returns></returns>
    public int GetRoomPlayerValue()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }

    /// <summary>
    /// サーバー時間の設定
    /// </summary>
    public void SetServerTime()
    {
        if(IsMasterClient())
        {
            var properties = new Hashtable();
            properties.Add("ServerTime", PhotonNetwork.Time);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    /// <summary>
    /// サーバー時間取得
    /// </summary>
    /// <returns></returns>
    public double GetServerTime()
    {
        return (double)PhotonNetwork.CurrentRoom.CustomProperties["ServerTime"];
    }

    /// <summary>
    /// photon内時間取得
    /// </summary>
    /// <returns></returns>
    public double GetPhotonTime()
    {
        return PhotonNetwork.Time;
    }

    /// <summary>
    /// デバッグ設定
    /// </summary>
    /// <param name="flag"></param>
    public void SetDebugRoom(bool flag)
    {
        if (IsMasterClient())
        {
            var properties = new Hashtable();
            properties.Add("DebugRoom", (double)Convert.ToInt32(flag));
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    private bool IsDebugRoom()
    {
        return Convert.ToBoolean(PhotonNetwork.CurrentRoom.CustomProperties["DebugRoom"]);
    }

    /// <summary>
    /// 少人数スタート設定
    /// </summary>
    /// <param name="flag"></param>
    public void SetDebugRoomSetting(bool flag, int time)
    {
        if (IsMasterClient())
        {
            var properties = new Hashtable();
            properties.Add("StartPlayer", (double)Convert.ToInt32(flag));
            properties.Add("GameTime", (double)time);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    /// <summary>
    /// 少人数スタート取得
    /// </summary>
    /// <returns></returns>
    public bool GetStartPlayer()
    {
        bool result = false;
        if(IsDebugRoom())
        {
            result = Convert.ToBoolean(PhotonNetwork.CurrentRoom.CustomProperties["StartPlayer"]);
        }
        return result;
    }

    /// <summary>
    /// 制限時間取得
    /// </summary>
    /// <returns></returns>
    public int GetGamTime()
    {
        int result = GameConst.GameTime;
        if (IsDebugRoom())
        {
            result = Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["GameTime"]);
        }
        return result;
    }

    /// <summary>
    /// 準備完了
    /// </summary>
    /// <param name="userID"></param>
    public void SetRoomReady(string userID)
    {
        string players = (string)PhotonNetwork.CurrentRoom.CustomProperties["ReadyPlayer"];
        players += (userID + ",");

        var properties = new Hashtable();
        properties.Add("ReadyPlayer", players);
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userID"></param>
    public void RemoveRoomReady(string userID)
    {
        string result = "";
        string players = (string)PhotonNetwork.CurrentRoom.CustomProperties["ReadyPlayer"];
        if (string.IsNullOrEmpty(players)) return;

        string[] arr = players.Split(',');
        for (int i = 0; i < arr.Length; ++i)
        {
            if (!string.IsNullOrEmpty(arr[i]))
            {
                if (arr[i] != userID)
                {
                    result += (arr[i] + ",");
                }
            }
        }
        var properties = new Hashtable();
        properties.Add("ReadyPlayer", players);
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

    /// <summary>
    /// 準備完了プレイヤー取得
    /// </summary>
    /// <returns></returns>
    public List<string> GetReadyPlayer()
    {
        List<string> result = new List<string>();

        string players = (string)PhotonNetwork.CurrentRoom.CustomProperties["ReadyPlayer"];
        if (string.IsNullOrEmpty(players)) return result;

        string[] arr = players.Split(',');
        for (int i = 0; i < arr.Length; ++i)
        {
            if (!string.IsNullOrEmpty(arr[i]))
            {
                if (!result.Contains(arr[i]))
                {
                    result.Add(arr[i]);
                }
            }
        }
        return result;
    }

    ///////////////////////////////////////////////////////////
    /// コールバック
    ///////////////////////////////////////////////////////////
    public void SetCallbackOnConnect(Action act) { this.onConnectedToMaster =  act; }
    public void SetCallbackOnDisconnected(Action act) { this.onDisconnected = act; }
    public void SetCallbackOnJoinedLobby(Action act) { this.onJoinedLobby = act; }
    public void SetCallbackOnLeaveLobby(Action act) { this.onLeaveLobby = act; }
    public void SetCallbackOnCreatedRoom(Action act) { this.onCreateRoom = act; }
    public void SetCallbackOnCreateRoomFailed(Action act) { this.onCreateRoomFailed = act; }
    public void SetCallbackOnJoinedRoom(Action act) { this.onJoinedRoom = act; }
    public void SetCallbackOnJoinRoomFailed(Action act) { this.onJoinedRoomFailed = act; }
    public void SetCallbackOnLeaveRoom(Action act) { this.onLeaveRoom = act; }
    public void SetCallbackOnRoomListUpdate(Action<List<RoomInfo>> act) { this.onRoomListUpdate = act; }
    public void SetCallbackOnPlayerEnteredRoom(Action<Player> act) { this.onPlayerEnteredRoom = act; }
    public void SetCallbackOnPlayerLeftRoom(Action<Player> act) { this.onPlayerLeftRoom = act; }
}
