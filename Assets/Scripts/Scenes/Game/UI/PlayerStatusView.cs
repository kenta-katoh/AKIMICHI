using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            this.player1st.ForceSetWeight(PlayerConst.InitWeight);
            this.player2nd.ForceSetWeight(PlayerConst.InitWeight);
            this.player3rd.ForceSetWeight(PlayerConst.InitWeight);
            this.player4th.ForceSetWeight(PlayerConst.InitWeight);
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
                    this.player1st.SetName(name);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.SetName(name);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.SetName(name);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.SetName(name);
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
    }
}
