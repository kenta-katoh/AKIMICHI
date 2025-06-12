using Akimichi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventManager : ManagerBase<EventManager>
    {
        private int eventId = 0;
        private List<EventDataBase> eventDataBases = new List<EventDataBase>();
        private List<EventAnime> eventAnime = new List<EventAnime>();

        public class EventAnime
        {
            public int EventId { get; set; } = -1;
            public bool IsUse { get; set; } = false;
            public AnimeController AnimeController { get; set; } = null;
        }

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.eventAnime.Clear();
            foreach(GameObject obj in ((EventManagerData)data).Events)
            {
                EventAnime anime = new EventAnime();
                anime.AnimeController = obj.GetComponent<AnimeController>();
                this.eventAnime.Add(anime);
            }
        }

        private void AddEvent(EventDataBase eventData)
        {
            bool isEvent = true;
            foreach(EventDataBase item in this.eventDataBases)
            {
                if(item.EventID == eventData.EventID)
                {
                    isEvent = false;
                    break;
                }
            }

            if(isEvent) this.eventDataBases.Add(eventData);
        }

        // 使用していない稽古アニメ取得
        private EventAnime GetEventAnime()
        {
            EventAnime result = null;
            foreach(EventAnime item in this.eventAnime)
            {
                if (!item.IsUse)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 稽古イベント予約
        /// </summary>
        public EventDataBase EventBooking(Transform mapSpace)
        {
            PracticeEventData eventData = new PracticeEventData(this.eventId);
            AddEvent(eventData);

            EventAnime anime = GetEventAnime();
            if(anime != null)
            {
                anime.EventId = eventData.EventID;
                anime.IsUse = true;
                anime.AnimeController.gameObject.transform.localPosition = mapSpace.localPosition;
            }
            this.eventId++;
            return eventData;
        }

        /// <summary>
        /// 稽古開始送信
        /// </summary>
        /// <param name="data"></param>
        public void SendPracticeBegins(PracticeEventData data)
        {
            ClearSendData();
            this.datas[0] = data.EventID;
            int index = 1;
            foreach (GameConst.PlayerIndex item in data.PracticePlayer)
            {
                this.datas[index] = (int)item;
                index++;
            }
            NetworkManager.Instance().SendEvent(EventConst.Event.PracticeBegins, this.datas);
        }

        /// <summary>
        /// 稽古イベント開始
        /// </summary>
        /// <param name="id"></param>
        public void PracticeBegins(int id)
        {
            foreach(EventAnime anime in this.eventAnime)
            {
                if(anime.EventId == id)
                {
                    anime.AnimeController.PlayAnime("IsEvent", true);
                }
            }
        }
    }
}
