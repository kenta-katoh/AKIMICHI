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

        /// <summary>
        /// ダイス振り
        /// </summary>
        /// <param name="index"></param>
        public void AddDiceCount(GameConst.PlayerIndex index)
        {
            if(this.resultDic.ContainsKey(index))
            {
                this.resultDic[index].DiceCount++;
            }
        }

        /// <summary>
        /// マス目数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        public void AddMapSpace(GameConst.PlayerIndex index, GameConst.MapSpaceType type)
        {
            if (this.resultDic.ContainsKey(index))
            {
                switch(type)
                {
                    case GameConst.MapSpaceType.Plus:
                        this.resultDic[index].PlusCount++;
                        break;
                    case GameConst.MapSpaceType.Minus:
                        this.resultDic[index].MinusCount++;
                        break;
                    case GameConst.MapSpaceType.Event:
                        this.resultDic[index].EventCount++;
                        break;
                }
            }
        }

        /// <summary>
        /// 移動量
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void AddMoveValue(GameConst.PlayerIndex index, int value)
        {
            if (this.resultDic.ContainsKey(index))
            {
                this.resultDic[index].MoveValue += value;
            }
        }

        /// <summary>
        ///  稽古数
        /// </summary>
        /// <param name="index"></param>
        public void AddPracticeCount(GameConst.PlayerIndex index)
        {
            if (this.resultDic.ContainsKey(index))
            {
                this.resultDic[index].PracticeCount++;
            }
        }
    }
}
