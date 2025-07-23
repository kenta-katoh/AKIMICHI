using Akimichi;
using Akimichi.Game;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingScene : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    private TextMeshProUGUI roomName = null;

    [SerializeField]
    private List<MatchingPlayerContents> playerList = new List<MatchingPlayerContents>();

    [SerializeField]
    private TMP_InputField inputField = null;

    [SerializeField]
    private GameObject inputIcon = null;

    [SerializeField]
    private Button readyBtn = null;

    [SerializeField]
    private TextMeshProUGUI readyText = null;

    private bool isReady = false;
    private GameConst.PlayerIndex playerIndex = GameConst.PlayerIndex.First;
    private List<GameConst.PlayerIndex> readyPlayer = new List<GameConst.PlayerIndex>();

    private void Awake()
    {
        NetworkManager.Instance().SetCallbackOnPlayerEnteredRoom(OnPlayerEnteredRoom);
        NetworkManager.Instance().SetCallbackOnPlayerLeftRoom(OnPlayerLeftRoom);
        this.isReady = false;
        this.inputIcon.SetActive(true);
        this.inputField.text = "どすこい";
        this.roomName.text = PhotonNetwork.CurrentRoom.Name;
        NetworkManager.Instance().SetName(this.inputField.text);

        // 作成ホストのみデバッグルーム設定ができる
        if(NetworkManager.Instance().IsMasterClient())
        {
            NetworkManager.Instance().SetDebugRoom(DebugManager.Instance().IsDebug);
            if(DebugManager.Instance().IsDebug)
            {
                NetworkManager.Instance().SetDebugRoomSetting(DebugManager.Instance().IsStartPlayer(),
                                                              DebugManager.Instance().GameTime());
            }
        }

        AudioManager.Instance().PlayBGM(SoundConst.BGM.Matching);
    }

    private void Start()
    {
        TransitionManager.Instance().Open();
        NetworkManager.Instance().SetSysncScene(true);
        UpdateRoomData();
    }

    /// <summary>
    /// ルーム退出
    /// </summary>
    public void LeaveRoom()
    {
        if (!this.isReady)
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Back);
            TransitionManager.Instance().Transition(SceneConst.Lobby);
        }
    }

    public void ReadyGame()
    {
        // 押下切り替え
        if (!this.isReady)
        {
            this.isReady = true;
            this.inputField.readOnly = true;
            this.inputIcon.SetActive(false);
            this.readyBtn.interactable = false;
            this.readyText.text = "待機中";
            SendReadyGame();

            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
        }
    }

    private void SendReadyGame()
    {
        if(this.isReady)
        {
            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)this.playerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.ReadyMatch, send);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateRoomData();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom((Player)otherPlayer);
        UpdateRoomData();
    }

    private void UpdateRoomData()
    {
        this.readyPlayer.Clear();
        this.playerIndex = NetworkManager.Instance().GetPlayerIndex(NetworkManager.Instance().GetUserID());
        foreach (var player in this.playerList)
        {
            player.ClearData();
        }

        int index = 0;
        var dic = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).ToList();
        foreach (var item in dic)
        {
            this.playerList[index].SetPlayerData((GameConst.PlayerIndex)index, item.Value);
            index++;
        }
        SendReadyGame();
    }

    public void ChangeName(string name)
    {
        NetworkManager.Instance().SetName(inputField.text);
    }

    public void OnEvent(EventData photonEvent)
    {
        var eventCode = photonEvent.Code;
        EventConst.Event _event = EventConst.ConvertEvent(eventCode);
        var data = (object[])photonEvent.CustomData;
        switch (_event)
        {
            case EventConst.Event.ReadyMatch:
                AudioManager.Instance().PlaySE(SoundConst.MATCHING.Ready);
                GameConst.PlayerIndex index = (GameConst.PlayerIndex)data[0];
                foreach (var item in this.playerList)
                {
                    item.Ready(index);
                }

                if (!this.readyPlayer.Contains(index)) this.readyPlayer.Add(index);
                if (this.readyPlayer.Count == GameConst.MaximumPlayers())
                {
                    AudioManager.Instance().PlaySE(SoundConst.MATCHING.TransGame);
                    if (NetworkManager.Instance().IsMasterClient())
                    {
                        // 全員そろったので遷移
                        TransitionManager.Instance().SysncTransition(SceneConst.Game);
                    }
                    else
                    {
                        TransitionManager.Instance().Close();
                    }
                }
                break;
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
