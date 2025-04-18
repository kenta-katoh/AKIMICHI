using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventConst
    {
        public enum Event
        {
            // 定期イベント
            None = 0,
            FinishState = 1,
            AffiliationMapSpace = 2,
            WaitingPractice = 3,

            CreatePlayerObject = 100,
            StartingPositionDistribution = 101,
        }

        public static Event ConvertEvent(byte data)
        {
            Event result = Event.None;
            result = (Event)((int)data);
            return result;
        }

        public static byte ConvertEvent(Event data)
        {
            byte result = 0;
            result = (byte)((int)data);
            return result;
        }

        // 稽古状態
        public enum Practice
        {
            None,
            Waiting,        // 稽古待機
            ReadyToGo,      // 稽古開始準備完了
            DuringPractice, // 稽古中
        }
    }
}
