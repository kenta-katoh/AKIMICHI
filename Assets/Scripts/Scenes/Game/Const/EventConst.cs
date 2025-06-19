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
            FinishState,
            AffiliationMapSpace,    // マス所属
            WaitingPractice,           // イベント待機
            PracticePossible,          // イベント可能
            StartPractice,             // イベント開始
            PracticeBegins,         // 稽古開始
            PracticeEffectStart,       // 稽古エフェクト再生
            EndPractice,           // イベント終了

            SetName,                // 名前設定
            AddWeight,              // 体重増加
            SubtractWeight,         // 体重減少

            CreatePlayerObject,
            StartingPositionDistribution,
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

        public enum EventMessageType
        {
            None,
            Main,
            Yes,
            No,
        }

        public static readonly string MessageLastChar = "F";    // メッセージの末尾を示す文字
        public static readonly int FontWaitFrame = 5;   // 文字送りフレーム
    }
}
