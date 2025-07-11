using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class SoundConst
    {
        //public static readonly int GameTime = 180;     // 制限時間（秒）

        public enum BGM
        {
            None,
            Main,
            Matching,
            Game,
            Event,
            Result,
            Winner,
        }

        public enum SE
        {
            Decide,
            Back,
            Trans,
        }

        public enum MATCHING
        {
            Ready,
            TransGame,
        }

        public enum GAME
        {
            Count1,
            Count2,
            Count3,
            DiceRoll,
            DiceDecide,
            Move,
            MoveStop,
            WeightUp,
            WeightDown,
            Change,
            Practice,
            Text,
        }

        public enum RESULT
        {
            Entry,
            Practice,
            Fall,
            Winner,
            Applause,
            Data,
        }
    }
}
