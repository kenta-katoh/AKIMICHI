using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomInformation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName = null;

    [SerializeField]
    private TextMeshProUGUI member = null;

    [SerializeField]
    private Button button = null;

    private void Awake()
    {
        button.clicked += JoinRoom;
    }

    public void SetRoomData(RoomInfo info)
    {
        roomName.text = info.Name;
        member.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinRoom()
    {
        Debug.Log("hoge");
        //NetworkManager.Instance().JoinRoom(roomName.text);
    }
}
