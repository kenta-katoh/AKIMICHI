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
        private bool isInput = true;
        private float frame = 0.0f; 

        [SerializeField]
        private GameObject debugBtn = null;

        [SerializeField]
        private GameObject debugWindow = null;

        [SerializeField]
        private Toggle startPlayer = null;

        [SerializeField]
        private TMP_InputField gameTime = null;

        [SerializeField]
        private TMP_InputField addWeight = null;

        [SerializeField]
        private TMP_InputField subtractWeight = null;

        [SerializeField]
        private Toggle isEvent = null;

        [SerializeField]
        private TMP_InputField eventValue = null;

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
            int result = Convert.ToInt32(this.gameTime.text);
            if(result > GameConst.GameTime || result < 10) result = GameConst.GameTime;
            return result;
        }

        public void AddWeight()
        {
            int value = Convert.ToInt32(this.addWeight.text);
            if (0 < value && value < 501 && this.isInput)
            {
                if(DebugManager.Instance().AddWeight(value))
                {
                    this.isInput = false;
                    this.frame = 3.0f;
                }
            }
        }

        public void SubtractWeight()
        {
            int value = Convert.ToInt32(this.subtractWeight.text);
            if (0 < value && value < 501 && this.isInput)
            {
                if (DebugManager.Instance().SubtractWeight(value))
                {
                    this.isInput = false;
                    this.frame = 3.0f;
                }
            }
        }

        public int OverwriteEvent()
        {
            int result = -1;
            if(this.isEvent.isOn)
            {
                int value = Convert.ToInt32(this.eventValue.text);
                if(0 < value && value < 8)
                {
                    result = value;
                }
            }
            return result;
        }

        private void Update()
        {
            if(!this.isInput)
            {
                this.frame -= Time.deltaTime;
                if (this.frame < 0.0f)
                {
                    this.isInput = true;
                    this.frame = 0.0f;
                }
            }
        }
    }
}
