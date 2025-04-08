using System;

namespace Akimichi.Game
{
    public static class GameConst
    {
        public static int MaximumPlayers(bool isUseDabug = false)
        {
            int result = Enum.GetNames(typeof(PlayerIndex)).Length;
            if (isUseDabug)
            {
                if (NetworkManager.Instance().GetRoomPlayerValue() < Enum.GetNames(typeof(PlayerIndex)).Length)
                {
                    result = NetworkManager.Instance().GetRoomPlayerValue();
                }
            }
            return result;
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

        public enum GameProgressState
        {
            None,
            Initialize,
            CreatedPlayerObject,
            InitializedFinish,
            BootCamera,
            InGame,
        }
    }
}
