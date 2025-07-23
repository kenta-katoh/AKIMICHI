using Akimichi.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi
{
    public class DebugObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject debugBtn = null;

        [SerializeField]
        private GameObject debugWindow = null;

        [SerializeField]
        private Toggle startPlayer = null;

        [SerializeField]
        private TMP_InputField inputField = null;

        private void Awake()
        {
            this.debugBtn.SetActive(false);
            this.debugWindow.SetActive(false);
        }

        /// <summary>
        /// デバッグ有効化
        /// </summary>
        public void DebugMode()
        {
            this .debugBtn.SetActive(true);
        }

        public void OpenWindow()
        {
            this.debugWindow.SetActive(true);
        }

        public void CloseWindow()
        {
            this.debugWindow.SetActive(false);
        }

        /// <summary>
        /// 少人数スタート
        /// </summary>
        /// <returns></returns>
        public bool IsStartPlayer()
        {
            return this.startPlayer.isOn;
        }

        /// <summary>
        /// 制限時間
        /// </summary>
        /// <returns></returns>
        public int GameTime()
        {
            int result = Convert.ToInt32(this.inputField.text);
            if(result > GameConst.GameTime || result < 10) result = GameConst.GameTime;
            return result;
        }
    }
}
