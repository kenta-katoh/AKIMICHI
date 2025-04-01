using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
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
        private Animator bootCameraAnime = null;

        [SerializeField]
        private GameObject playerPrefab = null;

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
            MapManager.Instance().Initialize();
            PlayerManager.Instance().Initialize();

            CreatePlayerModel();
            PlayerManager.Instance().PlayerView.transform.localPosition = MapManager.Instance().GetStartMapSpace((int)PlayerManager.Instance().PlayerIndex).transform.localPosition;
        }

        public void BootCamera()
        {
            this.virtualCamera.Follow = PlayerManager.Instance().PlayerView.transform;
            this.bootCameraAnime.SetBool("BootCamera", true);
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
                var data = new object[]
                {
                    photonView.ViewID,
                };
                NetworkManager.Instance().SendEvent(EventConst.Event.CreatePlayerObject, data);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId");
                Destroy(obj);
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            var eventCode = photonEvent.Code;
            EventConst.Event _event = EventConst.ConvertEvent(eventCode);
            var data = (object[])photonEvent.CustomData;

            switch (_event)
            {
                case EventConst.Event.CreatePlayerObject:
                    CreatePlayerObject((int)data[0]);
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

        private void CreatePlayerObject(int id)
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
            photonView.ViewID = id;
        }
    }
}
