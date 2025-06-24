using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomContents : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName = null;

    [SerializeField]
    private List<GameObject> member = null;

    [SerializeField]
    private List<GameObject> memberDisable = null;

    public Action<string> onInput { get; set; } = null;


    public void SetRoomData(RoomInfo info)
    {
        this.roomName.text = info.Name;
        int memberValue = info.PlayerCount;
        foreach (GameObject obj in this.member)
        {
            if(memberValue > 0)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
            memberValue--;
        }

        int disableMemberValue = info.MaxPlayers - info.PlayerCount;
        foreach (GameObject obj in this.memberDisable)
        {
            if (disableMemberValue > 0)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
            disableMemberValue--;
        }
    }

    public void ClearData()
    {
        this.roomName.text = string.Empty;
        foreach (GameObject obj in this.member)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in this.memberDisable)
        {
            obj.SetActive(true);
        }
    }

    public void JoinRoom()
    {
        if(this.roomName.text != string.Empty)
        {
            this.onInput?.Invoke(this.roomName.text);
        }
    }
}
