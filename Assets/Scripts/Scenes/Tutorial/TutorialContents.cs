using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Akimichi
{
    public class TutorialContents : MonoBehaviour
    {
        [SerializeField]
        private int Index = 0;

        [SerializeField]
        private TextMeshProUGUI text = null;

        private Action<int> action = null;

        private void Awake()
        {
            this.text.text = "遊び方" + (this.Index + 1).ToString();
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
        }
    }
}
