using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PracticeEventData : EventDataBase
    {
        public List<GameConst.PlayerIndex> PracticePlayer { get; set; } = new List<GameConst.PlayerIndex>();

        public PracticeEventData(int id) : base(id)
        {
            this.MapEventState = EventConst.MapEventState.Waiting;
        }
    }
}
