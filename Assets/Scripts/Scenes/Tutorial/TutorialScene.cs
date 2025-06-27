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
        }

        private void Start()
        {
            TransitionManager.Instance().Open();
        }

        public void OnClick(int index)
        {
            this.image.sprite = this.imageList[index];
            this.title.text = "遊び方" + (index + 1);
        }

        public void OnBack()
        {
            TransitionManager.Instance().Transition(SceneConst.Home);
        }
    }
}
