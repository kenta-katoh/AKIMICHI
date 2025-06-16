using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Akimichi.Game
{
    /// <summary>
    /// ホストだけが所持するイベント思考
    /// </summary>
    public class EventBrain
    {
        private int eventId = 1;
        private Dictionary<int, List<GameConst.PlayerIndex>> mapAffiliationDic = new Dictionary<int, List<GameConst.PlayerIndex>>();
        private List<MapEventBase> mapEventBases = new List<MapEventBase>();
        private Dictionary<int, List<GameConst.PlayerIndex>> mapEventObserver = new Dictionary<int, List<GameConst.PlayerIndex>>();

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
        /// イベント作成
        /// </summary>
        public int CreateEvent(EventConst.MapEventType type, int mapSpaceIndex, GameConst.PlayerIndex player)
        {
            this.eventId++;
            MapEventBase mapEvent = new MapEventBase(this.eventId, type, mapSpaceIndex);
            mapEvent.AddPlayer(player);
            this.mapEventBases.Add(mapEvent);
            AddEventObserver(mapEvent);

            return this.eventId;
        }

        /// <summary>
        /// イベント作成
        /// </summary>
        public int CreateEvent(EventConst.MapEventType type, int mapSpaceIndex, List<GameConst.PlayerIndex> players)
        {
            this.eventId++;
            MapEventBase mapEvent = new MapEventBase(this.eventId, type, mapSpaceIndex);
            foreach (var item in players)
            {
                mapEvent.AddPlayer(item);
            }
            this.mapEventBases.Add(mapEvent);
            AddEventObserver(mapEvent);

            return this.eventId;
        }

        // 稽古イベントの監視登録
        private void AddEventObserver(MapEventBase mapEvent)
        {
            if(!this.mapEventObserver.ContainsKey(mapEvent.EventID))
            {
                this.mapEventObserver.Add(mapEvent.EventID, new List<GameConst.PlayerIndex>(mapEvent.Players));
            }
        }

        /// <summary>
        /// イベント取得 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MapEventBase GetEvent(int id)
        {
            MapEventBase result = null;
            foreach (var item in this.mapEventBases)
            {
                if (item.EventID == id)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// プレイヤーの稽古開始可能を受けて、該当プレイヤーがそろったかどうか
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MapEventBase EventStartCheck(int id, GameConst.PlayerIndex index)
        {
            MapEventBase result = null;
            if(this.mapEventObserver.ContainsKey(id))
            {
                if (this.mapEventObserver[id].Contains(index))
                {
                    this.mapEventObserver[id].Remove(index);
                    // 事前に登録された開始の監視対象がそろった
                    if (this.mapEventObserver[id].Count == 0)
                    {
                        result = GetEvent(id);
                        this.mapEventObserver.Remove(id);
                    }
                }
            }
            return result;
        }
    }
}
