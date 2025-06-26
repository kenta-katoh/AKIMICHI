using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Akimichi.Game
{
    public class ResultDataManager : ManagerBase<ResultDataManager>
    {
        private Dictionary<GameConst.PlayerIndex, PlayerData> resultDic = new Dictionary<GameConst.PlayerIndex, PlayerData>();
        private bool isInitialize = false;

        public override void Initialize()
        {
            base.Initialize();
            this.resultDic.Clear();
            this.isInitialize = true;
            foreach (var item in Enum.GetValues(typeof(GameConst.PlayerIndex)))
            {
                PlayerData data = new PlayerData();
                this.resultDic.Add((GameConst.PlayerIndex)item, data);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.resultDic.Clear();
        }

        /// <summary>
        /// プレイヤー情報の設定
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void SetPlayerData(GameConst.PlayerIndex index, PlayerData data)
        {
            this.resultDic[index].Weight = data.Weight;
        }

        /// <summary>
        /// プレイヤー情報取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PlayerData GetPlayerData(GameConst.PlayerIndex index)
        {
            if(!this.isInitialize)
            {
                this.isInitialize= true;
                foreach (var item in Enum.GetValues(typeof(GameConst.PlayerIndex)))
                {
                    PlayerData data = new PlayerData();
                    this.resultDic.Add((GameConst.PlayerIndex)item, data);
                }
            }
            return this.resultDic[index];
        }
    }
}
