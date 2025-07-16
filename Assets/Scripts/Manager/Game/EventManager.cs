using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventManager : ManagerBase<EventManager>
    {
        private List<int> eventList = new List<int>();
        private List<EventLogic> eventLogics = new List<EventLogic>();
        private System.Random seed = new System.Random();
        private System.Random rate = new System.Random();
        private EventWindow eventWindow = null;
        private System.Random eventSeed = new System.Random();

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.eventLogics.Clear();
            foreach(GameObject obj in ((EventManagerData)data).Events)
            {
                this.eventLogics.Add((EventLogic)obj.GetComponent<EventView>().Logic);
            }
            this.eventWindow = ((EventManagerData)data).EventWindow;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.eventList.Clear();
            this.eventLogics.Clear();
            this.eventWindow = null;
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
        public void StartEventEffect(MapSpaceLogicBase space, Action action)
        {
            EventLogic eventLogic = GetEventAnime();
            if(eventLogic != null)
            {
                eventLogic.GetTransform().localScale = new Vector3(0.3f, 0.3f, 1.0f);
                eventLogic.GetTransform().localPosition = new Vector3(  space.GetTransform().localPosition.x,
                                                                        space.GetTransform().localPosition.y + 1.0f,
                                                                        -2.0f) ;
                eventLogic.AnimeController.PlayAnime("IsEvent", true, "Practice", () =>
                {
                    eventLogic.AnimeController.SetBool("IsEvent", false);
                    action?.Invoke();
                });
            }
            else
            {
                // 進行不能になると困るので一応の回避仕込み
                action?.Invoke();
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

        // シード取得
        private int Seed(int value)
        {
            return this.seed.Next(0, value);
        }

        /// <summary>
        /// 増加マスでの増加量取得
        /// </summary>
        /// <returns></returns>
        public int GetPlusValue()
        {
            int result = 0;
            int seed = Seed(20);
            switch (seed)
            {
                case 0:
                    result = 100;
                    break;
                case 1:
                    result = rate.Next(61, 80);
                    break;
                case 2:
                case 3:
                    result = rate.Next(41, 60);
                    break;
                case 4:
                case 5:
                    result = rate.Next(10, 20);
                    break;
                default:
                    result = rate.Next(21, 40);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 減少マスでの減少量取得
        /// </summary>
        /// <returns></returns>
        public int GetMinusValue()
        {
            int result = 0;
            int seed = Seed(10);
            switch (seed)
            {
                case 0:
                    result = rate.Next(15, 21);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    result = rate.Next(10, 16);
                    break;
                default:
                    result = rate.Next(5, 11);
                    break;
            }
            return result;
        }

        /// <summary>
        /// マップイベント開始
        /// </summary>
        public void MapEventStart()
        {
            EventDataBase eventData = null;
            int seed = this.eventSeed.Next(0, 7);
            switch(seed)
            {
                case 0:
                    eventData = new EventData01();
                    break;
                case 1:
                    eventData = new EventData02();
                    break;
                case 2:
                    eventData = new EventData03();
                    break;
                case 3:
                    eventData = new EventData04();
                    break;
                case 4:
                    eventData = new EventData05();
                    break;
                case 5:
                    eventData = new EventData06();
                    break;
                case 6:
                    eventData = new EventData07();
                    break;
            }
            this.eventWindow.StartEvent(eventData);
        }
    }
}
