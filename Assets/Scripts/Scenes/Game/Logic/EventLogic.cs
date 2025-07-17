using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventLogic : LogicBase
    {
        public AnimeController AnimeController { get; private set; } = null;

        public EventLogic(ViewBase view) : base(view)
        {
            this.AnimeController = this.view.GetComponent<AnimeController>();
        }
    }
}
