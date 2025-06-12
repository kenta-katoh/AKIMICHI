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
        private EventConst.Practice practiceState = EventConst.Practice.None;

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
            
            // 稽古関連の判定
            if(this.practiceState != EventConst.Practice.Waiting)
            {
                // まだダイス目が残っているかの判断
                if (DiceManager.Instance().IsDiceRest())
                {
                    StartMove();
                }
                else
                {
                    SetPlayerState(PlayerConst.State.WaitingInput);
                    this.playerLogic.StopMove(this.currentMapSpace.GetTransform());
                }
            }
            else if(this.practiceState == EventConst.Practice.Waiting)
            {
                // viewが追いついたので稽古可能状態に遷移
                this.practiceState = EventConst.Practice.ReadyToGo;
                SendPracticePossible();
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

        /// <summary>
        /// 稽古待機状態へ
        /// </summary>
        public void WaitingPractice()
        {
            this.practiceState = EventConst.Practice.Waiting;
            switch(this.State)
            {
                // 入力待機 or ダイス中の場合は即時稽古可能状態に
                case PlayerConst.State.WaitingInput:
                case PlayerConst.State.DuringDice:
                    this.practiceState = EventConst.Practice.ReadyToGo;
                    SendPracticePossible();
                    break;
            }
        }

        // 稽古可能状態の送信
        private void SendPracticePossible()
        {
            ClearSendData();
            this.datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.PracticePossible, this.datas);
        }

        /// <summary>
        /// 稽古開始
        /// </summary>
        public void PracticeBegins()
        {
            if(this.practiceState == EventConst.Practice.ReadyToGo)
            {
                this.practiceState = EventConst.Practice.DuringPractice;
            }
        }
    }
}
