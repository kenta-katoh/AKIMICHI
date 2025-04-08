using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerView : MonoBehaviour
    {
        private float targetRotation = 0.0f;
        private float rotation = 0.0f;

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
            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, this.rotation);
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
    }
}
