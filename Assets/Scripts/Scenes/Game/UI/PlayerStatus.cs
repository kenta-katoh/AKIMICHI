using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Akimichi.Game
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> iconImages = new List<Sprite>();

        [SerializeField]
        private Image playerImage = null;

        [SerializeField]
        private GameObject fatigueIcon = null;

        [SerializeField]
        private TextMeshProUGUI weight = null;

        [SerializeField]
        private TextMeshProUGUI playerName = null;

        [SerializeField]
        private AnimeController animeController = null;

        private GameConst.PlayerIndex playerIndex;
        private PlayerData playerData = new PlayerData();
        private int currentWeight = 0;

        /// <summary>
        /// 名前設定
        /// </summary>
        /// <param name="name"></param>
        public void SetName(GameConst.PlayerIndex index, string name)
        {
            this.playerIndex = index;
            this.playerData.Name = name;
            this.playerName.text = this.playerData.Name;
        }

        /// <summary>
        /// 体重強制変化(演出なし)
        /// </summary>
        /// <param name="value"></param>
        public void ForceSetWeight(int value)
        {
            this.playerData.Weight = value;
            this.weight.text = this.playerData.Weight.ToString() + "kg";
        }

        /// <summary>
        /// 体重増加
        /// </summary>
        /// <param name="value"></param>
        public void AddWeight(int value)
        {
            int currentLevel = this.playerData.GetLevel();
            this.currentWeight = this.playerData.Weight;
            this.playerData.Weight += value;
            if(currentLevel != this.playerData.GetLevel())
            {
                PlayerManager.Instance().ChangePlayerView(this.playerIndex, this.playerData.GetLevel());
            }

            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => this.currentWeight, (n) => this.currentWeight = n, this.playerData.Weight, 1)
                .OnUpdate(() => this.weight.text = this.currentWeight.ToString() + "kg"));
            seq.OnComplete(() =>
            {
                seq.Kill();
                seq = null;
                this.currentWeight = this.playerData.Weight;
                this.weight.text = this.currentWeight.ToString() + "kg";
            });

            this.animeController.PlayAnime("Up", true, "Up", () =>
            {
                this.animeController.SetBool("Up", false);
            });
        }

        /// <summary>
        /// 体重減少
        /// </summary>
        /// <param name="value"></param>
        public void SubtractWeight(int value)
        {
            int currentLevel = this.playerData.GetLevel();
            this.currentWeight = this.playerData.Weight;
            this.playerData.Weight -= value;
            if(this.playerData.Weight < 1) this.playerData.Weight = 1;
            if (currentLevel != this.playerData.GetLevel())
            {
                PlayerManager.Instance().ChangePlayerView(this.playerIndex, this.playerData.GetLevel());
            }

            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => this.currentWeight, (n) => this.currentWeight = n, this.playerData.Weight, 1)
                .OnUpdate(() => this.weight.text = this.currentWeight.ToString() + "kg"));
            seq.OnComplete(() =>
            {
                seq.Kill();
                seq = null;
                this.currentWeight = this.playerData.Weight;
                this.weight.text = this.currentWeight.ToString() + "kg";
            });

            this.animeController.PlayAnime("Down", true, "Down", () =>
            {
                this.animeController.SetBool("Down", false);
            });
        }

        /// <summary>
        /// 疲労アイコン
        /// </summary>
        /// <param name="flag"></param>
        public void VisibleFatigue(bool flag)
        {
            this.fatigueIcon.SetActive(flag);
        }

        /// <summary>
        /// 体重取得
        /// </summary>
        /// <returns></returns>
        public int GetWeight()
        {
            return this.playerData.Weight;
        }
    }
}
