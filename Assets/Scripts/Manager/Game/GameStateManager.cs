using System;
using System.Collections.Generic;
using TMPro;

namespace Akimichi.Game
{
    public class GameStateManager : ManagerBase<GameStateManager>
    {
        private Dictionary<GameConst.GameProgressState, List<GameConst.PlayerIndex>> stateDic = new Dictionary<GameConst.GameProgressState, List<GameConst.PlayerIndex>>();
        private GameConst.GameProgressState currentState = GameConst.GameProgressState.None;
        private GameProgressManager progressManager = null;
        private double serverTime = 0;
        private double delayTime = 0;
        private int leftTime = 0;
        private TextMeshProUGUI timer = null;
        private bool isLast = false;
        private bool isFinish = false;

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.progressManager = ((GameStateManagerData)data).ProgressManager;
            this.timer = ((GameStateManagerData)data).Timer;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.stateDic.Clear();
            this.progressManager = null;
            this.serverTime = 0;
            this.delayTime = 0;
            this.leftTime = 0;
            this.timer = null;
            this.isLast = false;
            this.isFinish = false;
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

            var t = TimeSpan.FromSeconds(GameConst.GameTime);
            this.timer.text = (int)t.TotalMinutes + ":" + t.Seconds.ToString("00");
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

        /// <summary>
        /// サーバー時間設定
        /// </summary>
        public void SetServerTime()
        {
            this.serverTime = 0;
            this.leftTime = 0;
            this.serverTime = NetworkManager.Instance().GetServerTime();
            this.delayTime = NetworkManager.Instance().GetPhotonTime() - this.serverTime;
            this.isLast = false;
        }

        public override void ManagedUpdate()
        {
            base.ManagedUpdate();
            if(this.currentState == GameConst.GameProgressState.InGame)
            {
                if (this.isFinish) return;
                this.leftTime = GameConst.GameTime - (int)(NetworkManager.Instance().GetPhotonTime() - this.delayTime - this.serverTime);
                var t = TimeSpan.FromSeconds(this.leftTime);
                this.timer.text = (int)t.TotalMinutes + ":" + t.Seconds.ToString("00");

                if (!this.isLast && this.leftTime < GameConst.LastTime)
                {
                    this.isLast = true;
                    PlayerManager.Instance().VisibleWeight(false);
                }

                if(!this.isFinish && this.leftTime < 150)
                {
                    this.isFinish = true;
                    this.progressManager.FinishGame();
                    SendState(GameConst.GameProgressState.FinishGame);
                }
            }
        }
    }
}
