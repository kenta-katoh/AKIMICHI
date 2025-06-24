using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class ResultScene : MonoBehaviour
    {
        [SerializeField]
        private AnimeController resultAnime = null;

        [SerializeField]
        private GameObject winEffect = null;

        private void Awake()
        {
            NetworkManager.Instance().DeleteCallBack();
            //NetworkManager.Instance().LeaveRoom();
            NetworkManager.Instance().Disconnect();

            DiceManager.Instance().Dispose();
            EventManager.Instance().Dispose();
            GameStateManager.Instance().Dispose();
            MapManager.Instance().Dispose();
            PlayerManager.Instance().Dispose();
        }

        private void Start()
        {
            this.resultAnime.PlayAnime("End", true, "End", () =>
            {
                this.winEffect.SetActive(true);
            });
        }
    }
}
