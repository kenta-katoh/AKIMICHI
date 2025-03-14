using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapSpaceLogicBase : LogicBase
    {
        private GameConst.MapSpaceType spaceType = GameConst.MapSpaceType.None;
        public GameConst.MapSpaceType MapSpaceType { get { return spaceType; } }

        public MapSpaceLogicBase(GameConst.MapSpaceType type, MapSpaceViewBase view) : base(view)
        { 
            this.spaceType = type;
        }
    }
}
