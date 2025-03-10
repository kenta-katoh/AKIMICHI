using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomContents : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName = null;

    [SerializeField]
    private TextMeshProUGUI member = null;

    public void SetRoomData(RoomInfo info)
    {
        roomName.text = info.Name;
        member.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void ClearData()
    {
        roomName.text = string.Empty;
        member.text = string.Empty;
    }

    public void JoinRoom()
    {
        if(roomName.text != string.Empty)
        {
            NetworkManager.Instance().JoinRoom(roomName.text);
        }
    }
}
