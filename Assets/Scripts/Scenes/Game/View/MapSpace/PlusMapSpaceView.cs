using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlusMapSpaceView : MapSpaceViewBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new PlusMapSpaceLogic(this);
        }
    }
}
