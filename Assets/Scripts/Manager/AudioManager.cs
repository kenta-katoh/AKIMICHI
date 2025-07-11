
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
        public void PlayBGM(SoundConst.BGM bgm)
        {
            if(this.soundController != null)
            {
                if(this.currentBgm != bgm)
                {
                    this.currentBgm = bgm;
                    this.soundController.PlayBGM(this.currentBgm);
                }
            }
        }
    }
}
