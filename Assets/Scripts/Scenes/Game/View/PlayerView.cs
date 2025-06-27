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
        private List<Sprite> sprites = new List<Sprite>();

        [SerializeField]
        private AnimeController statusAnime = null;

        [SerializeField]
        private AnimeController changeAnime = null;

        [SerializeField]
        private AnimeController moveAnime = null;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new PlayerLogic(this);
            this.spriteRenderer.sprite = this.sprites[0];
        }

        /// <summary>
        /// 見た目更新
        /// </summary>
        /// <param name="level"></param>
        public void ChangeView(int level)
        {
            if(level < this.sprites.Count)
            {
                this.spriteRenderer.sprite = this.sprites[level];
                this.changeAnime.PlayAnime("Change", true, "Change", () =>
                {
                    this.changeAnime.SetBool("Change", false);
                });
            }
        }

        /// <summary>
        /// 減少エフェクト
        /// </summary>
        public void SubtractEffect()
        {
            this.statusAnime.PlayAnime("Down", true, "Down", () =>
            {
                this.statusAnime.SetBool("Down", false);
            });
        }

        /// <summary>
        /// 増加エフェクト
        /// </summary>
        public void AddEffect()
        {
            this.statusAnime.PlayAnime("Up", true, "Up", () =>
            {
                this.statusAnime.SetBool("Up", false);
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
