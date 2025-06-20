using System;
using System.Collections.Generic;

namespace Akimichi.Game
{
    public class GameStateManager : ManagerBase<GameStateManager>
    {
        private Dictionary<GameConst.GameProgressState, List<GameConst.PlayerIndex>> stateDic = new Dictionary<GameConst.GameProgressState, List<GameConst.PlayerIndex>>();
        private GameConst.GameProgressState currentState = GameConst.GameProgressState.None;
        private GameProgressManager progressManager = null;

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.progressManager = ((GameStateManagerData)data).ProgressManager;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.stateDic.Clear();
            this.progressManager = null;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.currentState = GameConst.GameProgressState.Initialize;
            this.stateDic.Clear();
            foreach(var state in Enum.GetValues(typeof(GameConst.GameProgressState)))
            {
                List<GameConst.PlayerIndex> list = new List<GameConst.PlayerIndex>();
                this.stateDic.Add((GameConst.GameProgressState)state, list);
            }
        }

        /// <summary>
        /// ステータス送信
        /// </summary>
        /// <param name="state"></param>
        public void SendState(GameConst.GameProgressState state)
        {
            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)state;
            send.Datas[1] = (int)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.FinishState, send);
        }

        /// <summary>
        /// ステータス完了
        /// </summary>
        /// <param name="state"></param>
        /// <param name="index"></param>
        public void CompleteState(GameConst.GameProgressState state, GameConst.PlayerIndex index)
        {
            if(this.stateDic.ContainsKey(state))
            {
                if(!this.stateDic[state].Contains(index))
                {
                    this.stateDic[state].Add(index);
                }
            }

            // 全員のステータスが完了しているかチェック
            if (IsCompleteState(state))
            {
                // 次のステータスに遷移
                TransitionState();

                // ステータス思考
                this.progressManager.StatusBehavior();
            }
        }

        /// <summary>
        /// 該当ステータスが全員菅慮しているか
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsCompleteState(GameConst.GameProgressState state)
        {
            bool result = false;
            if(this.stateDic.ContainsKey(state))
            {
                if(this.stateDic[state].Count == GameConst.MaximumPlayers(true))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// ステータス遷移
        /// </summary>
        public void TransitionState()
        {
            int index = (int)this.currentState;
            index++;

            if(index < Enum.GetValues(typeof(GameConst.GameProgressState)).Length)
            {
                this.currentState = (GameConst.GameProgressState)index;
            }
        }

        /// <summary>
        /// 現在ステータス
        /// </summary>
        /// <returns></returns>
        public GameConst.GameProgressState CurrentState()
        {
            return this.currentState;
        }
    }
}
