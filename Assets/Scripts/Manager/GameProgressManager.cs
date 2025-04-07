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
        private GameObject playerPrefab = null;

        private object[] datas = new object[10];

        private void Awake()
        {
            MapManagerData mapManagerData = new MapManagerData();
            mapManagerData.MapSpacesRoot = this.mapSpacesRoot;
            MapManager.Instance().DataTransfer(mapManagerData);

            PlayerManagerData playerManagerData = new PlayerManagerData();
            playerManagerData.PlayerRoot = this.playerRoot;
            PlayerManager.Instance().DataTransfer(playerManagerData);
        }

        private void Start()
        {
            GameStateManager.Instance().Initialize();
            MapManager.Instance().Initialize();
            PlayerManager.Instance().Initialize();

            GameStateManager.Instance().CompleteState(GameConst.GameProgressState.Initialize);
            GameStateManager.Instance().SendState(GameConst.GameProgressState.Initialize);
            TransitionState(GameConst.GameProgressState.Initialize);
        }

        private void ClearSendData()
        {
            for(int i = 0; i < datas.Length; ++i)
            {
                datas[i] = null;
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

                    TransitionState((GameConst.GameProgressState)data[0]);
                    break;

                case EventConst.Event.CreatePlayerObject:
                    CreatePlayerObject(data);
                    break;
            }
        }

        /// <summary>
        /// ステータスに関する振る舞い思考
        /// </summary>
        private void StatusBehavior()
        {
            switch(GameStateManager.Instance().CurrentState())
            {
                case GameConst.GameProgressState.Initialize:
                    TransitionState(GameConst.GameProgressState.Initialize);
                    break;
                case GameConst.GameProgressState.CreatedPlayerObject:
                    // 自身の生成
                    CreatePlayerModel();
                    PlayerManager.Instance().PlayerView.transform.localPosition = MapManager.Instance().GetStartMapSpace((int)PlayerManager.Instance().PlayerIndex).transform.localPosition;
                    GameStateManager.Instance().CompleteState(GameConst.GameProgressState.CreatedPlayerObject);
                    TransitionState(GameConst.GameProgressState.CreatedPlayerObject);
                    break;
                case GameConst.GameProgressState.InitializedFinish:
                    // カメラ起動
                    this.virtualCamera.Follow = PlayerManager.Instance().PlayerView.transform;
                    this.bootCameraAnime.PlayAnime("BootCamera", true, "BootCamera", null);
                    break;
            }
        }

        private void TransitionState(GameConst.GameProgressState state)
        {
            if (GameStateManager.Instance().IsCompleteState(state))
            {
                GameStateManager.Instance().TransitionState();
                StatusBehavior();
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
            var obj = Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);
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
            photonTransformView.m_SynchronizeRotation = false;
            photonTransformView.m_SynchronizeScale = true;
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
            // 受信したtransformを設定
            var obj = Instantiate(this.playerPrefab, Vector3.zero, Quaternion.identity);
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
            photonTransformView.m_SynchronizeRotation = false;
            photonTransformView.m_SynchronizeScale = true;
            photonTransformView.m_UseLocal = true;

            // PhotonViewに紐付ける
            photonView.ObservedComponents.Add(photonTransformView);

            // 受信したViewIDを用いて同期する
            photonView.ViewID = (int)data[0];

            GameStateManager.Instance().CompleteState(  GameConst.GameProgressState.CreatedPlayerObject,
                                                        (GameConst.PlayerIndex)data[1]);

            TransitionState(GameConst.GameProgressState.CreatedPlayerObject);
        }
    }
}
