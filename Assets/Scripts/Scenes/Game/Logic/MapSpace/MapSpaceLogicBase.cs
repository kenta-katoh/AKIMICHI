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
    }
}
