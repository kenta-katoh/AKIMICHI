using Akimichi.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi
{
    public class ResultScene : MonoBehaviour
    {
        [SerializeField]
        private Image player1stImage = null;

        [SerializeField]
        private List<Sprite> player1stList = new List<Sprite>();

        [SerializeField]
        private Image player2ndImage = null;

        [SerializeField]
        private List<Sprite> player2ndList = new List<Sprite>();

        [SerializeField]
        private Image player3rdImage = null;

        [SerializeField]
        private List<Sprite> player3rdList = new List<Sprite>();

        [SerializeField]
        private Image player4thImage = null;

        [SerializeField]
        private List<Sprite> player4thList = new List<Sprite>();

        [SerializeField]
        private AnimeController resultAnime = null;

        [SerializeField]
        private GameObject winEffect = null;

        [SerializeField]
        private GameObject transition = null;

        private void Awake()
        {
            TransitionManager.Instance().AddScene(SceneConst.Result);
            NetworkManager.Instance().DeleteCallBack();
            NetworkManager.Instance().LeaveRoom();
            NetworkManager.Instance().Disconnect();

            DiceManager.Instance().Dispose();
            EventManager.Instance().Dispose();
            GameStateManager.Instance().Dispose();
            MapManager.Instance().Dispose();
            PlayerManager.Instance().Dispose();

            // 順位決定
            Dictionary<GameConst.PlayerIndex, int> result = new Dictionary<GameConst.PlayerIndex, int>();
            foreach (var item in Enum.GetValues(typeof(GameConst.PlayerIndex)))
            {
                GameConst.PlayerIndex playerIndex = (GameConst.PlayerIndex)item;
                PlayerData data = ResultDataManager.Instance().GetPlayerData(playerIndex);
                result.Add(playerIndex, data.Weight);
            }

            int index = 0;
            foreach(var item in result.OrderByDescending((c) => c.Value))
            {
                Image image = null;
                switch(index)
                {
                    case 0:
                        image = this.player1stImage;
                        break;
                    case 1:
                        image = this.player2ndImage;
                        break;
                    case 2:
                        image = this.player3rdImage;
                        break;
                    case 3:
                        image = this.player4thImage;
                        break;
                }
                index++;

                PlayerData data = ResultDataManager.Instance().GetPlayerData(item.Key);
                switch(item.Key)
                {
                    case GameConst.PlayerIndex.First:
                        image.sprite = this.player1stList[data.GetLevel()];
                        break;
                    case GameConst.PlayerIndex.Second:
                        image.sprite = this.player2ndList[data.GetLevel()];
                        break;
                    case GameConst.PlayerIndex.Third:
                        image.sprite = this.player3rdList[data.GetLevel()];
                        break;
                    case GameConst.PlayerIndex.Fourth:
                        image.sprite = this.player4thList[data.GetLevel()];
                        break;
                }
            }
            this.transition.SetActive(false);
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
            this.resultAnime.PlayAnime("End", true, "End", () =>
            {
                this.winEffect.SetActive(true);
                this.transition.SetActive(true);
                ResultDataManager.Instance().Dispose();
            });
        }

        public void OnLobby()
        {
            TransitionManager.Instance().Transition(SceneConst.Lobby);
        }

        public void OnHome()
        {
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
