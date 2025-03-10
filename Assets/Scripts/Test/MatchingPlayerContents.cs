using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPlayerContents : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName = null;

    [SerializeField]
    private GameObject mySelf = null;

    [SerializeField]
    private Image backImage = null;

    public void SetPlayerData(int order, Player player)
    {
        this.mySelf.SetActive(NetworkManager.Instance().IsMyself(player.UserId));
        this.playerName.text = player.NickName;

        if(order == 1)
        {
            this.backImage.color = Color.red;
        }
        else if(order == 2)
        {
            this.backImage.color = Color.blue;
        }
        else if (order == 3)
        {
            this.backImage.color = Color.green;
        }
        else if(order == 4)
        {
            this.backImage.color = Color.yellow;
        }
    }

    public void ClearData()
    {
        this.mySelf.SetActive(false);
        this.playerName.text = string.Empty;
        this.backImage.color = Color.white;
    }
}
