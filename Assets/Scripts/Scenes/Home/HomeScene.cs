using UnityEngine;

namespace Akimichi
{
    public class HomeScene : MonoBehaviour
    {
        

        private void Awake()
        {
            NetworkManager.Instance().Disconnect();
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Main);
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
        }

        public void ChangeLobby()
        {
            TransitionManager.Instance().Transition(SceneConst.Lobby);
        }

        public void ChangeTutorial()
        {
            TransitionManager.Instance().Transition(SceneConst.Tutorial);
        }
    }
}
