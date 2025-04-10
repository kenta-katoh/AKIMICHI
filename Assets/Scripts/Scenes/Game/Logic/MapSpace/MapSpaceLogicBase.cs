using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapSpaceLogicBase : LogicBase
    {
        private GameConst.MapSpaceType spaceType = GameConst.MapSpaceType.None;
        public GameConst.MapSpaceType MapSpaceType { get { return spaceType; } }
        public int Index { get; private set; } = 0;
        private List<GameConst.PlayerIndex> affiliatedPlayers = new List<GameConst.PlayerIndex>();  // 自マスに属しているプレイヤー

        public MapSpaceLogicBase(GameConst.MapSpaceType type, MapSpaceViewBase view) : base(view)
        { 
            this.spaceType = type;
        }

        /// <summary>
        /// マスインデックス
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            this.Index = index;
        }

        /// <summary>
        /// スタート位置設定されているか
        /// </summary>
        /// <returns></returns>
        public bool IsStartingPosition()
        {
            return ((MapSpaceViewBase)this.view).IsStartingPosition;
        }

        /// <summary>
        /// マスに所属
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Affiliation(GameConst.PlayerIndex index)
        {
            bool result = false;
            if(!this.affiliatedPlayers.Contains(index))
            {
                this.affiliatedPlayers.Add(index);
            }

            if(this.affiliatedPlayers.Count > 1)
            {
                // 属しているプレイヤーが二人以上の場合にイベントが発生する
                result = true;
            }
            return result;
        }

        /// <summary>
        /// マスから離属
        /// </summary>
        /// <param name="index"></param>
        public void Separation(GameConst.PlayerIndex index)
        {
            if (this.affiliatedPlayers.Contains(index))
            {
                this.affiliatedPlayers.Remove(index);
            }
        }
    }
}
