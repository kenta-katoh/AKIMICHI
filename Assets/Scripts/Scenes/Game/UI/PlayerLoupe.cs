using System;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi.Game
{
    public class PlayerLoupe : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup = null;

        [SerializeField]
        private Image loupeImage = null;

        private Vector2 canvasSize = Vector2.zero;
        private float canvasRot = 0.0f;
        private float canvasUpperRot = 0.0f;
        private float canvasScale = 0.4f;
        private Vector2 pos = new Vector2(0.0f, 0.0f);
        private RectTransform rectTransform = null;

        private void Awake()
        {
            this.rectTransform = (RectTransform)transform;
        }

        /// <summary>
        /// キャンバス設定
        /// </summary>
        /// <param name="size"></param>
        public void SetCanvas(Vector2 size)
        {
            this.canvasSize = size;
            this.canvasRot = Mathf.Abs(Mathf.Atan2(this.canvasSize.y, this.canvasSize.x) * Mathf.Rad2Deg);
            this.canvasUpperRot = (90.0f - this.canvasRot) * 2.0f + this.canvasRot;
        }

        public void UpdateLoupe(float length, float rot)
        {
            bool math = false;
            math = 10.0f <= length && length <= 15.0f;
            this.canvasGroup.alpha = Convert.ToInt32(math);
            if (!math) return;

            float abs = Mathf.Abs(rot);
            float sign = Mathf.Sign(rot);
            var angleRAdian = rot / 180.0f * Mathf.PI;
            if (abs <= this.canvasRot)
            {
                float y = (Mathf.Tan(angleRAdian) * this.canvasSize.x * this.canvasScale);
                this.pos.Set(this.canvasSize.x * this.canvasScale, y);
                this.rectTransform.localPosition = this.pos;
            }
            else if(this.canvasRot < abs && abs <= this.canvasUpperRot)
            {
                float x = this.canvasSize.y * this.canvasScale / Mathf.Tan(angleRAdian);
                this.pos.Set(x * sign, this.canvasSize.y * this.canvasScale * sign);
                this.rectTransform.localPosition = this.pos;
            }
            else
            {
                float y = (Mathf.Tan(angleRAdian) * this.canvasSize.x * this.canvasScale);
                this.pos.Set(this.canvasSize.x * this.canvasScale * -1.0f, y * -1.0f);
                this.rectTransform.localPosition = this.pos;
            }
        }
    }
}
