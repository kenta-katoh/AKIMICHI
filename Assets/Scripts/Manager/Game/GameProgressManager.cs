using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
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
                    bool isEvent = false;
                    List<GameConst.PlayerIndex> players = null;
                    if (this.eventBrain != null)
                    {
                        players = this.eventBrain.AffiliationMapSpace((int)data[0], (GameConst.PlayerIndex)data[1]);
                        // 着地したマスに2人以上所属していたので稽古遷移
                        if (players != null && players.Count > 1)
                        {
                            // 稽古待機状態へ
                            isEvent = true;
                            int index = 0;
                            ClearSendData();
                            foreach (GameConst.PlayerIndex item in players)
                            {
                                this.datas[index] = (int)item;
                                index++;
                            }
                        }
                    }

                    if (isEvent)
                    {
                        MapSpaceLogicBase mapSpace = MapManager.Instance().GetMapSpace((int)data[0]);
                        EventDataBase mapEvent = EventManager.Instance().EventBooking(mapSpace.GetTransform());
                        if(this.eventBrain != null && players != null)
                        {
                            // 稽古イベント開始の監視対象の登録
                            this.eventBrain.AddPracticeEvent(mapEvent, players);
                            NetworkManager.Instance().SendEvent(EventConst.Event.WaitingPractice, this.datas);
                        }
                    }
                    break;
                
                /////////////////////////////
                // 稽古関連
                /////////////////////////////
                // 稽古待機状態
                case EventConst.Event.WaitingPractice:
                    if(IsReception(data))
                    {
                        PlayerManager.Instance().WaitingPractice();
                    }
                    break;
                // 稽古可能状態の監視（ブレインだけで良さそう）
                case EventConst.Event.PracticePossible:
                    if (this.eventBrain != null)
                    {
                        EventDataBase eventData = this.eventBrain.PracticeStartCheck((GameConst.PlayerIndex)data[0]);
                        // プレイヤーがそろったので稽古発火
                        if (eventData != null)
                        {
                            EventManager.Instance().SendPracticeBegins((PracticeEventData)eventData);
                        }
                    }
                    break;
                // 稽古開始
                case EventConst.Event.PracticeBegins:
                    int eventId = (int)data[0];
                    EventManager.Instance().PracticeBegins(eventId);
                    if((GameConst.PlayerIndex)data[1] == PlayerManager.Instance().PlayerIndex ||
                        (GameConst.PlayerIndex)data[2] == PlayerManager.Instance().PlayerIndex)
                    {
                        PlayerManager.Instance().PracticeBegins();
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
                    MapManager.Instance().SendAffiliation(logic.Index);
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

        /// <summary>
        /// 送信対象に自身が含まれているか
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        private bool IsReception(object[] objects)
        {
            bool result = false;
            for (int i = 0; i < objects.Length; ++i)
            {
                if (objects[i] != null)
                {
                    if (PlayerManager.Instance().PlayerIndex == (GameConst.PlayerIndex)(int)objects[i])
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
