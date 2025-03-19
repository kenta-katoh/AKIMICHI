using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Akimichi.Game
{
    public static class GameConst
    {
        public static int MaximumPlayers()
        {
            return Enum.GetNames(typeof(PlayerIndex)).Length;
        }

        public enum PlayerIndex
        {
            First = 0,
            Second = 1,
            Third = 2,
            Fourth = 3,
        }

        public enum MapSpaceType
        {
            None,
            Plus,
            Minus,
            Event,
        }
    }
}
