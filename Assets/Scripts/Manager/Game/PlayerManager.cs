using System;
using System.Collections.Generic;
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
        private Dictionary<GameConst.PlayerIndex, PlayerLogic> playerDic = new Dictionary<GameConst.PlayerIndex, PlayerLogic>();
        private PlayerStatusView playerStatusView = null;
        private bool isfatigue = false;

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            PlayerManagerData managerData = (PlayerManagerData)data;
            this.playerStatusView = managerData.StatusView;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.playerLogic = null;
            this.State = PlayerConst.State.None;
            this.returnedState = PlayerConst.State.None;
            this.direction = PlayerConst.Direction.None;
            this.currentMapSpace = null;
            this.PracticeState = EventConst.Practice.None;
            this.playerDic.Clear();
            this.playerStatusView = null;
            this.isfatigue = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.PlayerIndex = NetworkManager.Instance().GetPlayerIndex(NetworkManager.Instance().GetUserID());
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
            ResistPlayer(this.PlayerIndex, this.playerLogic);
        }

        /// <summary>
        /// プレイヤー登録
        /// </summary>
        /// <param name="index"></param>
        /// <param name="logic"></param>
        public void ResistPlayer(GameConst.PlayerIndex index, PlayerLogic logic)
        {
            if(!this.playerDic.ContainsKey(index))
            {
                this.playerDic.Add(index, logic);
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
                            this.isfatigue = false; // 疲労回復
                            var send2 = DataObjectManager.Instance().Get();
                            send2.Datas[0] = (int)this.PlayerIndex;
                            NetworkManager.Instance().SendEvent(EventConst.Event.ReleaseFatigue, send2);

                            var send = DataObjectManager.Instance().Get();
                            send.Datas[0] = (int)this.PlayerIndex;
                            send.Datas[1] = EventManager.Instance().GetPlusValue();
                            NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, send);
                            break;
                        case GameConst.MapSpaceType.Minus:
                            var send1 = DataObjectManager.Instance().Get();
                            send1.Datas[0] = (int)this.PlayerIndex;
                            send1.Datas[1] = EventManager.Instance().GetMinusValue();
                            NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, send1);
                            break;
                        case GameConst.MapSpaceType.Event:
                            EventManager.Instance().MapEventStart();
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
                MapManager.Instance().SendAffiliation(this.currentMapSpace.Index);

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

            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)this.PlayerIndex;
            send.Datas[1] = EventManager.Instance().GetEvent();
            NetworkManager.Instance().SendEvent(EventConst.Event.PracticePossible, send);
        }

        /// <summary>
        /// 稽古開始
        /// </summary>
        public void StartPractice()
        {
            if(this.PracticeState == EventConst.Practice.ReadyToGo)
            {
                this.PracticeState = EventConst.Practice.DuringPractice;
                this.playerLogic.StopMove(this.currentMapSpace.GetTransform());
                SetPlayerState(PlayerConst.State.Event);
            }
        }

        /// <summary>
        /// 稽古ステータス計算
        /// </summary>
        public void CalcPractice()
        {
            int weight = this.playerStatusView.GetWeight(this.PlayerIndex);
            int result = 0;
            if(!this.isfatigue)
            {
                result = (int)(weight * 0.1f);
            }
            else
            {
                result = (int)(weight * 0.15f);
            }
            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)this.PlayerIndex;
            send.Datas[1] = result;
            NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, send);
        }

        /// <summary>
        /// 稽古から解放
        /// </summary>
        public void ReleasePractice()
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

                // 疲労状態へ
                this.isfatigue = true;
                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = (int)this.PlayerIndex;
                NetworkManager.Instance().SendEvent(EventConst.Event.HoldFatigue, send);
            }
        }

        /// <summary>
        /// 名前設定
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        public void SetName(GameConst.PlayerIndex index, string name)
        {
            this.playerStatusView.SetName(index, name);
        }

        /// <summary>
        /// 体重増加
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void AddWeight(GameConst.PlayerIndex index, int value)
        {
            this.playerStatusView.AddWeight(index, value);
            if (this.playerDic.ContainsKey(index))
            {
                this.playerDic[index].AddEffect();
            }
        }

        /// <summary>
        /// 体重減少
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SubtractWeight(GameConst.PlayerIndex index, int value)
        {
            this.playerStatusView.SubtractWeight(index, value);
            if (this.playerDic.ContainsKey(index))
            {
                this.playerDic[index].SubtractEffect();
            }
        }

        /// <summary>
        /// プレイヤーの表示更新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="level"></param>
        public void ChangePlayerView(GameConst.PlayerIndex index, int level)
        {
            if(this.playerDic.ContainsKey(index))
            {
                this.playerDic[index].ChangeView(level);
            }
        }

        /// <summary>
        /// 疲労開始
        /// </summary>
        /// <param name="index"></param>
        public void HoldFatigue(GameConst.PlayerIndex index)
        {
            this.playerStatusView.HoldFatigue(index);
        }

        /// <summary>
        /// 疲労終了
        /// </summary>
        /// <param name="index"></param>
        public void ReleaseFatigue(GameConst.PlayerIndex index)
        {
            this.playerStatusView.ReleaseFatigue(index);
        }

        /// <summary>
        /// 体重の表示切り替え
        /// </summary>
        public void VisibleWeight(bool flag)
        {
            this.playerStatusView.VisibleWeight(flag);
        }
    }
}
