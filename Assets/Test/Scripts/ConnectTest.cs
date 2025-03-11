using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using Akimichi.Lobby;

public class ConnectTest : MonoBehaviour
{
    private List<RoomInfo> cashRoomList = new List<RoomInfo>();

    [SerializeField]
    private List<RoomContents> roomInfo = new List<RoomContents>();

    [SerializeField]
    private Transform listParent = null;

    [SerializeField]
    private Transform disableParent = null;

    private enum State
    {
        None,
        CreateRoom,
    }
    private State state = State.None;

    private void Awake()
    {
        NetworkManager.Instance().SetCallbackOnRoomListUpdate(OnRoomListUpdate);
        NetworkManager.Instance().SetCallbackOnJoinedRoom(() => 
        {
            TestSceneManager.Instance().ChangeScene("MatchingScene");
        });
        NetworkManager.Instance().SetCallbackOnConnect(() => 
        { 
            NetworkManager.Instance().JoinLobby("akimichi"); 
        }); 
        NetworkManager.Instance().Connect();
        this.state = State.None;
    }

    private void Start()
    {
        foreach (RoomContents room in roomInfo)
        {
            room.transform.parent = this.disableParent;
        }
        this.cashRoomList.Clear();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.cashRoomList.Clear();
        foreach(RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                continue;
            }
            this.cashRoomList.Add(room);
        }
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        foreach (RoomContents room in this.roomInfo)
        {
            room.transform.parent = this.disableParent;
            room.ClearData();
        }

        int index = 0;
        foreach (RoomInfo room in this.cashRoomList)
        {
            if (index < this.roomInfo.Count)
            {
                this.roomInfo[index].transform.parent = this.listParent;
                this.roomInfo[index].SetRoomData(room);
            }
            index++;
        }
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    public void CreateRoom()
    {
        if (this.state == State.None)
        {
            this.state = State.CreateRoom;
            string roomName = string.Empty;
            foreach(string name in LobbySceneConst.RoomNameList)
            {
                bool isUse = false;
                foreach (RoomInfo room in this.cashRoomList)
                {
                    if(name == room.Name)
                    {
                        isUse = true; 
                        break;
                    }
                }

                if(!isUse)
                {
                    roomName = name;
                    break;
                }
            }

            if(!string.IsNullOrEmpty(roomName))
            {
                NetworkManager.Instance().CreateRoom(roomName);
            }
        }
    }
}
