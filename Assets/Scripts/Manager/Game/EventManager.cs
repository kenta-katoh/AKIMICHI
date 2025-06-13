using Akimichi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventManager : ManagerBase<EventManager>
    {
        private List<int> eventList = new List<int>();
        private List<EventLogic> eventLogics = new List<EventLogic>();

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.eventLogics.Clear();
            foreach(GameObject obj in ((EventManagerData)data).Events)
            {
                this.eventLogics.Add((EventLogic)obj.GetComponent<EventView>().Logic);
            }
        }

        // 使用していない稽古アニメ取得
        private EventLogic GetEventAnime()
        {
            EventLogic result = null;
            foreach(EventLogic item in this.eventLogics)
            {
                if (!item.AnimeController.IsPlaying)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 稽古イベント開始
        /// </summary>
        /// <param name="id"></param>
        public void StartEventEffect(EventConst.MapEventType type, MapSpaceLogicBase space, Action action)
        {
            EventLogic eventLogic = GetEventAnime();
            if(eventLogic != null)
            {
                eventLogic.GetTransform().localPosition = space.GetTransform().localPosition;
                eventLogic.AnimeController.PlayAnime("IsEvent", true, "EndEvent", action);
            }
        }

        /// <summary>
        /// イベント追加
        /// </summary>
        /// <param name="id"></param>
        public void AddEventId(int id)
        {
            if(!this.eventList.Contains(id))
            {
                this.eventList.Add(id);
            }
            this.eventList.Sort();  // イベントIDは加算されていくので、送信ラグも考慮して発行されたものから処理する
        }

        /// <summary>
        /// イベント終了
        /// </summary>
        /// <param name="id"></param>
        public void ReleaseEvent(int id)
        {
            if (this.eventList.Contains(id))
            {
                this.eventList.Remove(id);
            }
        }

        /// <summary>
        /// 先頭のイベントID取得
        /// </summary>
        /// <returns></returns>
        public int GetEvent()
        {
            if(this.eventList.Count > 0)
            {
                return this.eventList[0];
            }
            return -1;
        }
    }
}
