using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Akimichi.Game.GameConst;

namespace Akimichi.Game
{
    /// <summary>
    /// ホストだけが所持するイベント思考
    /// </summary>
    public class EventBrain
    {
        private Dictionary<int, List<GameConst.PlayerIndex>> mapAffiliationDic = new Dictionary<int, List<GameConst.PlayerIndex>>();
        private Dictionary<EventDataBase, List<GameConst.PlayerIndex>> mapEventDic = new Dictionary<EventDataBase, List<PlayerIndex>>();

        public void Initialize(int mapSpaces)
        {
            this.mapAffiliationDic.Clear();
            for (int i = 0; i < mapSpaces; ++i)
            {
                this.mapAffiliationDic.Add(i, new List<GameConst.PlayerIndex>());
            }
        }

        /// <summary>
        /// マス目所属
        /// </summary>
        /// <param name="index"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public List<GameConst.PlayerIndex> AffiliationMapSpace(int index, GameConst.PlayerIndex player)
        {
            List<GameConst.PlayerIndex> result = new List<GameConst.PlayerIndex>();
            // 離属
            foreach(var dic in this.mapAffiliationDic)
            {
                if(dic.Value.Contains(player))
                {
                    dic.Value.Remove(player);
                }
            }

            // 所属
            foreach (var dic in this.mapAffiliationDic)
            {
                if (dic.Key == index)
                {
                    if (!dic.Value.Contains(player))
                    {
                        dic.Value.Add(player);
                        result = dic.Value;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 稽古イベントの監視登録
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="players"></param>
        public void AddPracticeEvent(EventDataBase mapEvent, List<GameConst.PlayerIndex> players)
        {
            bool isEvent = true;
            foreach (var item in this.mapEventDic)
            {
                if(item.Key.EventID == mapEvent.EventID)
                {
                    isEvent = false;
                    break;
                }
            }

            if(isEvent)
            {
                List <GameConst.PlayerIndex> list = new List<GameConst.PlayerIndex>(players);
                ((PracticeEventData)mapEvent).PracticePlayer.AddRange(list);
                this.mapEventDic.Add(mapEvent, list);
            }
        }

        /// <summary>
        /// プレイヤーの稽古開始可能を受けて、該当プレイヤーがそろったかどうか
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EventDataBase PracticeStartCheck(GameConst.PlayerIndex index)
        {
            EventDataBase result = null;
            foreach(var item in this.mapEventDic)
            {
                if(item.Key.GetType() == typeof(PracticeEventData))
                {
                    if(item.Value.Contains(index))
                    {
                        item.Value.Remove(index);
                        // 事前に登録された開始の監視対象がそろった
                        if(item.Value.Count == 0)
                        {
                            result = item.Key;
                            this.mapEventDic.Remove(result);
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
