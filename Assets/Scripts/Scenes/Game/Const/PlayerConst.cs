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
    }
}
