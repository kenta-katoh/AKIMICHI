using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEngine.Rendering.DebugUI;

namespace Akimichi.Game
{
    public class PlayerStatusView : MonoBehaviour
    {
        [SerializeField]
        private PlayerStatus player1st = null;

        [SerializeField]
        private PlayerStatus player2nd = null;

        [SerializeField]
        private PlayerStatus player3rd = null;

        [SerializeField]
        private PlayerStatus player4th = null;

        private void Awake()
        {
            this.player1st.ForceSetWeight(GameConst.PlayerIndex.First, PlayerConst.InitWeight);
            this.player2nd.ForceSetWeight(GameConst.PlayerIndex.Second, PlayerConst.InitWeight);
            this.player3rd.ForceSetWeight(GameConst.PlayerIndex.Third, PlayerConst.InitWeight);
            this.player4th.ForceSetWeight(GameConst.PlayerIndex.Fourth, PlayerConst.InitWeight);
        }

        /// <summary>
        /// 名前設定
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        public void SetName(GameConst.PlayerIndex index, string name)
        {
            switch(index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.SetName(index, name);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.SetName(index, name);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.SetName(index, name);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.SetName(index, name);
                    break;
            }
        }

        /// <summary>
        /// 体重増加
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void AddWeight(GameConst.PlayerIndex index, int value)
        {
            switch (index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.AddWeight(value);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.AddWeight(value);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.AddWeight(value);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.AddWeight(value);
                    break;
            }
        }

        /// <summary>
        /// 体重減少
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SubtractWeight(GameConst.PlayerIndex index, int value)
        {
            switch (index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.SubtractWeight(value);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.SubtractWeight(value);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.SubtractWeight(value);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.SubtractWeight(value);
                    break;
            }
        }

        /// <summary>
        /// 疲労開始
        /// </summary>
        /// <param name="index"></param>
        public void HoldFatigue(GameConst.PlayerIndex index)
        {
            switch (index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.VisibleFatigue(true);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.VisibleFatigue(true);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.VisibleFatigue(true);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.VisibleFatigue(true);
                    break;
            }
        }

        /// <summary>
        /// 疲労終了
        /// </summary>
        /// <param name="index"></param>
        public void ReleaseFatigue(GameConst.PlayerIndex index)
        {
            switch (index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.VisibleFatigue(false);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.VisibleFatigue(false);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.VisibleFatigue(false);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.VisibleFatigue(false);
                    break;
            }
        }

        /// <summary>
        /// 体重取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetWeight(GameConst.PlayerIndex index)
        {
            int weight = 0;
            switch (index)
            {
                case GameConst.PlayerIndex.First:
                    weight = this.player1st.GetWeight();
                    break;
                case GameConst.PlayerIndex.Second:
                    weight = this.player2nd.GetWeight();
                    break;
                case GameConst.PlayerIndex.Third:
                    weight = this.player3rd.GetWeight();
                    break;
                case GameConst.PlayerIndex.Fourth:
                    weight = this.player4th.GetWeight();
                    break;
            }
            return weight;
        }

        /// <summary>
        /// 体重表示切り替え
        /// </summary>
        /// <param name="flag"></param>
        public void VisibleWeight(bool flag)
        {
            this.player1st.VisibleWeight(flag);
            this.player2nd.VisibleWeight(flag);
            this.player3rd.VisibleWeight(flag);
            this.player4th.VisibleWeight(flag);
        }

        /// <summary>
        /// リザルトデータ受け渡し
        /// </summary>
        public void SendResultData()
        {
            this.player1st.SendResultData();
            this.player2nd.SendResultData();
            this.player3rd.SendResultData();
            this.player4th.SendResultData();
        }
    }
}
