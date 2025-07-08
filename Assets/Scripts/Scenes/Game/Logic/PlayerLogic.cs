using UnityEngine;
using DG.Tweening;
using static Cinemachine.DocumentationSortingAttribute;

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
    }
}
