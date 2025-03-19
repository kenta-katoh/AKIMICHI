using Cinemachine;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        private GameObject playerRoot = null;
        private List<PlayerView> playerViews = new List<PlayerView>();
        private Dictionary<GameConst.PlayerIndex, PlayerData> players = new Dictionary<GameConst.PlayerIndex, PlayerData>();

        public class PlayerData
        {
            public Player Player { get; private set; }
            public PlayerView PlayerView { get; private set; }

            public PlayerData(Player player, PlayerView playerView)
            {
                Player = player;
                PlayerView = playerView;
            }
        }

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.playerRoot = ((PlayerManagerData)data).PlayerRoot;
        }

        public override void Initialize()
        {
            base.Initialize();
            GameObject obj = NetworkManager.Instance().CreateObject("Player");
            obj.transform.SetParent(this.playerRoot.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            this.playerViews.Clear();
            int childCount = this.playerRoot.transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                PlayerView player = this.playerRoot.transform.GetChild(i).GetComponent<PlayerView>();
                if (player != null)
                {
                    this.playerViews.Add(player);
                }
            }
        }

        public override void CreateData()
        {
            base.CreateData();
            for (int i = 0; i < GameConst.MaximumPlayers(); i++)
            {
                if (i < this.playerViews.Count)
                {
                    Player player = NetworkManager.Instance().GetPlayer((GameConst.PlayerIndex)i);
                    PlayerView view = this.playerViews[i];
                    PlayerData data = new PlayerData(player, view);
                    this.players.Add((GameConst.PlayerIndex)i, data);
                }
            }
        }

        /// <summary>
        /// 自身の取得
        /// </summary>
        /// <returns></returns>
        public PlayerData GetLocalPlayer()
        {
            PlayerData result = null;
            foreach(var item in this.players)
            {
                if(NetworkManager.Instance().IsMyself(item.Value.Player.UserId))
                {
                    result = item.Value;
                    break;
                }
            }
            return result;
        }
    }
}
