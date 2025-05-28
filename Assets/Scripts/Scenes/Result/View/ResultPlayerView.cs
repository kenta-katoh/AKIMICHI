using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akimichi.Game;

namespace Akimichi.Result
{
    public class ResultPlayerView : ViewBase
    {
        public enum PlayerSprite
        { 
            Player1_00 = 0,
            Player1_01,
            Player1_02,
            Player1_03,
            Player1_04,
            Player2_00,
            Player2_01,
            Player2_02,
            Player2_03,
            Player2_04,
            Player3_00,
            Player3_01,
            Player3_02,
            Player3_03,
            Player3_04,
            Player4_00,
            Player4_01,
            Player4_02,
            Player4_03,
            Player4_04,
        };

        public List<Sprite> playerSprites = new List<Sprite>();
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new ResultPlayerLogic(this);
            ChangePayerSprite(PlayerSprite.Player1_00);
        }

        /// <summary>
        /// プレイヤー見た目変更
        /// </summary>
        /// <param name="playerSpriteIndex"></param>
        void ChangePayerSprite(PlayerSprite playerSpriteIndex)
        {
            var sp = GetComponent<SpriteRenderer>();
            sp.sprite = this.playerSprites[(int)playerSpriteIndex];
        }
    }
}

