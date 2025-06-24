using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Akimichi.Lobby;
using UnityEngine.SceneManagement;
using Akimichi;

public class LobbyScene : MonoBehaviour
{
    private SortedDictionary<int, RoomInfo> cashRoomDic = new SortedDictionary<int, RoomInfo>();

    [SerializeField]
    private List<RoomContents> roomInfo = new List<RoomContents>();

    [SerializeField]
    private Transform listParent = null;

    [SerializeField]
    private Transform disableParent = null;

    private bool isInput = false;

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
            SceneManager.LoadScene(SceneConst.Matching);
        });
        NetworkManager.Instance().SetCallbackOnConnect(() => 
        { 
            NetworkManager.Instance().JoinLobby("akimichi"); 
        }); 
        NetworkManager.Instance().Connect();
        this.state = State.None;

        foreach(var room in this.roomInfo)
        {
            room.onInput = JoinRoom;
        }
    }

    private void Start()
    {
        NetworkManager.Instance().SetSysncScene(false);
        foreach (RoomContents room in this.roomInfo)
        {
            room.transform.SetParent(this.disableParent);
        }
        this.cashRoomDic.Clear();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.isInput = true;
        foreach(RoomInfo room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                RemoveRoomList(room);
            }
            else
            {
                AddRoomList(room);
            }
        }
        UpdateRoomList();
    }

    private void AddRoomList(RoomInfo room)
    {
        bool result = false;
        foreach(var item in this.cashRoomDic)
        {
            if(item.Value.Name == room.Name)
            {
                result = true;
                break;
            }
        }

        if (!result)
        {
            int index = 0;
            foreach (string name in LobbySceneConst.RoomNameList)
            {
                if (room.Name == name)
                {
                    break;
                }
                index++;
            }
            this.cashRoomDic.Add(index, room);
        }
    }

    private void RemoveRoomList(RoomInfo room)
    {
        bool result = false;
        foreach (var item in this.cashRoomDic)
        {
            if (item.Value.Name == room.Name)
            {
                result = true;
                break;
            }
        }

        if (result)
        {
            int index = 0;
            foreach (string name in LobbySceneConst.RoomNameList)
            {
                if (room.Name == name)
                {
                    break;
                }
                index++;
            }
            this.cashRoomDic.Remove(index);
        }
    }

    private void UpdateRoomList()
    {
        foreach (RoomContents room in this.roomInfo)
        {
            room.transform.SetParent(this.disableParent);
            room.ClearData();
        }

        int index = 0;
        foreach (var room in this.cashRoomDic)
        {
            if (index < this.roomInfo.Count)
            {
                this.roomInfo[index].transform.SetParent(this.listParent);
                this.roomInfo[index].SetRoomData(room.Value);
            }
            index++;
        }
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    public void CreateRoom()
    {
        if (!this.isInput) return;
        if (this.state == State.None)
        {
            this.state = State.CreateRoom;
            string roomName = string.Empty;
            foreach(string name in LobbySceneConst.RoomNameList)
            {
                bool isUse = false;
                foreach (var room in this.cashRoomDic)
                {
                    if(name == room.Value.Name)
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

            if (!string.IsNullOrEmpty(roomName))
            {
                NetworkManager.Instance().CreateRoom(roomName);
            }
        }
    }

    public void BackHome()
    {
        if(!this.isInput) return;
        SceneManager.LoadScene(SceneConst.Home);
    }

    public void JoinRoom(string name)
    {
        if(!this.isInput) return;
        NetworkManager.Instance().JoinRoom(name);
    }
}
