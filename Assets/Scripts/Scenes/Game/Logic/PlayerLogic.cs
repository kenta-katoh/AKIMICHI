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

        public PlayerLogic(ViewBase view) : base(view)
        {
        }

        public override void OnManagedUpdate()
        {
            base.OnManagedUpdate();
            if (this.isMove)
            {
                AddRotation(this.rotRange);
                if (Mathf.Sign(this.rotRange) > 0.0f)
                {
                    if (this.targetRotation < this.rotation)
                    {
                        SetTargetRotation(-PlayerConst.MaximumRot, -PlayerConst.RotRange);
                    }
                }
                else
                {
                    if (this.targetRotation > this.rotation)
                    {
                        SetTargetRotation(PlayerConst.MaximumRot, PlayerConst.RotRange);
                    }
                }
            }
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        /// <param name="target"></param>
        public void StartMove(Transform target)
        {
            this.isMove = true;
            this.targetTransform = target;
            this.view.transform.DOLocalMove(target.localPosition, PlayerConst.MoveTime).OnComplete(() => 
            {
                PlayerManager.Instance().MoveBehavior();

            });
            SetTargetRotation(PlayerConst.MaximumRot, PlayerConst.RotRange);
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
            SyncRotView();
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
        /// すり足回転
        /// </summary>
        /// <param name="value"></param>
        private void AddRotation(float value)
        {
            this.rotation += value;
            int sign = (int)Mathf.Sign(this.rotation);
            float rot = Mathf.Abs(this.rotation) % 360.0f;
            rot *= sign;
            this.rotation = rot;
            SyncRotView();
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
        /// logic側の回転に対してview側の同期を行う
        /// </summary>
        private void SyncRotView()
        {
            this.view.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, this.rotation);
        }
    }
}
