using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerLoupe : MonoBehaviour
    {
        [SerializeField]
        private RectTransform canvas = null;

        [SerializeField]
        private GameObject player1stObject = null;

        [SerializeField]
        private GameObject player2ndObject = null;

        [SerializeField]
        private GameObject player3rdObject = null;

        [SerializeField]
        private GameObject player4thObject = null;

        private Vector2 canvasSize = Vector2.zero;
        private float canvasRot = 0.0f;
        private float canvasUpperRot = 0.0f;
        private float canvasScale = 0.4f;
        private Vector2 pos = new Vector2(0.0f, 0.0f);

        private void Awake()
        {
            this.canvasSize = this.canvas.sizeDelta;
            this.canvasRot = Mathf.Abs(Mathf.Atan2(this.canvasSize.y, this.canvasSize.x) * Mathf.Rad2Deg);
            this.canvasUpperRot = (90.0f - this.canvasRot) * 2.0f + this.canvasRot;
        }

        public void UpdateLoupe(GameConst.PlayerIndex index, float length, float rot)
        {
            float abs = Mathf.Abs(rot);
            float sign = Mathf.Sign(rot);
            var angleRAdian = rot / 180.0f * Mathf.PI;
            if (abs <= this.canvasRot)
            {
                float y = (Mathf.Tan(angleRAdian) * this.canvasSize.x * this.canvasScale);
                this.pos.Set(this.canvasSize.x * this.canvasScale, y);
                ((RectTransform)this.player1stObject.transform).localPosition = this.pos;
            }
            else if(this.canvasRot < abs && abs <= this.canvasUpperRot)
            {
                float x = this.canvasSize.y * this.canvasScale / Mathf.Tan(angleRAdian);
                this.pos.Set(x * sign, this.canvasSize.y * this.canvasScale * sign);
                ((RectTransform)this.player1stObject.transform).localPosition = this.pos;
            }
            else
            {
                float y = (Mathf.Tan(angleRAdian) * this.canvasSize.x * this.canvasScale);
                this.pos.Set(this.canvasSize.x * this.canvasScale * -1.0f, y * -1.0f);
                ((RectTransform)this.player1stObject.transform).localPosition = this.pos;
            }
        }
    }
}
