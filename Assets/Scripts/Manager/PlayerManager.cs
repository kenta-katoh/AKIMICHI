using Cinemachine;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        public GameConst.PlayerIndex PlayerIndex {  get; private set; }
        public PlayerView PlayerView { get; private set; } = null;

        public override void Initialize()
        {
            base.Initialize();
            PlayerIndex = NetworkManager.Instance().GetPlayerIndex(NetworkManager.Instance().GetUserID());
        }

        public void SetPlayerData(GameObject obj)
        {
            PlayerView view = obj.GetComponent<PlayerView>();
            if(view != null)
            {
                this.PlayerView = view;
            }
        }
    }
}
