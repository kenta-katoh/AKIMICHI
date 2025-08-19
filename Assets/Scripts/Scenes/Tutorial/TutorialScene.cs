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

        private List<string> list = new List<string>() { "基本ルール", 
                                                         "サイコロ",
                                                         "マス",
                                                         "体重",
                                                         "ぶつかり稽古1",
                                                         "ぶつかり稽古2",
                                                         "近くのプレイヤー",
                                                         "本場所"};

        private bool isInit = false;

        private void Awake()
        {
            foreach(var item in this.contents)
            {
                item.SetButtonAction(OnClick);
                item.VisibleSelect(false);
            }
            AudioManager.Instance().PlayBGM(SoundConst.BGM.Main);
        }

        private void Start()
        {
            int index = 0;
            this.image.sprite = this.imageList[index];
            this.title.text = this.list[index];
            this.contents[index].Initialized();

            TransitionManager.Instance().Open(() => { this.isInit = true; });
        }

        public void OnClick(int index)
        {
            if(this.isInit) AudioManager.Instance().PlaySE(SoundConst.SE.Decide);
            this.image.sprite = this.imageList[index];
            this.title.text = this.list[index];
            DebugManager.Instance().ResistDebugCode(index + 1);

            int value = 0;
            foreach(var item in this.contents)
            {
                if(value != index) item.VisibleSelect(false);
                value++;
            }
        }

        public void OnBack()
        {
            AudioManager.Instance().PlaySE(SoundConst.SE.Back);
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
