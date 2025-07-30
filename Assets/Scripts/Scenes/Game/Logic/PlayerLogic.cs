using UnityEngine;
using DG.Tweening;

namespace Akimichi.Game
{
    public class PlayerLogic : LogicBase
    {
        private PlayerView playerView = null;

        public PlayerLogic(ViewBase view) : base(view)
        {
            this.playerView = (PlayerView)view;
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        /// <param name="target"></param>
        public void StartMove(Transform target)
        {
            this.view.transform.DOLocalMove(target.localPosition, PlayerConst.MoveTime).
                SetEase(Ease.Linear).
                OnComplete(() => 
            {
                PlayerManager.Instance().MoveBehavior();

            });
            this.playerView.StartMove();

            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.StartMove, send);
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        public void StartMove()
        {
            this.playerView.StartMove();
        }

        /// <summary>
        /// 移動停止
        /// </summary>
        /// <param name="target"></param>
        public void StopMove(Transform target)
        {
            this.view.transform.DOKill();
            SetPosInstantSync(target.localPosition);
            this.playerView.StopMove();

            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.EndMove, send);
        }

        /// <summary>
        /// 移動停止
        /// </summary>
        public void StopMove()
        {
            this.playerView.StopMove();
        }

        /// <summary>
        /// 位置設定（即時同期）
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosInstantSync(Vector3 pos)
        {
            this.view.transform.localPosition = pos;
        }

        /// <summary>
        /// 見た目更新
        /// </summary>
        /// <param name="level"></param>
        public void ChangeView(GameConst.PlayerIndex index, int level)
        {
            this.playerView.ChangeView(index, level);
        }

        /// <summary>
        /// 減少エフェクト
        /// </summary>
        public void SubtractEffect()
        {
            this.playerView.SubtractEffect();
        }

        /// <summary>
        /// 増加エフェクト
        /// </summary>
        public void AddEffect()
        {
            this.playerView.AddEffect();
        }

        /// <summary>
        /// ヒエラルキーを下位に移動
        /// </summary>
        public void SetAsLast()
        {
            this.playerView.transform.SetAsLastSibling();
        }
    }
}
