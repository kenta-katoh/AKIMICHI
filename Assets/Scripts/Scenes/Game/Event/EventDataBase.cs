using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventDataBase
    {
        public int EventID { get; private set; } = -1;
        public EventConst.MapEventState MapEventState { get; protected set; } = EventConst.MapEventState.None;

        public EventDataBase(int eventID)
        {
            this.EventID = eventID;
        }
    }
}
