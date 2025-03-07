using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectTest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> roomInfo = new List<GameObject>();

    [SerializeField]
    private Transform listParent = null;

    [SerializeField]
    private Transform disableParent = null;

    private void Awake()
    {
        // PhotonServerSettingsの設定内容を使って、マスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();

        // Photonのサーバーから切断する
        //PhotonNetwork.Disconnect();
    }

    private void Start()
    {
        foreach (GameObject room in roomInfo)
        {
            room.transform.parent = disableParent;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (GameObject room in roomInfo)
        {
            room.transform.parent = disableParent;
        }

        int index = 0;
        foreach(RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                continue;
            }

            if (index < roomInfo.Count)
            {
                roomInfo[index].transform.parent = listParent;
            }
            index++;
        }
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    public void CreateRoom()
    {
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 4;
        option.IsVisible = true;
        option.IsOpen = true;
        PhotonNetwork.CreateRoom(null, option);
    }

    /// <summary>
    /// ルーム参加
    /// </summary>
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("hogehoge");
    }

    /// <summary>
    /// ルーム退出
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに参加しました");
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        if (PhotonNetwork.IsConnected)
        {
            TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
            PhotonNetwork.JoinLobby(customLobby);
        }
    }

    // Photonのサーバーから切断された時に呼ばれるコールバック
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
    }

    // ルームの作成が成功した時に呼ばれるコールバック
    public override void OnCreatedRoom()
    {
        Debug.Log("ルームの作成に成功しました");
    }

    // ルームの作成が失敗した時に呼ばれるコールバック
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームの作成に失敗しました: {message}");
    }

    // ルームへの参加が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへ参加しました");
    }

    // ルーム名を指定したルームへの参加が失敗した時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルームへの参加に失敗しました: {message}");
    }

    // ルームから退出した時に呼ばれるコールバック
    public override void OnLeftRoom()
    {
        Debug.Log("ルームから退出しました");
    }
}
