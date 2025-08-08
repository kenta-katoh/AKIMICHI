using Akimichi;
using Akimichi.Game;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPlayerContents : MonoBehaviour
{
    [SerializeField]
    private Image player = null;

    [SerializeField]
    private GameObject mySelf = null;

    [SerializeField]
    private GameObject readyIcon = null;

    private string userID = string.Empty;
    private GameConst.PlayerIndex playerIndex = GameConst.PlayerIndex.First;
    public bool IsNull { get; private set; } = true;

    public void SetPlayerData(GameConst.PlayerIndex index, Player player)
    {
        ContentVisible(true);
        this.mySelf.SetActive(NetworkManager.Instance().IsMyself(player.UserId));
        this.userID = player.UserId;
        this.playerIndex = index;
        this.IsNull = false;
        this.readyIcon.SetActive(false);
        this.player.sprite = PlayerSpriteManager.Instance().GetUISprite(this.playerIndex, 0);
    }

    public void ClearData()
    {
        ContentVisible(false);
        this.mySelf.SetActive(false);
        this.userID = string.Empty;
        this.playerIndex = GameConst.PlayerIndex.First;
        this.IsNull = true;
        this.readyIcon.SetActive(false);
    }

    private void ContentVisible(bool visible)
    {
        this.player.gameObject.SetActive(visible);
    }

    /// <summary>
    /// 準備完了のアイコン
    /// </summary>
    /// <param name="index"></param>
    public bool Ready(string id)
    {
        if(!this.IsNull && this.userID == id)
        {
            this.readyIcon?.SetActive(true);
            return true;
        }
        return false;
    }
}
