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

    private GameConst.PlayerIndex playerIndex = GameConst.PlayerIndex.First;
    public bool IsNull { get; private set; } = true;

    public void SetPlayerData(GameConst.PlayerIndex index, Player player)
    {
        ContentVisible(true);
        this.mySelf.SetActive(NetworkManager.Instance().IsMyself(player.UserId));
        this.playerIndex = index;
        this.IsNull = false;
        this.readyIcon.SetActive(false);
        this.player.sprite = PlayerSpriteManager.Instance().GetUISprite(this.playerIndex, 0);
    }

    public void ClearData()
    {
        ContentVisible(false);
        this.mySelf.SetActive(false);
        this.playerIndex = GameConst.PlayerIndex.First;
        this.IsNull = true;
        this.readyIcon.SetActive(false);
    }

    private void ContentVisible(bool visible)
    {
        //this.onPlayer.SetActive(visible);
        this.player.gameObject.SetActive(visible);
    }

    /// <summary>
    /// 準備完了のアイコン
    /// </summary>
    /// <param name="index"></param>
    public void Ready(GameConst.PlayerIndex index)
    {
        if(!this.IsNull && this.playerIndex == index)
        {
            this.readyIcon?.SetActive(true);
        }
    }
}
