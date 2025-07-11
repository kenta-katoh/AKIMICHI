using UnityEngine;

namespace Akimichi
{
    public class TitleScene : MonoBehaviour
    {
        private void Awake()
        {
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Main);
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
        }

        public void ChangeScene()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
