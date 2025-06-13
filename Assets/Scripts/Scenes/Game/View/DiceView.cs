using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Akimichi.Game
{
    public class DiceView : ViewBase
    {
        [Header("ダイス回転時間(秒)")]
        [SerializeField]
        private float diceRollTime = 1.0f;

        [SerializeField]
        private TextMeshProUGUI dice = null;
        private bool isRoll = false;
        private float rollTime = 10.0f;
        private System.Random rand = new System.Random();
        private bool isForceStop = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.dice.text = "0";
        }

        public void MoveLeft()
        {
            DiceRoll(PlayerConst.Direction.ClockWise);
        }

        public void MoveRight()
        {
            DiceRoll(PlayerConst.Direction.CounterClockWise);
        }

        private void DiceRoll(PlayerConst.Direction dir)
        {
            bool isSuccess = DiceManager.Instance().DiceRoll();
            if(isSuccess)
            {
                PlayerManager.Instance().DiceRoll();

                // ダイス成功時に進行方向設定
                PlayerManager.Instance().SetDirection(dir);
                this.isRoll = true;
                this.rollTime = this.diceRollTime;
                this.isForceStop = false;
            }
        }

        private void Update()
        {
            if(this.isRoll)
            {
                if(!this.isForceStop)
                {
                    this.rollTime -= Time.deltaTime;
                    this.dice.text = this.rand.Next(1, 7).ToString();
                    if (this.rollTime < 0.0f)
                    {
                        this.dice.text = DiceManager.Instance().DiceValue.ToString();
                        this.isRoll = false;
                        PlayerManager.Instance().StartMove();
                    }
                }
                else
                {
                    // ロール中に強制停止
                    PlayerManager.Instance().SetDirection(PlayerConst.Direction.None);
                    this.isRoll = false;
                    this.rollTime = this.diceRollTime;
                }
            }
        }

        /// <summary>
        /// イベントが発生したので強制停止
        /// </summary>
        public void ForceStop()
        {
            this.isForceStop = true;
        }
    }
}
