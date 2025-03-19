using Akimichi.Game;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class GameProgressManager : MonoBehaviour
    {
        private static GameProgressManager instance = null;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera = null;

        [SerializeField]
        private List<PlayerView> players = new List<PlayerView>();

        [SerializeField]
        private Animator bootCameraAnime = null;

        public static GameProgressManager Instance()
        {
            if (instance == null) instance = new GameProgressManager();
            return instance;
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.P))
            {
                BootCamera();
            }
        }

        private void BootCamera()
        {
            this.virtualCamera.Follow = this.players[0].transform;
            this.bootCameraAnime.SetBool("BootCamera", true);
        }
    }
}
