using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    /// <summary>
    /// ホストだけが所持するイベント思考
    /// </summary>
    public class EventBrain
    {
        private Dictionary<int, List<GameConst.PlayerIndex>> mapAffiliationDic = new Dictionary<int, List<GameConst.PlayerIndex>>();

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
    }
}
