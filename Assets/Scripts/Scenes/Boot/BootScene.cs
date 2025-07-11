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

        [SerializeField]
        private SoundController soundController = null;

        [SerializeField]
        private List<Sprite> sprites1st = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites2nd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites3rd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites4th = new List<Sprite>();

        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif

            TransitionManager.Instance().Initialize();
            TransitionManager.Instance().SetObject(this.transitionObject);
            AudioManager.Instance().SetController(this.soundController);
            DontDestroyOnLoad(this.transitionObject);
            DontDestroyOnLoad(this.photonManager);
            DontDestroyOnLoad(this.soundController);

            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.First, this.sprites1st);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Second, this.sprites2nd);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Third, this.sprites3rd);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Fourth, this.sprites4th);
        }

        private void Start()
        {
            TransitionManager.Instance().Transition(SceneConst.Title);
        }
    }
}
