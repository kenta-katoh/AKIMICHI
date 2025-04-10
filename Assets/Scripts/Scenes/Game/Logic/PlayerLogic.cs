using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerLogic : LogicBase
    {
        private Vector3 logicPosition = Vector3.zero;
        private float targetRotation = 0.0f;
        private float rotation = 0.0f;

        public PlayerLogic(ViewBase view) : base(view)
        {
        }

        /// <summary>
        /// 回転設定
        /// </summary>
        /// <param name="rot"></param>
        public void SetTargetRotation(float rot)
        {
            this.targetRotation = rot;
        }

        /// <summary>
        /// すり足回転
        /// </summary>
        /// <param name="value"></param>
        public void AddRotation(float value)
        {
            this.rotation += value;
            int sign = (int)Mathf.Sign(this.rotation);
            float rot = Mathf.Abs(this.rotation) % 360.0f;
            rot *= sign;
            this.rotation = rot;
            SyncRotView();
        }

        /// <summary>
        /// 回転角チェック
        /// </summary>
        /// <returns></returns>
        public bool IsCheckRotationExceed()
        {
            return this.targetRotation < this.rotation;
        }

        /// <summary>
        /// 回転角チェック
        /// </summary>
        /// <returns></returns>
        public bool IsCheckRotationBelow()
        {
            return this.targetRotation > this.rotation;
        }

        /// <summary>
        /// 位置設定（即時同期）
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosInstantSync(Vector3 pos)
        {
            this.logicPosition = pos;
            SyncPosView();
        }

        /// <summary>
        /// logic側の位置に対してview側の同期を行う
        /// </summary>
        private void SyncPosView()
        {
            this.view.transform.localPosition = this.logicPosition;
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
