using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Akimichi.Game
{
    public class GameProgressManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField]
        private GameObject mapSpacesRoot = null;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera = null;

        [SerializeField]
        private GameObject playerRoot = null;

        [SerializeField]
        private AnimeController bootCameraAnime = null;

        [SerializeField]
        private AnimeController startAnime = null;

        [SerializeField]
        private List<GameObject> playerPrefabs = null;

        [SerializeField]
        private GameObject eventRoot = null;

        [SerializeField]
        private GameObject eventObject = null;

        [SerializeField]
        private DiceView diceView = null;

        [SerializeField]
        private PlayerStatusView playerStatusView = null;

        [SerializeField]
        private EventWindow eventWindow = null;

        [SerializeField]
        private TextMeshProUGUI timer = null;

        [SerializeField]
        private CanvasGroup canvasGroup = null;

        [SerializeField]
        private AnimeController finishAnime = null;

        [SerializeField]
        private PlayerLoupeView loupe = null;

        [SerializeField]
        private GameObject errorBtn = null;

        private System.Random rand = new System.Random();
        List<GameConst.PlayerIndex> playerList = new List<GameConst.PlayerIndex>();
        private EventBrain eventBrain = null;
        private bool isConnectError = false;

        private void Awake()
        {
            ClearData();
            NetworkManager.Instance().SetServerTime();
            GameStateManagerData stateManagerData = new GameStateManagerData();
            stateManagerData.ProgressManager = this;
            stateManagerData.Timer = this.timer;
            GameStateManager.Instance().DataTransfer(stateManagerData);

            MapManagerData mapManagerData = new MapManagerData();
            mapManagerData.MapSpacesRoot = this.mapSpacesRoot;
            MapManager.Instance().DataTransfer(mapManagerData);

            PlayerManagerData playerManagerData = new PlayerManagerData();
            playerManagerData.StatusView = this.playerStatusView;
            playerManagerData.PlayerLoupeView = this.loupe;
            PlayerManager.Instance().DataTransfer(playerManagerData);

            EventManagerData eventManagerData = new EventManagerData();
            for(int i = 0; i < 2; ++i)
            {
                var obj = Instantiate(this.eventObject, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(this.eventRoot.transform);
                eventManagerData.Events.Add(obj);
            }
            eventManagerData.EventWindow = this.eventWindow;
            EventManager.Instance().DataTransfer(eventManagerData);

            DiceManagerData diceManagerData = new DiceManagerData();
            diceManagerData.DiceView = this.diceView;
            DiceManager.Instance().DataTransfer(diceManagerData);

            // ホスト思考
            if (NetworkManager.Instance().IsMasterClient())
            {
                this.eventBrain = new EventBrain();
            }

            this.canvasGroup.blocksRaycasts = true;
            this.finishAnime.gameObject.SetActive(false);

            this.isConnectError = false;
            this.errorBtn.SetActive(this.isConnectError);
            NetworkManager.Instance().DeleteCallBack();
            NetworkManager.Instance().SetCallbackOnPlayerLeftRoom(OnPlayerLeftRoom);
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Game);
        }

        private void Start()
        {
            DataObjectManager.Instance().Initialize();
            GameStateManager.Instance().Initialize();
            MapManager.Instance().Initialize();
            PlayerManager.Instance().Initialize();
            ResultDataManager.Instance().Initialize();
            if (this.eventBrain != null) this.eventBrain.Initialize(MapManager.Instance().GetMapSpaces());

            var data = DataObjectManager.Instance().Get();
            data.Datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            data.Datas[1] = PlayerManager.Instance().GetName();
            NetworkManager.Instance().SendEvent(EventConst.Event.SetName, data);

            GameStateManager.Instance().SendState(GameConst.GameProgressState.Initialize);
        }

        public void OnEvent(EventData photonEvent)
        {
            var eventCode = photonEvent.Code;
            EventConst.Event _event = EventConst.ConvertEvent(eventCode);
            var data = (object[])photonEvent.CustomData;

            switch (_event)
            {
                case EventConst.Event.FinishState:
                    GameStateManager.Instance().CompleteState(  (GameConst.GameProgressState)data[0], 
                                                                (GameConst.PlayerIndex)data[1]);
                    break;
                case EventConst.Event.AffiliationMapSpace:
                    // マス所属はホスト思考
                    if (this.eventBrain != null)
                    {
                        GameConst.PlayerIndex playerIndex = (GameConst.PlayerIndex)data[1];
                        this.playerList.Clear();
                        this.playerList.AddRange(this.eventBrain.AffiliationMapSpace((int)data[0], playerIndex));
                        bool isEvent = false;
                        int eventId = 0;

                        // 稽古をしていない他プレイヤーの抽出
                        this.playerList.Remove(playerIndex);
                        List<GameConst.PlayerIndex> practicePlayers = new List<GameConst.PlayerIndex>();
                        var mapEvents = this.eventBrain.GetEventFromMapSpace((int)data[0]);
                        foreach (var item in mapEvents)
                        {
                            if (item.EventType == EventConst.MapEventType.Practice &&
                                item.MapEventState != EventConst.MapEventState.None)
                            {
                                foreach (var player in item.Players)
                                {
                                    if (!practicePlayers.Contains(player)) practicePlayers.Add(player);
                                }
                            }
                        }
                        foreach (var player in practicePlayers)
                        {
                            if (this.playerList.Contains(player)) this.playerList.Remove(player);
                        }

                        string playerStr = "";
                        switch (this.playerList.Count)
                        {
                            // 着地したマスに暇そうにしていた人が一人いたので稽古遷移
                            case 1:
                                isEvent = true;
                                this.playerList.Add(playerIndex);
                                eventId = this.eventBrain.CreateEvent(EventConst.MapEventType.Practice, (int)data[0], this.playerList);
                                // 該当プレイヤーを稽古待機状態へ変更
                                playerStr = CreatePlayerList(this.playerList);
                                break;
                            // 複数人暇している
                            case 2:
                            case 3:
                                // ランダムで誰かと稽古発生
                                int index = rand.Next(0, this.playerList.Count);
                                List<GameConst.PlayerIndex> practices = new List<GameConst.PlayerIndex>();
                                practices.Add(playerIndex);
                                practices.Add(this.playerList[index]);

                                isEvent = true;
                                eventId = this.eventBrain.CreateEvent(EventConst.MapEventType.Practice, (int)data[0], practices);
                                // 該当プレイヤーを稽古待機状態へ変更
                                playerStr = CreatePlayerList(practices);
                                break;
                        }

                        if (isEvent)
                        {
                            var send = DataObjectManager.Instance().Get();
                            send.Datas[0] = playerStr;
                            send.Datas[1] = eventId;
                            NetworkManager.Instance().SendEvent(EventConst.Event.WaitingPractice, send);
                        }
                    }
                    break;
                
                /////////////////////////////
                // マップイベント関連
                /////////////////////////////
                // 稽古待機状態
                case EventConst.Event.WaitingPractice:
                    if(IsReception(data))
                    {
                        EventManager.Instance().AddEventId((int)data[1]);
                        PlayerManager.Instance().WaitingEvent();
                    }
                    break;
                // 稽古可能状態の監視
                case EventConst.Event.PracticePossible:
                    if (this.eventBrain != null)
                    {
                        MapEventBase mapEvent = this.eventBrain.EventStartCheck((int)data[1], (GameConst.PlayerIndex)data[0]);
                        // プレイヤーがそろったのでイベント発火
                        if (mapEvent != null)
                        {
                            mapEvent.StartEvent();  // イベントを開始中へ
                            var send = DataObjectManager.Instance().Get();
                            send.Datas[0] = CreatePlayerList(mapEvent.Players);
                            send.Datas[1] = mapEvent.EventID;
                            NetworkManager.Instance().SendEvent(EventConst.Event.StartPractice, send);
                        }
                    }
                    break;
                // イベント開始
                case EventConst.Event.StartPractice:
                    // エフェクトの再生通知
                    if(this.eventBrain != null)
                    {
                        int eventId = (int)data[1];
                        MapEventBase mapEvent = this.eventBrain.GetEvent(eventId);
                        if(mapEvent != null)
                        {
                            var send = DataObjectManager.Instance().Get();
                            send.Datas[0] = eventId;
                            send.Datas[1] = (int)mapEvent.MapSpaceIndex;
                            NetworkManager.Instance().SendEvent(EventConst.Event.PracticeEffectStart, send);
                        }
                    }

                    // 自分自身が対象なら状態遷移
                    if (IsReception(data))
                    {
                        PlayerManager.Instance().StartPractice();
                    }
                    break;
                // 稽古エフェクト再生
                case EventConst.Event.PracticeEffectStart:
                    AudioManager.Instance().PlaySE(SoundConst.GAME.Practice);
                    MapSpaceLogicBase mapSpace = MapManager.Instance().GetMapSpace((int)data[1]);
                    if (this.eventBrain == null) EventManager.Instance().StartEventEffect(mapSpace, null);
                    else
                    {
                        // 稽古ステータス計算
                        MapEventBase mapEvent = this.eventBrain.GetEvent((int)data[0]);
                        var send = DataObjectManager.Instance().Get();
                        send.Datas[0] = CreatePlayerList(mapEvent.Players);
                        NetworkManager.Instance().SendEvent(EventConst.Event.CalcPractice, send);

                        // ホストはイベントエフェクト終了の監視
                        EventManager.Instance().StartEventEffect(mapSpace, () =>
                        {
                            MapEventBase mapEvent = this.eventBrain.GetEvent((int)data[0]);
                            var send = DataObjectManager.Instance().Get();
                            send.Datas[0] = CreatePlayerList(mapEvent.Players);
                            send.Datas[1] = mapEvent.EventID;
                            NetworkManager.Instance().SendEvent(EventConst.Event.EndPractice, send);
                        });
                    }
                    break;
                // 稽古ステータス計算
                case EventConst.Event.CalcPractice:
                    if (IsReception(data))
                    {
                        PlayerManager.Instance().CalcPractice();
                    }
                    break;
                // イベント終了
                case EventConst.Event.EndPractice:
                    if(this.eventBrain != null)
                    {
                        MapEventBase mapEvent = this.eventBrain.GetEvent((int)data[1]);
                        this.eventBrain.ReleaseEvent(mapEvent);
                    }

                    if (IsReception(data))
                    {
                        EventManager.Instance().ReleaseEvent((int)data[1]);
                        PlayerManager.Instance().ReleasePractice();
                    }
                    break;

                // ゲーム進行関連
                case EventConst.Event.CreatePlayerObject:
                    CreatePlayerObject(data);
                    break;
                case EventConst.Event.StartingPositionDistribution:
                    // もらったシードでスタート位置をシャッフル
                    List<int> list = new List<int>();
                    for(int i = 0; i < data.Length; ++i)
                    {
                        if(data[i] != null) list.Add((int)data[i]);
                    }
                    MapManager.Instance().StartPositionShuffle(list);

                    // スタート位置確定後に所属の送信
                    MapSpaceLogicBase logic = MapManager.Instance().GetStartMapSpace((int)PlayerManager.Instance().PlayerIndex);
                    MapManager.Instance().SendAffiliation(logic.Index);
                    PlayerManager.Instance().SetMapSpace(logic);

                    // スタート位置に配置
                    PlayerManager.Instance().SetPosInstantSync(logic.GetTransform().localPosition);
                    GameStateManager.Instance().SendState(GameConst.GameProgressState.StartPositionSetting);
                    break;

                // ステータス関連
                // 名前設定
                case EventConst.Event.SetName:
                    PlayerManager.Instance().SetName((GameConst.PlayerIndex)data[0], (string)data[1]);
                    break;
                // 体重増加
                case EventConst.Event.AddWeight:
                    PlayerManager.Instance().AddWeight((GameConst.PlayerIndex)data[0], (int)data[1]);
                    break;
                // 体重減少
                case EventConst.Event.SubtractWeight:
                    PlayerManager.Instance().SubtractWeight((GameConst.PlayerIndex)data[0], (int)data[1]);
                    break;
                // 疲労開始
                case EventConst.Event.HoldFatigue:
                    PlayerManager.Instance().HoldFatigue((GameConst.PlayerIndex)data[0]);
                    break;
                // 疲労終了
                case EventConst.Event.ReleaseFatigue:
                    PlayerManager.Instance().ReleaseFatigue((GameConst.PlayerIndex)data[0]);
                    break;
                // リザルトデータ関連
                case EventConst.Event.ResultData:
                    switch ((EventConst.ResultData)data[1])
                    {
                        case EventConst.ResultData.DiceCount:
                            ResultDataManager.Instance().AddDiceCount((GameConst.PlayerIndex)data[0]);
                            break;
                        case EventConst.ResultData.MoveValue:
                            ResultDataManager.Instance().AddMoveValue((GameConst.PlayerIndex)data[0], (int)data[2]);
                            break;
                        case EventConst.ResultData.SpaceCount:
                            ResultDataManager.Instance().AddMapSpace((GameConst.PlayerIndex)data[0], (GameConst.MapSpaceType)data[2]);
                            break;
                        case EventConst.ResultData.PracticeCount:
                            ResultDataManager.Instance().AddPracticeCount((GameConst.PlayerIndex)data[0]);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// ステータスに関する振る舞い思考
        /// </summary>
        public void StatusBehavior()
        {
            switch(GameStateManager.Instance().CurrentState())
            {
                case GameConst.GameProgressState.CreatedPlayerObject:
                    // 自身の生成
                    CreatePlayerModel();                    
                    GameStateManager.Instance().SendState(GameConst.GameProgressState.CreatedPlayerObject);
                    break;
                case GameConst.GameProgressState.StartPositionSetting:
                    PlayerManager.Instance().SetAsLast();
                    // スタート位置のランダム生成
                    // ホストのみで行って配布
                    if(NetworkManager.Instance().IsMasterClient())
                    {
                        var send = DataObjectManager.Instance().Get();
                        List<int> seed = MapManager.Instance().StartPositionSetting();
                        for(int i = 0; i < seed.Count; ++i)
                        {
                            send.Datas[i] = seed[i];
                        }
                        NetworkManager.Instance().SendEvent(EventConst.Event.StartingPositionDistribution, send);
                    }
                    break;
                case GameConst.GameProgressState.InitializedFinish:
                    TransitionManager.Instance().Open();
                    // カメラ起動
                    this.virtualCamera.Follow = PlayerManager.Instance().GetPlayerTransform();
                    this.bootCameraAnime.PlayAnime("BootCamera", true, "BootCamera", () => {
                        this.bootCameraAnime.enabled = false;
                        this.startAnime.PlayAnime("Start", true, "Start", () =>
                        {
                            this.startAnime.gameObject.SetActive(false);
                            GameStateManager.Instance().SendState(GameConst.GameProgressState.BootCamera);
                        });
                    });
                    break;
                case GameConst.GameProgressState.InGame:
                    PlayerManager.Instance().EnterGame();
                    GameStateManager.Instance().SetServerTime();
                    DebugManager.Instance().InGame(true);
                    break;
                case GameConst.GameProgressState.FinishGame:
                    this.finishAnime.gameObject.SetActive(true);
                    this.finishAnime.PlayAnime("Finish", true, "Finish", () =>
                    {
                        // ゲーム終了
                        PlayerManager.Instance().SendResultData();
                        TransitionManager.Instance().Transition(SceneConst.Result);
                    });
                    DebugManager.Instance().InGame(false);
                    break;
            }
        }

        private void Update()
        {
            if (!this.isConnectError)
            {
                GameStateManager.Instance().ManagedUpdate();
                PlayerManager.Instance().ManagedUpdate();
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

        private void CreatePlayerModel()
        {
            GameObject playerObj = this.playerPrefabs[(int)PlayerManager.Instance().PlayerIndex];
            var obj = Instantiate(playerObj, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(this.playerRoot.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            PlayerManager.Instance().SetPlayerData(obj);

            // Photon
            var photonView = obj.AddComponent<PhotonView>();
            var photonTransformView = obj.AddComponent<PhotonTransformView>();

            // 初期化を行う
            photonView.ObservedComponents = new List<Component>();

            // Synchronizeするものを設定
            photonTransformView.m_SynchronizePosition = true;
            photonTransformView.m_SynchronizeRotation = true;
            photonTransformView.m_SynchronizeScale = false;
            photonTransformView.m_UseLocal = true;

            // PhotonViewに紐付ける
            photonView.ObservedComponents.Add(photonTransformView);

            // 同期にはViewIDが必要なため取得する
            if (NetworkManager.Instance().IsAllocateViewID(photonView))
            {
                // 同じRoom内の他のユーザーへ通知
                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = photonView.ViewID;
                send.Datas[1] = (int)PlayerManager.Instance().PlayerIndex;
                NetworkManager.Instance().SendEvent(EventConst.Event.CreatePlayerObject, send);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId");
                Destroy(obj);
            }
        }

        private void CreatePlayerObject(object[] data)
        {
            if ((GameConst.PlayerIndex)data[1] != PlayerManager.Instance().PlayerIndex)
            {
                // 受信したtransformを設定
                GameObject playerObj = this.playerPrefabs[(int)data[1]];
                var obj = Instantiate(playerObj, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(this.playerRoot.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                PlayerView view = obj.GetComponent<PlayerView>();
                if (view != null)
                {
                    PlayerManager.Instance().ResistPlayer((GameConst.PlayerIndex)data[1], (PlayerLogic)view.Logic);
                }

                // Photon
                var photonView = obj.AddComponent<PhotonView>();
                var photonTransformView = obj.AddComponent<PhotonTransformView>();

                // 初期化を行う
                photonView.ObservedComponents = new List<Component>();

                // Synchronizeするものを設定
                photonTransformView.m_SynchronizePosition = true;
                photonTransformView.m_SynchronizeRotation = true;
                photonTransformView.m_SynchronizeScale = false;
                photonTransformView.m_UseLocal = true;

                // PhotonViewに紐付ける
                photonView.ObservedComponents.Add(photonTransformView);

                // 受信したViewIDを用いて同期する
                photonView.ViewID = (int)data[0];
            }
            GameStateManager.Instance().CompleteState(  GameConst.GameProgressState.CreatedPlayerObject,
                                                        (GameConst.PlayerIndex)data[1]);
        }

        // 送信プレイヤーの作成
        private string CreatePlayerList(List<GameConst.PlayerIndex> list)
        {
            string result = "";
            foreach(GameConst.PlayerIndex item in list)
            {
                result += Convert.ToString((int)item);
                result += ",";
            }
            return result;
        }

        /// <summary>
        /// 送信対象に自身が含まれているか
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        private bool IsReception(object[] objects)
        {
            bool result = false;
            string player = (string)objects[0];
            string[] arr = player.Split(',');
            for (int i = 0; i < arr.Length; ++i)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                {
                    if (PlayerManager.Instance().PlayerIndex == (GameConst.PlayerIndex)Convert.ToInt32(arr[i]))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 入力阻害
        /// </summary>
        public void FinishGame()
        {
            this.canvasGroup.blocksRaycasts = false;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom((Player)otherPlayer);
            this.isConnectError = true;
            this.errorBtn.SetActive(this.isConnectError);
        }

        public void LeftHome()
        {
            ClearData();
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            TransitionManager.Instance().Transition(SceneConst.Home);
        }

        private void ClearData()
        {
            DiceManager.Instance().Dispose();
            EventManager.Instance().Dispose();
            GameStateManager.Instance().Dispose();
            MapManager.Instance().Dispose();
            PlayerManager.Instance().Dispose();
            ResultDataManager.Instance().Dispose();
        }
    }
}
