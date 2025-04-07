using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventMapSpaceView : MapSpaceViewBase
    {
        [SerializeField]
        private int EventID = 0;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new EventMapSpaceLogic(this);
        }
    }
}
