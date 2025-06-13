using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapEventBase
    {
        public int EventID { get; private set; } = -1;
        public EventConst.MapEventType EventType { get; private set; } = EventConst.MapEventType.None;
        public EventConst.MapEventState MapEventState { get; protected set; } = EventConst.MapEventState.None;
        public int MapSpaceIndex {  get; private set; } = -1;
        public List<GameConst.PlayerIndex> Players { get; private set; } = new List<GameConst.PlayerIndex>();

        public MapEventBase(int eventID, EventConst.MapEventType type, int index)
        {
            this.EventID = eventID;
            this.EventType = type;
            this.MapSpaceIndex = index;
            this.MapEventState = EventConst.MapEventState.Waiting;
        }

        /// <summary>
        /// イベント中遷移
        /// </summary>
        public void StartEvent()
        {
            this.MapEventState = EventConst.MapEventState.DuringEvent;
        }

        /// <summary>
        /// イベント参加プレイヤーの登録
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(GameConst.PlayerIndex player)
        {
            if(!this.Players.Contains(player))
            {
                this.Players.Add(player);
            }
        }

        /// <summary>
        /// イベント終了
        /// </summary>
        public void ReleaseEvent()
        {
            this.EventID = -1;
            this.EventType = EventConst.MapEventType.None;
            this.MapSpaceIndex = -1;
            this.MapEventState = EventConst.MapEventState.None;
            this.Players.Clear();
        }
    }
}
