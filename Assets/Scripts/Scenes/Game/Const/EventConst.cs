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
            AffiliationMapSpace = 2,    // マス所属
            WaitingEvent = 3,           // イベント待機
            EventPossible = 4,          // イベント可能
            StartEvent,
            PracticeBegins = 5,         // 稽古開始
            EventEffectStart = 6,    // 稽古エフェクト再生
            PlusEventBegins = 7,        // 増加イベント
            MinusEventBegins = 7,       // 現象イベント
            MapEventBegins = 7,         // イベント
            EventRelease = 8,           // イベント終了

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

        public enum PlayerEventState
        {
            None,
            ViewWaiting,    // View側の追いつき待ち
            ReadyToGo,      // イベント発火可能状態
            Doing,
        }

        // 稽古状態
        public enum Practice
        {
            None,
            Waiting,        // 稽古待機
            ReadyToGo,      // 稽古開始準備完了
            DuringPractice, // 稽古中
        }

        public enum MapEventState
        {
            None,
            Waiting,        // 発火待機
            DuringEvent,    // イベント中
        }

        public enum MapEventType
        {
            None,
            Practice,       // 稽古
            Plus,
            Minus,
            Event,
        }
    }
}
