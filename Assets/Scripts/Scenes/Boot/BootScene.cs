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
        private DebugObject debugObject = null;

        [SerializeField]
        private List<Sprite> sprites1st = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites2nd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites3rd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> sprites4th = new List<Sprite>();

        [SerializeField]
        private List<Sprite> uiSprites1st = new List<Sprite>();

        [SerializeField]
        private List<Sprite> uiSprites2nd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> uiSprites3rd = new List<Sprite>();

        [SerializeField]
        private List<Sprite> uiSprites4th = new List<Sprite>();

        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif

            TransitionManager.Instance().Initialize();
            TransitionManager.Instance().SetObject(this.transitionObject);
            AudioManager.Instance().SetController(this.soundController);
            DebugManager.Instance().SetObject(this.debugObject);
            DontDestroyOnLoad(this.transitionObject);
            DontDestroyOnLoad(this.photonManager);
            DontDestroyOnLoad(this.soundController);
            DontDestroyOnLoad(this.debugObject);

            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.First, this.sprites1st);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Second, this.sprites2nd);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Third, this.sprites3rd);
            PlayerSpriteManager.Instance().SetSprite(Game.GameConst.PlayerIndex.Fourth, this.sprites4th);

            PlayerSpriteManager.Instance().SetUISprite(Game.GameConst.PlayerIndex.First, this.uiSprites1st);
            PlayerSpriteManager.Instance().SetUISprite(Game.GameConst.PlayerIndex.Second, this.uiSprites2nd);
            PlayerSpriteManager.Instance().SetUISprite(Game.GameConst.PlayerIndex.Third, this.uiSprites3rd);
            PlayerSpriteManager.Instance().SetUISprite(Game.GameConst.PlayerIndex.Fourth, this.uiSprites4th);
        }

        private void Start()
        {
            TransitionManager.Instance().Transition(SceneConst.Title);
        }
    }
}
