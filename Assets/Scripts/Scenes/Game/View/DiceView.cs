using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi.Game
{
    public class DiceView : ViewBase
    {
        [SerializeField]
        private List<Sprite> sprites = new List<Sprite>();

        [SerializeField]
        private Image diceImage = null;

        [SerializeField]
        private AnimeController animeController = null;
        private string key = string.Empty;

        protected override void OnAwake()
        {
            base.OnAwake();
            this.diceImage.sprite = this.sprites[0];
            this.gameObject.SetActive(false);
        }

        public void MoveLeft()
        {
            DiceRoll(PlayerConst.Direction.CounterClockWise);
        }

        public void MoveRight()
        {
            DiceRoll(PlayerConst.Direction.ClockWise);
        }

        private void DiceRoll(PlayerConst.Direction dir)
        {
            bool isSuccess = DiceManager.Instance().DiceRoll();
            if(isSuccess)
            {
                AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
                PlayerManager.Instance().DiceRoll();

                // ダイス成功時に進行方向設定
                PlayerManager.Instance().SetDirection(dir);

                this.animeController.gameObject.SetActive(true);
                this.key = "Result" + DiceManager.Instance().DiceValue;
                this.animeController.PlayAnime(this.key, true, this.key, () => 
                {
                    this.gameObject.SetActive(true);
                    this.animeController.SetBool(this.key, false);
                    this.diceImage.sprite = this.sprites[DiceManager.Instance().DiceValue - 1];
                    PlayerManager.Instance().StartMove();
                });
            }
        }

        /// <summary>
        /// イベントが発生したので強制停止
        /// </summary>
        public void ForceStop()
        {
            this.animeController.gameObject.SetActive(false);
            this.animeController.DeleteAction();
            this.animeController.SetBool("Result1", false);
            this.animeController.SetBool("Result2", false);
            this.animeController.SetBool("Result3", false);
            this.animeController.SetBool("Result4", false);
            this.animeController.SetBool("Result5", false);
            this.animeController.SetBool("Result6", false);
            this.animeController.ForcePlay("Idle");
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="value"></param>
        public void UpdateView(int value)
        {
            this.diceImage.sprite = this.sprites[value - 1];
        }

        private void Update()
        {
        }
    }
}
