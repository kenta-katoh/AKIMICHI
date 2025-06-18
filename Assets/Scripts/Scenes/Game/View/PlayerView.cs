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
            }
        }
    }
}
