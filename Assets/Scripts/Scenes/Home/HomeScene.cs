using UnityEngine;

namespace Akimichi
{
    public class HomeScene : MonoBehaviour
    {
        [SerializeField]
        private AkimichiPhotonManager photonManager = null;

        private void Awake()
        {
            NetworkManager.Instance().Disconnect();
            TransitionManager.Instance().AddScene(SceneConst.Home);
            if (TransitionManager.Instance().IsFirstTransedScene(SceneConst.Home))
            {
                DontDestroyOnLoad(photonManager);
            }
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
