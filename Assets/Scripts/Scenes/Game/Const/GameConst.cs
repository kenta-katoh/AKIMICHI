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
            StartPositionSetting,
            InitializedFinish,
            BootCamera,
            InGame,
            FinishGame,
        }

        public static readonly int GameTime = 180;     // 制限時間（秒）
        public static readonly int LastTime = 60;     // 最終時間（秒）
    }
}
