using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class DiceManager : ManagerBase<DiceManager>
    {
        public int DiceValue { get; private set; } = 0;
        private System.Random rand = new System.Random();
        private DiceView diceView = null;

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.diceView = ((DiceManagerData)data).DiceView;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.diceView = null;
        }

        /// <summary>
        /// ダイス振り
        /// </summary>
        public bool DiceRoll()
        {
            if (PlayerManager.Instance().State == PlayerConst.State.WaitingInput && 
                PlayerManager.Instance().PracticeState == EventConst.Practice.None)
            {
                this.DiceValue = this.rand.Next(1, 7);
                if(DebugManager.Instance().IsDebug)
                {
                    int value = DebugManager.Instance().OverwriteDice();
                    if(0 < value && value < 7)
                    {
                        this.DiceValue = value;
                    }
                }

                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
                send.Datas[1] = (byte)EventConst.ResultData.DiceCount;
                NetworkManager.Instance().SendEvent(EventConst.Event.ResultData, send);

                var send1 = DataObjectManager.Instance().Get();
                send1.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
                send1.Datas[1] = (byte)EventConst.ResultData.MoveValue;
                send1.Datas[2] = (byte)this.DiceValue;
                NetworkManager.Instance().SendEvent(EventConst.Event.ResultData, send1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 1進んだ
        /// </summary>
        public void DiceDecrement()
        {
            if (this.DiceValue > 0)
            {
                this.DiceValue--;
            }
        }

        /// <summary>
        /// ダイス目が残っているかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsDiceRest()
        {
            return this.DiceValue > 0;
        }

        /// <summary>
        /// イベントが発生したので強制停止
        /// </summary>
        public void ForceStop()
        {
            this.diceView.ForceStop();
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        public void UpdateView()
        {
            if(this.DiceValue > 0)
            {
                this.diceView.gameObject.SetActive(true);
                this.diceView.UpdateView(this.DiceValue);
            }
            else
            {
                this.diceView.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// on off
        /// </summary>
        /// <param name="visible"></param>
        public void Visible(bool visible)
        {
            if (visible && this.DiceValue < 1) return;
            this.diceView.gameObject.SetActive(visible);
        }
    }
}
