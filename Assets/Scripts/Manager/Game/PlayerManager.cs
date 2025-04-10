using System;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        public GameConst.PlayerIndex PlayerIndex {  get; private set; }
        private PlayerLogic playerLogic = null;
        public PlayerConst.State State { get; private set; } = PlayerConst.State.None;
        private PlayerConst.Direction direction = PlayerConst.Direction.None;
        private MapSpaceViewBase nextSpace = null;

        // player math
        private float rotRange = 0.0f;

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
                this.playerLogic = (PlayerLogic)view.Logic;
            }
        }

        public override void ManagedUpdate()
        {
            base.ManagedUpdate();
            if(this.State == PlayerConst.State.OnMove)
            {
                this.playerLogic.AddRotation(this.rotRange);
                if(Mathf.Sign(this.rotRange) > 0.0f)
                {
                    if (this.playerLogic.IsCheckRotationExceed())
                    {
                        this.playerLogic.SetTargetRotation(-PlayerConst.MaximumRot);
                        this.rotRange = -PlayerConst.RotRange;
                    }
                }
                else
                {
                    if (this.playerLogic.IsCheckRotationBelow())
                    {
                        this.playerLogic.SetTargetRotation(PlayerConst.MaximumRot);
                        this.rotRange = PlayerConst.RotRange;
                    }
                }
            }
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        public void EnterGame()
        {
            SetPlayerState(PlayerConst.State.WaitingInput);
        }

        /// <summary>
        /// ダイス振り
        /// </summary>
        public void DiceRoll()
        {
            SetPlayerState(PlayerConst.State.DuringDice);
        }

        private void SetPlayerState(PlayerConst.State state)
        {
            this.State = state;
        }

        /// <summary>
        /// プレイヤー移動
        /// </summary>
        public void MovePlayer()
        {
            this.State = PlayerConst.State.OnMove;
            this.playerLogic.SetTargetRotation(PlayerConst.MaximumRot);
            this.rotRange = PlayerConst.RotRange;
        }

        /// <summary>
        /// プレイヤーtransform取得
        /// </summary>
        /// <returns></returns>
        public Transform GetPlayerTransform()
        {
            return this.playerLogic.GetTransform();
        }

        /// <summary>
        /// 位置設定（即時同期）
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosInstantSync(Vector3 pos)
        {
            this.playerLogic.SetPosInstantSync(pos);
        }

        /// <summary>
        /// 進行方向設定
        /// </summary>
        /// <param name="dir"></param>
        public void SetDirection(PlayerConst.Direction dir)
        {
            this.direction = dir;
        }
    }
}
