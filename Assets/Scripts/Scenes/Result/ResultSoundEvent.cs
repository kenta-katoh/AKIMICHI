
using UnityEngine;

namespace Akimichi
{
    public class ResultSoundEvent : MonoBehaviour
    {
        public void Entry()
        {
            AudioManager.Instance().PlaySE(SoundConst.RESULT.Entry);
        }

        public void Practice()
        {
            AudioManager.Instance().PlaySE(SoundConst.RESULT.Practice);
        }

        public void Fall()
        {
            AudioManager.Instance().PlaySE(SoundConst.RESULT.Fall);
        }

        public void Winner()
        {
            AudioManager.Instance().PlaySE(SoundConst.RESULT.Winner);
        }

        public void Applause()
        {
            AudioManager.Instance().PlaySE(SoundConst.RESULT.Applause);
        }

        public void WinBGM()
        {
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Winner, false, false);
        }
    }
}
