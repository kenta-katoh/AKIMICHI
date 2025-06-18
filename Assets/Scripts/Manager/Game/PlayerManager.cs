using System;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : ManagerBase<PlayerManager>
    {
        public GameConst.PlayerIndex PlayerIndex {  get; private set; }
        private PlayerLogic playerLogic = null;
        public PlayerConst.State State { get; private set; } = PlayerConst.State.None;
        private PlayerConst.State returnedState = PlayerConst.State.None;
        private PlayerConst.Direction direction = PlayerConst.Direction.None;
        private MapSpaceLogicBase currentMapSpace = null;
        public EventConst.Practice PracticeState { get; private set; } = EventConst.Practice.None;

        public override void Initialize()
        {
            base.Initialize();
            PlayerIndex = NetworkManager.Instance().GetPlayerIndex(NetworkManager.Instance().GetUserID());
        }

        /// <summary>
        /// 名前取得
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return NetworkManager.Instance().GetName(NetworkManager.Instance().GetUserID());
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
            
            // イベント関連の判定
            if(this.PracticeState != EventConst.Practice.Waiting)
            {
                // まだダイス目が残っているかの判断
                if (DiceManager.Instance().IsDiceRest())
                {
                    StartMove();
                }
                else
                {
                    // 止まったマス目の思考
                    switch(this.currentMapSpace.MapSpaceType)
                    {
                        case GameConst.MapSpaceType.Plus:
                            ClearSendData();
                            this.datas[0] = (int)this.PlayerIndex;
                            this.datas[1] = EventManager.Instance().GetPlusValue();
                            NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, this.datas);
                            break;
                        case GameConst.MapSpaceType.Minus:
                            ClearSendData();
                            this.datas[0] = (int)this.PlayerIndex;
                            this.datas[1] = EventManager.Instance().GetMinusValue();
                            NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, this.datas);
                            break;
                        case GameConst.MapSpaceType.Event:
                            break;
                    }

                    this.playerLogic.StopMove(this.currentMapSpace.GetTransform());
                    SetPlayerState(PlayerConst.State.WaitingInput);
                }
            }
            else if(this.PracticeState == EventConst.Practice.Waiting)
            {
                // viewが追いついたので稽古可能状態に遷移
                this.PracticeState = EventConst.Practice.ReadyToGo;
                SendEventPossible();
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
                // 1マス進んでいるのでデクリメント
                DiceManager.Instance().DiceDecrement();

                // logic側ではすでに所属マスが変わっているので通知
                MapManager.Instance().SendAffiliation(this.currentMapSpace.Index, DiceManager.Instance().DiceValue);

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
        /// イベント待機状態へ
        /// </summary>
        public void WaitingEvent()
        {
            this.PracticeState = EventConst.Practice.Waiting;
            this.returnedState = this.State;    // イベントから復帰時に戻るステータス
            DiceManager.Instance().ForceStop();
            switch(this.State)
            {
                // Viewがすでに追いついている場合は即時イベント可能状態に
                case PlayerConst.State.WaitingInput:
                case PlayerConst.State.DuringDice:
                    SendEventPossible();
                    break;
            }
            SetPlayerState(PlayerConst.State.Event);
        }

        // 稽古可能状態の送信
        private void SendEventPossible()
        {
            this.PracticeState = EventConst.Practice.ReadyToGo;
            this.returnedState = this.State;    // イベントから復帰時に戻るステータス
            SetPlayerState(PlayerConst.State.Event);
            DiceManager.Instance().ForceStop();

            ClearSendData();
            this.datas[0] = (int)this.PlayerIndex;
            this.datas[1] = EventManager.Instance().GetEvent();
            NetworkManager.Instance().SendEvent(EventConst.Event.PracticePossible, this.datas);
        }

        /// <summary>
        /// ふたたびイベント待機状態へ
        /// </summary>
        public void SendReEventPossible()
        {
            this.PracticeState = EventConst.Practice.ReadyToGo;
            SetPlayerState(PlayerConst.State.Event);
            DiceManager.Instance().ForceStop();

            ClearSendData();
            this.datas[0] = (int)this.PlayerIndex;
            this.datas[1] = EventManager.Instance().GetEvent();
            NetworkManager.Instance().SendEvent(EventConst.Event.PracticePossible, this.datas);
        }

        /// <summary>
        /// イベント開始
        /// </summary>
        public void StartEvent()
        {
            if(this.PracticeState == EventConst.Practice.ReadyToGo)
            {
                this.PracticeState = EventConst.Practice.DuringPractice;
                this.playerLogic.StopMove(this.currentMapSpace.GetTransform());
                SetPlayerState(PlayerConst.State.Event);
            }
        }

        /// <summary>
        /// イベントから解放
        /// </summary>
        public void ReleaseEvent()
        {
            if (this.PracticeState == EventConst.Practice.DuringPractice)
            {
                this.PracticeState = EventConst.Practice.None;
                SetPlayerState(this.returnedState);
                switch(this.State)
                {
                    case PlayerConst.State.WaitingInput:
                    case PlayerConst.State.DuringDice:
                    case PlayerConst.State.OnMove:
                        SetPlayerState(PlayerConst.State.WaitingInput);
                        break;
                    case PlayerConst.State.MoveBehavior:
                        MoveBehavior();
                        break;
                }
            }
        }
    }
}
