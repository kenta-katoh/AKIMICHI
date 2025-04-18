using Akimichi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventManager : ManagerBase<EventManager>
    {
        public EventConst.Practice Practice { get; private set; } = EventConst.Practice.None;

        /// <summary>
        /// マスに所属を送信
        /// </summary>
        /// <param name="index"></param>
        /// <param name="playerIndex"></param>
        public void SendAffiliation(int index)
        {
            ClearSendData();
            this.datas[0] = index;
            this.datas[1] = (int)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.AffiliationMapSpace, this.datas);
        }

        /// <summary>
        /// 稽古待機状態へ
        /// </summary>
        public void WaitingPractice()
        {
            this.Practice = EventConst.Practice.Waiting;
        }

        /// <summary>
        /// 準備完了
        /// </summary>
        public void ReadyToGo()
        {
            this.Practice = EventConst.Practice.ReadyToGo;
        }
    }
}
