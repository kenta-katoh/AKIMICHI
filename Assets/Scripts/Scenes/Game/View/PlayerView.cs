using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerView : ViewBase
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private AnimeController upAnime = null;

        [SerializeField]
        private AnimeController downAnime = null;

        [SerializeField]
        private AnimeController changeAnime = null;

        [SerializeField]
        private AnimeController moveAnime = null;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new PlayerLogic(this);
        }

        /// <summary>
        /// 見た目更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="level"></param>
        /// <param name="isFatigue"></param>
        public void SwitchView(GameConst.PlayerIndex index, int level, bool isFatigue)
        {
            Sprite sprite = null;
            if (isFatigue) sprite = PlayerSpriteManager.Instance().GetFatigueSprite(index, level);
            else sprite = PlayerSpriteManager.Instance().GetSprite(index, level);
            if (sprite != null) this.spriteRenderer.sprite = sprite;
        }

        /// <summary>
        /// 見た目変化
        /// </summary>
        /// <param name="level"></param>
        public void ChangeView(GameConst.PlayerIndex index, int level, bool isFatigue)
        {
            SwitchView(index, level, isFatigue);
            this.changeAnime.PlayAnime("Change", true, "Change", () =>
            {
                this.changeAnime.SetBool("Change", false);
            });
        }

        /// <summary>
        /// 減少エフェクト
        /// </summary>
        public void SubtractEffect()
        {
            this.downAnime.PlayAnime("Down", true, "Down", () =>
            {
                this.downAnime.SetBool("Down", false);
            });
        }

        /// <summary>
        /// 増加エフェクト
        /// </summary>
        public void AddEffect()
        {
            this.upAnime.PlayAnime("Up", true, "Up", () =>
            {
                this.upAnime.SetBool("Up", false);
            });
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        public void StartMove()
        {
            this.moveAnime.SetBool("IsMove", true);
        }

        /// <summary>
        /// 移動終了
        /// </summary>
        public void StopMove()
        {
            this.moveAnime.SetBool("IsMove", false);
        }
    }
}
