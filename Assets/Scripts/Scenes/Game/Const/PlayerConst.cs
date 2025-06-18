using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerConst
    {
        public enum State
        {
            None,
            WaitingInput,    // 入力待機
            DuringDice,      // サイコロ中
            OnMove,          // 移動中
            MoveBehavior,    // 移動思考中
            Event,          // イベント中
        }

        public enum Direction
        {
            None,
            ClockWise,              // 時計まわり
            CounterClockWise,       // 反時計回り
        }

        public static readonly float MoveTime = 1.0f;   // マス間の移動時間
        public static readonly int InitWeight = 50;     // 初期体重
    }
}
