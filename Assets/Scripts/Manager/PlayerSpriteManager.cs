using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class PlayerSpriteManager : ManagerBase<PlayerSpriteManager>
    {
        private Dictionary<GameConst.PlayerIndex, List<Sprite>> playerSpriteDic = new Dictionary<GameConst.PlayerIndex, List<Sprite>>();

        public void SetSprite(GameConst.PlayerIndex index, List<Sprite> list)
        {
            if(!this.playerSpriteDic.ContainsKey(index))
            {
                List<Sprite> sprites = new List<Sprite>();
                sprites.AddRange(list);
                this.playerSpriteDic.Add(index, sprites);
            }
        }

        public Sprite GetSprite(GameConst.PlayerIndex index, int level)
        {
            Sprite result = null;
            if(this.playerSpriteDic.ContainsKey(index) && level < this.playerSpriteDic[index].Count)
            {
                result = this.playerSpriteDic[index][level];
            }
            return result;
        }
    }
}
