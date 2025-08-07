using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi
{
    public class TutorialContents : MonoBehaviour
    {
        [SerializeField]
        private int Index = 0;

        [SerializeField]
        private TextMeshProUGUI text = null;

        [SerializeField]
        private Image imageBase = null;

        private Action<int> action = null;
        private List<string> list = new List<string>() { "基本ルール",
                                                         "サイコロ",
                                                         "マス",
                                                         "体重",
                                                         "ぶつかり稽古1",
                                                         "ぶつかり稽古2",
                                                         "近くのプレイヤー",
                                                         "本場所"};

        private void Awake()
        {
            this.text.text = this.list[this.Index];
        }

        /// <summary>
        /// 押下時
        /// </summary>
        /// <param name="action"></param>
        public void SetButtonAction(Action<int> action)
        {
            this.action = action;
        }

        public void OnClick()
        {
            this.action?.Invoke(this.Index);
            VisibleSelect(true);
        }

        /// <summary>
        /// 選択切り替え
        /// </summary>
        /// <param name="flag"></param>
        public void VisibleSelect(bool flag)
        {
            this.imageBase.enabled = flag;
        }
    }
}
