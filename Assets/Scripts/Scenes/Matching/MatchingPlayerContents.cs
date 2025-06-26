using Akimichi.Game;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPlayerContents : MonoBehaviour
{
    [SerializeField]
    private GameObject onPlayer = null;

    [SerializeField]
    private GameObject offPlayer = null;

    [SerializeField]
    private GameObject mySelf = null;

    [SerializeField]
    private Image backImage = null;

    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

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

        switch (this.playerIndex)
        {
            case GameConst.PlayerIndex.First:
                this.backImage.sprite = this.sprites[0];
                this.backImage.color = Color.red;
                break;
            case GameConst.PlayerIndex.Second:
                this.backImage.sprite = this.sprites[1];
                this.backImage.color = Color.blue;
                break;
            case GameConst.PlayerIndex.Third:
                this.backImage.sprite = this.sprites[2];
                this.backImage.color = Color.green;
                break;
            case GameConst.PlayerIndex.Fourth:
                this.backImage.sprite = this.sprites[3];
                this.backImage.color = Color.yellow;
                break;
        }
    }

    public void ClearData()
    {
        ContentVisible(false);
        this.mySelf.SetActive(false);
        this.backImage.color = Color.white;
        this.playerIndex = GameConst.PlayerIndex.First;
        this.IsNull = true;
        this.readyIcon.SetActive(false);
    }

    private void ContentVisible(bool visible)
    {
        this.onPlayer.SetActive(visible);
        this.offPlayer.SetActive(!visible);
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
