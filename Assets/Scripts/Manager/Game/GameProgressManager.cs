using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
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
        private List<GameObject> playerPrefabs = null;

        [SerializeField]
        private GameObject eventRoot = null;

        [SerializeField]
        private GameObject eventObject = null;

        [SerializeField]
        private DiceView diceView = null;

        private object[] datas = new object[10];
        private EventBrain eventBrain = null;

        private void Awake()
        {
            GameStateManagerData stateManagerData = new GameStateManagerData();
            stateManagerData.ProgressManager = this;
            GameStateManager.Instance().DataTransfer(stateManagerData);

            MapManagerData mapManagerData = new MapManagerData();
            mapManagerData.MapSpacesRoot = this.mapSpacesRoot;
            MapManager.Instance().DataTransfer(mapManagerData);

            PlayerManagerData playerManagerData = new PlayerManagerData();
            playerManagerData.PlayerRoot = this.playerRoot;
            PlayerManager.Instance().DataTransfer(playerManagerData);

            EventManagerData eventManagerData = new EventManagerData();
            for(int i = 0; i < 2; ++i)
            {
                var obj = Instantiate(this.eventObject, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(this.eventRoot.transform);
                eventManagerData.Events.Add(obj);
            }
            EventManager.Instance().DataTransfer(eventManagerData);

            DiceManagerData diceManagerData = new DiceManagerData();
            diceManagerData.DiceView = this.diceView;
            DiceManager.Instance().DataTransfer(diceManagerData);

            // ホスト思考
            if (NetworkManager.Instance().IsMasterClient())
            {
                this.eventBrain = new EventBrain();
            }
        }

        private void Start()
        {
            GameStateManager.Instance().Initialize();
            MapManager.Instance().Initialize();
            PlayerManager.Instance().Initialize();
            if (this.eventBrain != null) this.eventBrain.Initialize(MapManager.Instance().GetMapSpaces());

            GameStateManager.Instance().SendState(GameConst.GameProgressState.Initialize);
        }

        private void ClearSendData()
        {
            for(int i = 0; i < this.datas.Length; ++i)
            {
                this.datas[i] = null;
            }
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
                        List<GameConst.PlayerIndex> players = this.eventBrain.AffiliationMapSpace((int)data[0], (GameConst.PlayerIndex)data[1]);
                        if(GameStateManager.Instance().CurrentState() == GameConst.GameProgressState.InGame)
                        {
                            ClearSendData();
                            bool isEvent = false;
                            int eventId = 0;
                            // 着地したマスに2人以上所属していたので稽古遷移
                            if (players.Count > 1)
                            {
                                isEvent = true;
                                eventId = this.eventBrain.CreateEvent(EventConst.MapEventType.Practice, (int)data[0], players);
                                // 該当プレイヤーを稽古待機状態へ変更
                                this.datas[0] = CreatePlayerList(players);
                            }
                            else
                            {
                                // 誰もいなかったのでマス目の思考
                                if ((int)data[2] == 0)
                                {
                                    isEvent = true;
                                    // 残りの進むダイス目がなかったので、マス目の着地と判断
                                    MapSpaceLogicBase mapSpace1 = MapManager.Instance().GetMapSpace((int)data[0]);
                                    EventConst.MapEventType eventType = EventConst.MapEventType.None;
                                    
                                    switch (mapSpace1.MapSpaceType)
                                    {
                                        case GameConst.MapSpaceType.Plus:
                                            eventType = EventConst.MapEventType.Plus;
                                            break;
                                        case GameConst.MapSpaceType.Minus:
                                            eventType = EventConst.MapEventType.Minus;
                                            break;
                                        case GameConst.MapSpaceType.Event:
                                            eventType = EventConst.MapEventType.Event;
                                            break;
                                    }
                                    eventId = this.eventBrain.CreateEvent(eventType, (int)data[0], (GameConst.PlayerIndex)data[1]);
                                    // 該当プレイヤーをイベント待機状態へ変更
                                    this.datas[0] = CreatePlayerList((GameConst.PlayerIndex)data[1]);
                                }
                            }

                            if(isEvent)
                            {
                                this.datas[1] = eventId;
                                NetworkManager.Instance().SendEvent(EventConst.Event.WaitingEvent, this.datas);
                            }
                        }
                    }
                    break;
                
                /////////////////////////////
                // マップイベント関連
                /////////////////////////////
                // マップイベント待機状態
                case EventConst.Event.WaitingEvent:
                    if(IsReception(data))
                    {
                        EventManager.Instance().AddEventId((int)data[1]);
                        PlayerManager.Instance().WaitingEvent();
                    }
                    break;
                // マップイベント可能状態の監視
                case EventConst.Event.EventPossible:
                    if (this.eventBrain != null)
                    {
                        MapEventBase mapEvent = this.eventBrain.EventStartCheck((int)data[1], (GameConst.PlayerIndex)data[0]);
                        // プレイヤーがそろったのでイベント発火
                        if (mapEvent != null)
                        {
                            mapEvent.StartEvent();  // イベントを開始中へ
                            ClearSendData();
                            this.datas[0] = CreatePlayerList(mapEvent.Players);
                            this.datas[1] = mapEvent.EventID;
                            this.datas[2] = (int)mapEvent.EventType;
                            NetworkManager.Instance().SendEvent(EventConst.Event.StartEvent, this.datas);
                        }
                    }
                    break;
                // イベント開始
                case EventConst.Event.StartEvent:
                    // エフェクトの再生通知
                    if(this.eventBrain != null)
                    {
                        int eventId = (int)data[1];
                        MapEventBase mapEvent = this.eventBrain.GetEvent(eventId);
                        if(mapEvent != null)
                        {
                            AkimichiLog(mapEvent.EventType.ToString());
                            ClearSendData();
                            this.datas[0] = eventId;
                            this.datas[1] = (int)mapEvent.MapSpaceIndex;
                            this.datas[2] = (int)mapEvent.EventType;
                            NetworkManager.Instance().SendEvent(EventConst.Event.EventEffectStart, this.datas);

                            // ステータス計算
                            switch ((EventConst.MapEventType)data[2])
                            {
                                case EventConst.MapEventType.Practice:
                                    break;
                                case EventConst.MapEventType.Plus:
                                    break;
                                case EventConst.MapEventType.Minus:
                                    break;
                                case EventConst.MapEventType.Event:
                                    break;
                            }
                        }
                    }

                    // 自分自身が対象なら状態遷移
                    if (IsReception(data))
                    {
                        PlayerManager.Instance().StartEvent();
                    }
                    break;
                // 稽古エフェクト再生
                case EventConst.Event.EventEffectStart:
                    MapSpaceLogicBase mapSpace = MapManager.Instance().GetMapSpace((int)data[1]);
                    if(this.eventBrain == null) EventManager.Instance().StartEventEffect((EventConst.MapEventType)data[2], mapSpace, null);
                    else EventManager.Instance().StartEventEffect((EventConst.MapEventType)data[2], mapSpace, () => 
                    {
                        // ホストはイベントエフェクト終了の監視
                        MapEventBase mapEvent = this.eventBrain.GetEvent((int)data[0]);
                        ClearSendData();
                        this.datas[0] = CreatePlayerList(mapEvent.Players);
                        this.datas[1] = mapEvent.EventID;
                        NetworkManager.Instance().SendEvent(EventConst.Event.EventRelease, this.datas);
                        mapEvent.ReleaseEvent();
                    });
                    break;
                // イベント終了
                case EventConst.Event.EventRelease:
                    if (IsReception(data))
                    {
                        EventManager.Instance().ReleaseEvent((int)data[1]);
                        if(EventManager.Instance().GetEvent() > 0)
                        {
                            // まだつまれているイベントが存在する
                            PlayerManager.Instance().SendReEventPossible();
                        }
                        else
                        {
                            PlayerManager.Instance().ReleaseEvent();
                        }
                    }
                    break;

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
                    MapManager.Instance().SendAffiliation(logic.Index, 0);
                    PlayerManager.Instance().SetMapSpace(logic);

                    // スタート位置に配置
                    PlayerManager.Instance().SetPosInstantSync(logic.GetTransform().localPosition);
                    GameStateManager.Instance().SendState(GameConst.GameProgressState.StartPositionSetting);
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
                    // スタート位置のランダム生成
                    // ホストのみで行って配布
                    if(NetworkManager.Instance().IsMasterClient())
                    {
                        ClearSendData();
                        List<int> seed = MapManager.Instance().StartPositionSetting();
                        for(int i = 0; i < seed.Count; ++i)
                        {
                            this.datas[i] = seed[i];
                        }
                        NetworkManager.Instance().SendEvent(EventConst.Event.StartingPositionDistribution, this.datas);
                    }
                    break;
                case GameConst.GameProgressState.InitializedFinish:
                    // カメラ起動
                    this.virtualCamera.Follow = PlayerManager.Instance().GetPlayerTransform();
                    this.bootCameraAnime.PlayAnime("BootCamera", true, "BootCamera", () => {
                        GameStateManager.Instance().SendState(GameConst.GameProgressState.BootCamera);
                    });
                    break;
                case GameConst.GameProgressState.InGame:
                    PlayerManager.Instance().EnterGame();
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
                ClearSendData();
                this.datas[0] = photonView.ViewID;
                this.datas[1] = (int)PlayerManager.Instance().PlayerIndex;
                NetworkManager.Instance().SendEvent(EventConst.Event.CreatePlayerObject, this.datas);
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
        private string CreatePlayerList(GameConst.PlayerIndex player)
        {
            return Convert.ToString((int)player);
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
                if (PlayerManager.Instance().PlayerIndex == (GameConst.PlayerIndex)Convert.ToInt32(arr[i]))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void AkimichiLog(string message)
        {
            Debug.Log("akimichi_log : " + message);
        }
    }
}
