using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MinusMapSpaceView : MapSpaceViewBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new MinusMapSpaceLogic(this);
        }
    }
}
