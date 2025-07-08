using Akimichi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class BootScene : MonoBehaviour
    {
        [SerializeField]
        private TransitionObject transitionObject = null;

        [SerializeField]
        private AkimichiPhotonManager photonManager = null;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            TransitionManager.Instance().Initialize();
            TransitionManager.Instance().SetObject(this.transitionObject);
            DontDestroyOnLoad(this.transitionObject);
            DontDestroyOnLoad(this.photonManager);
        }

        private void Start()
        {
            TransitionManager.Instance().Transition(SceneConst.Title);
        }
    }
}
