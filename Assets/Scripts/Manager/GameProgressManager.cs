using Akimichi.Game;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class GameProgressManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject mapSpacesRoot = null;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera = null;

        [SerializeField]
        private GameObject playerRoot = null;

        [SerializeField]
        private Animator bootCameraAnime = null;

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

            MapManager.Instance().CreateData();
            PlayerManager.Instance().CreateData();
        }

        public void BootCamera()
        {
            this.virtualCamera.Follow = PlayerManager.Instance().GetLocalPlayer().PlayerView.transform;
            this.bootCameraAnime.SetBool("BootCamera", true);
        }
    }
}
