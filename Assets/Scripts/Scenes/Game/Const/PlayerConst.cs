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
        }

        public enum Direction
        {
            None,
            ClockWise,              // 時計まわり
            CounterClockWise,       // 反時計回り
        }

        public static readonly float MaximumRot = 15.0f;
        public static readonly float RotRange = 0.25f;
    }
}
