
using static Akimichi.SoundConst;

namespace Akimichi
{
    public class AudioManager : ManagerBase<AudioManager>
    {
        private SoundController soundController;
        private SoundConst.BGM currentBgm = SoundConst.BGM.None;

        /// <summary>
        /// コントローラー設定
        /// </summary>
        /// <param name="soundController"></param>
        public void SetController(SoundController soundController)
        {
            this.soundController = soundController;
            this.currentBgm = SoundConst.BGM.None;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="bgm"></param>
        public void PlayBGM(SoundConst.BGM bgm, bool isReturn = false, bool isLoop = true)
        {
            if(this.soundController != null)
            {
                if(this.currentBgm != bgm)
                {
                    this.currentBgm = bgm;
                    this.soundController.PlayBGM(this.currentBgm, isReturn, isLoop);
                }
            }
        }

        /// <summary>
        /// 共通
        /// </summary>
        /// <param name="se"></param>
        public void PlaySE(SoundConst.SE se)
        {
            if(this.soundController != null)
            {
                this.soundController.PlaySE(se);
            }
        }

        /// <summary>
        /// マッチング
        /// </summary>
        /// <param name="se"></param>
        public void PlaySE(SoundConst.MATCHING se)
        {
            if (this.soundController != null)
            {
                this.soundController.PlaySE(se);
            }
        }

        /// <summary>
        /// インゲーム
        /// </summary>
        /// <param name="se"></param>
        public void PlaySE(SoundConst.GAME se)
        {
            if (this.soundController != null)
            {
                this.soundController.PlaySE(se);
            }
        }

        /// <summary>
        /// リザルト
        /// </summary>
        /// <param name="se"></param>
        public void PlaySE(SoundConst.RESULT se)
        {
            if (this.soundController != null)
            {
                this.soundController.PlaySE(se);
            }
        }

        /// <summary>
        /// SEをフェードで消す
        /// </summary>
        public void FadeSE()
        {
            if (this.soundController != null)
            {
                this.soundController.FadeSE();
            }
        }
    }
}
