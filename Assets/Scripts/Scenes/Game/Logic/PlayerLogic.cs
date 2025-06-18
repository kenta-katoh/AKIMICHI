using UnityEngine;
using DG.Tweening;
using static Cinemachine.DocumentationSortingAttribute;

namespace Akimichi.Game
{
    public class PlayerLogic : LogicBase
    {
        private PlayerView playerView = null;
        private Vector3 startRotation = new Vector3(0.0f, 0.0f, 15.0f);
        private Vector3 loopRotation = new Vector3(0.0f, 0.0f, -15.0f);

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

            this.view.transform.DOLocalRotate(this.startRotation, 0.25f).
                SetEase(Ease.Linear).
                OnComplete(() => 
            {
                this.view.transform.DOLocalRotate(this.loopRotation, 0.5f).
                SetEase(Ease.Linear).
                SetLoops(-1, LoopType.Yoyo);
            });
        }

        /// <summary>
        /// 移動停止
        /// </summary>
        /// <param name="target"></param>
        public void StopMove(Transform target)
        {
            this.view.transform.DOKill();
            SetPosInstantSync(target.localPosition);
            this.view.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
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
        public void ChangeView(int level)
        {
            this.playerView.ChangeView(level);
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
