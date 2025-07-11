using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi
{
    public class TutorialScene : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> imageList = new List<Sprite>();

        [SerializeField]
        private List<TutorialContents> contents = new List<TutorialContents>();

        [SerializeField]
        private Image image = null;

        [SerializeField]
        private TextMeshProUGUI title = null;

        private void Awake()
        {
            foreach(var item in this.contents)
            {
                item.SetButtonAction(OnClick);
            }
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Main);
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
        }

        public void OnClick(int index)
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            this.image.sprite = this.imageList[index];
            this.title.text = "遊び方" + (index + 1);
        }

        public void OnBack()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Back);
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
