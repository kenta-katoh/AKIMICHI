using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventView : ViewBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new EventLogic(this);
        }
    }
}
