using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Akimichi.Game
{
    public class PlayerLogic : LogicBase
    {
        private bool isMove = false;
        private float targetRotation = 0.0f;
        private float rotation = 0.0f;
        private float rotRange = 0.0f;
        private Transform targetTransform = null;
        private Vector3 startRotation = new Vector3(0.0f, 0.0f, 15.0f);
        private Vector3 loopRotation = new Vector3(0.0f, 0.0f, -15.0f);

        public PlayerLogic(ViewBase view) : base(view)
        {
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        /// <param name="target"></param>
        public void StartMove(Transform target)
        {
            this.targetTransform = target;
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
            this.isMove = false;
            this.view.transform.DOKill();
            SetPosInstantSync(target.localPosition);

            this.targetRotation = 0.0f;
            this.rotRange = 0.0f;
            this.rotation = 0.0f;
            this.view.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// 回転設定
        /// </summary>
        /// <param name="rot"></param>
        private void SetTargetRotation(float target, float range)
        {
            this.targetRotation = target;
            this.rotRange = range;
        }

        /// <summary>
        /// 位置設定（即時同期）
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosInstantSync(Vector3 pos)
        {
            this.view.transform.localPosition = pos;
        }
    }
}
