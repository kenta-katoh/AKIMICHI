using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchingPlayerContents : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName = null;

    [SerializeField]
    private GameObject mySelf = null;

    public void SetPlayerData(Player player)
    {
        this.mySelf.SetActive(NetworkManager.Instance().IsMyself(player.UserId));
        this.playerName.text = player.NickName;
    }

    public void ClearData()
    {
        this.mySelf.SetActive(false);
        this.playerName.text = string.Empty;
    }
}
