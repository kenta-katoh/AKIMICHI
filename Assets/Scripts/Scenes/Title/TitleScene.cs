using UnityEngine;

namespace Akimichi
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField]
        private TransitionObject transitionObject = null;

        private void Awake()
        {
            TransitionManager.Instance().Initialize();
            TransitionManager.Instance().SetObject(this.transitionObject);
            TransitionManager.Instance().AddScene(SceneConst.Title);
            if (TransitionManager.Instance().IsFirstTransedScene(SceneConst.Title))
            {
                DontDestroyOnLoad(this.transitionObject);
            }

            Application.targetFrameRate = 60;
        }

        public void ChangeScene()
        {
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
