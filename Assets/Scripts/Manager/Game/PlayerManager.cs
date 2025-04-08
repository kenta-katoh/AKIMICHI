using System;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        public GameConst.PlayerIndex PlayerIndex {  get; private set; }
        public PlayerView PlayerView { get; private set; } = null;
        public PlayerConst.State State { get; private set; } = PlayerConst.State.None;
        private MapSpaceViewBase nextSpace = null;

        // 一旦サイコロ処理はこちらに
        private int dice = 0;
        private System.Random rand = new System.Random();

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

        public override void ManagedUpdate()
        {
            base.ManagedUpdate();
            if(this.State == PlayerConst.State.OnMove)
            {

            }
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        public void EnterGame()
        {
            this.State = PlayerConst.State.WaitingInput;
        }

        /// <summary>
        /// ダイスロール
        /// </summary>
        public void RollDice()
        {
            this.State = PlayerConst.State.DuringDice;

            // 一旦サイコロ処理はここで
            this.dice = rand.Next(1, 7);
            Debug.LogError(this.dice);
            MovePlayer();
        }

        /// <summary>
        /// プレイヤー移動
        /// </summary>
        public void MovePlayer()
        {
            this.State = PlayerConst.State.OnMove;
        }
    }
}
