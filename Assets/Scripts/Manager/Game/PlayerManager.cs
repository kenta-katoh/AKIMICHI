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
        private MapSpaceLogicBase currentMapSpace = null;

        public override void Initialize()
        {
            base.Initialize();
            PlayerIndex = NetworkManager.Instance().GetPlayerIndex(NetworkManager.Instance().GetUserID());
        }

        /// <summary>
        /// 生成後に自身の取得
        /// </summary>
        /// <param name="obj"></param>
        public void SetPlayerData(GameObject obj)
        {
            PlayerView view = obj.GetComponent<PlayerView>();
            if(view != null)
            {
                this.playerLogic = (PlayerLogic)view.Logic;
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

        /// <summary>
        /// 移動思考中
        /// </summary>
        public void MoveBehavior()
        {
            SetPlayerState(PlayerConst.State.MoveBehavior);
            
            // 1マス移動を完了して思考時間なので、稽古が強制発生する場合はここかも

            // まだダイス目が残っているかの判断
            if(DiceManager.Instance().IsDiceRest())
            {
                StartMove();
            }
            else
            {
                SetPlayerState(PlayerConst.State.WaitingInput);
                this.playerLogic.StopMove(this.currentMapSpace.GetTransform());
            }
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        public void StartMove()
        {
            if(this.direction != PlayerConst.Direction.None && DiceManager.Instance().IsDiceRest())
            {
                SetPlayerState(PlayerConst.State.OnMove);
                switch(this.direction)
                {
                    case PlayerConst.Direction.ClockWise:
                        this.currentMapSpace = MapManager.Instance().NextMapSpace(this.currentMapSpace);
                        break;
                    case PlayerConst.Direction.CounterClockWise:
                        this.currentMapSpace = MapManager.Instance().PreviousMapSpace(this.currentMapSpace);
                        break;
                }
                // logic側ではすでに所属マスが変わっているので通知
                MapManager.Instance().SendAffiliation(this.currentMapSpace.Index);

                // 1マス進んでいるのでデクリメント
                DiceManager.Instance().DiceDecrement();

                // view側の移動
                this.playerLogic.StartMove(this.currentMapSpace.GetTransform());
            }
        }

        private void SetPlayerState(PlayerConst.State state)
        {
            this.State = state;
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

        /// <summary>
        /// 所属マスの設定
        /// </summary>
        /// <param name="logic"></param>
        public void SetMapSpace(MapSpaceLogicBase logic)
        {
            this.currentMapSpace = logic;
        }
    }
}
