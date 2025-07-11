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
        private Image player2ndImage = null;

        [SerializeField]
        private Image player3rdImage = null;

        [SerializeField]
        private Image player4thImage = null;

        [SerializeField]
        private AnimeController resultAnime = null;

        [SerializeField]
        private GameObject winEffect = null;

        [SerializeField]
        private GameObject transition = null;

        private void Awake()
        {
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
                Sprite sprite = PlayerSpriteManager.Instance().GetSprite(item.Key, data.GetLevel());
                if(sprite != null) image.sprite = sprite;
            }
            this.transition.SetActive(false);

            AudioManager.Instance().PlayBGM(SoundConst.BGM.Result);
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
            this.resultAnime.PlayAnime("End", true, "End", () =>
            {
                this.winEffect.SetActive(true);
                this.transition.SetActive(true);
            });
        }

        public void OnResultData()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            TransitionManager.Instance().Transition(SceneConst.ResultData);
        }
    }
}
