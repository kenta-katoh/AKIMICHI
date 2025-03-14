using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager instance = null;
    private static Action onConnectedToMaster = null;   // サーバー接続時
    private static Action onDisconnected = null;        // サーバー接続解除
    private static Action onJoinedLobby = null;         // ロビー接続時
    private static Action onLeaveLobby = null;          // ロビー接続解除
    private static Action onCreateRoom = null;          // ルーム作成時
    private static Action onCreateRoomFailed = null;    // ルーム作成失敗
    private static Action onJoinedRoom = null;          // ルーム入室時
    private static Action onJoinedRoomFailed = null;    // ルーム入室失敗
    private static Action onLeaveRoom = null;           // ルーム退出時
    private static Action<List<RoomInfo>> onRoomListUpdate = null; // ルーム情報更新時
    private static Action<Player> onPlayerEnteredRoom = null;   // ルームにプレイヤーIn時
    private static Action<Player> onPlayerLeftRoom = null;      // ルームからプレイヤーout時

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.NickName = "akimichi" + UnityEngine.Random.Range(1000, 9999); ;
    }

    public static NetworkManager Instance()
    {
        if(instance == null) instance = new NetworkManager();
        return instance;
    }

    private void OnDestroy()
    {
        DeleteCallBack();
    }

    /// <summary>
    /// 各種コールバックの削除
    /// </summary>
    public void DeleteCallBack()
    {
        Debug.Log("delete callback");
        onConnectedToMaster = null;
        onDisconnected = null;
        onJoinedLobby = null;
        onLeaveLobby = null;
        onCreateRoom = null;
        onCreateRoomFailed = null;
        onJoinedRoom = null;
        onJoinedRoomFailed = null;
        onLeaveRoom = null;
        onRoomListUpdate = null;
        onPlayerEnteredRoom = null;
        onPlayerLeftRoom = null;
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
        option.MaxPlayers = 4;
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
        PhotonNetwork.LeaveRoom();
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

    public void CreateObject(string pass)
    {
        PhotonNetwork.InstantiateRoomObject(pass, Vector3.zero, Quaternion.identity);
    }

    ///////////////////////////////////////////////////////////
    /// コールバック
    ///////////////////////////////////////////////////////////
    public void SetCallbackOnConnect(Action act) { onConnectedToMaster =  act; }
    public void SetCallbackOnDisconnected(Action act) { onDisconnected = act; }
    public void SetCallbackOnJoinedLobby(Action act) { onJoinedLobby = act; }
    public void SetCallbackOnLeaveLobby(Action act) { onLeaveLobby = act; }
    public void SetCallbackOnCreatedRoom(Action act) { onCreateRoom = act; }
    public void SetCallbackOnCreateRoomFailed(Action act) { onCreateRoomFailed = act; }
    public void SetCallbackOnJoinedRoom(Action act) { onJoinedRoom = act; }
    public void SetCallbackOnJoinRoomFailed(Action act) { onJoinedRoomFailed = act; }
    public void SetCallbackOnLeaveRoom(Action act) { onLeaveRoom = act; }
    public void SetCallbackOnRoomListUpdate(Action<List<RoomInfo>> act) { onRoomListUpdate = act; }
    public void SetCallbackOnPlayerEnteredRoom(Action<Player> act) { onPlayerEnteredRoom = act; }
    public void SetCallbackOnPlayerLeftRoom(Action<Player> act) { onPlayerLeftRoom = act; }

    /// <summary>
    /// マスターサーバーへの接続が成功した時に呼ばれるコールバック
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        Debug.Log(onConnectedToMaster);
        onConnectedToMaster?.Invoke();
    }

    /// <summary>
    /// Photonのサーバーから切断された時に呼ばれるコールバック
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
        onDisconnected?.Invoke();
    }

    /// <summary>
    /// ロビー接続時コールバック
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに参加しました");
        onJoinedLobby?.Invoke();
    }

    /// <summary>
    /// ロビー接続解除時コールバック
    /// </summary>
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("ロビーから退出しました");
        onLeaveLobby?.Invoke();
    }

    /// <summary>
    /// ルームの作成が成功した時に呼ばれるコールバック
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("ルームの作成に成功しました");
        onCreateRoom?.Invoke();
    }

    /// <summary>
    /// ルームの作成が失敗した時に呼ばれるコールバック
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームの作成に失敗しました: {message}");
        onCreateRoomFailed?.Invoke();
    }

    /// <summary>
    /// ルームへの参加が成功した時に呼ばれるコールバック
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへ参加しました");
        onJoinedRoom?.Invoke();
    }

    /// <summary>
    /// ルーム名を指定したルームへの参加が失敗した時に呼ばれるコールバック
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームへの参加に失敗しました: {message}");
        onJoinedRoomFailed?.Invoke();
    }

    /// <summary>
    /// ルームから退出した時に呼ばれるコールバック
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("ルームから退出しました");
        onLeaveRoom?.Invoke();
    }

    /// <summary>
    /// ロビー接続時にルーム情報に更新があった場合
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("ルーム情報が更新されました");
        onRoomListUpdate?.Invoke(roomList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("プレイヤーが入室しました");
        onPlayerEnteredRoom?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("プレイヤーが退室しました");
        onPlayerLeftRoom?.Invoke(otherPlayer);
    }
}
