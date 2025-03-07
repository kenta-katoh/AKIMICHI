using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectTest : MonoBehaviour
{
    private List<RoomInfo> cashRoomList = new List<RoomInfo>();

    [SerializeField]
    private List<RoomInformation> roomInfo = new List<RoomInformation>();

    [SerializeField]
    private Transform listParent = null;

    [SerializeField]
    private Transform disableParent = null;

    [SerializeField]
    private TMP_InputField inputField = null;

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
        foreach (RoomInformation room in roomInfo)
        {
            room.transform.parent = this.disableParent;
        }
        this.cashRoomList.Clear();
        this.inputField.readOnly = false;
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
        foreach (RoomInformation room in this.roomInfo)
        {
            room.transform.parent = this.disableParent;
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
        if (this.state == State.None && CreateRoomCheck())
        {
            this.state = State.CreateRoom;
            this.inputField.readOnly = true;

            NetworkManager.Instance().CreateRoom(this.inputField.text);
        }
    }

    private bool CreateRoomCheck()
    {
        bool isCreate = false;
        if (this.inputField.text != string.Empty)
        {
            isCreate = true;
            foreach (RoomInfo room in this.cashRoomList)
            {
                if (room.Name == this.inputField.text)
                {
                    isCreate = false;
                    break;
                }
            }
        }
        return isCreate;
    }
}
