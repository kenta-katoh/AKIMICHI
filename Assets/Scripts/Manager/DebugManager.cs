using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class DebugManager : ManagerBase<DebugManager>
    {
        public bool IsDebug { get; private set; } = false;
        private int[] debugCode = new int[] { 6, 5, 4, 6, 5, 4 };
        private int debugCodeIndex = 0;
        private DebugObject debugObject = null;
        
        /// <summary>
        /// オブジェクト設定
        /// </summary>
        /// <param name="debugObject"></param>
        public void SetObject(DebugObject debugObject)
        { 
            this.debugObject = debugObject;
            this.debugCodeIndex = 0;
            this.IsDebug = false;
        }

        /// <summary>
        /// デバッグコード登録
        /// </summary>
        /// <param name="code"></param>
        public void ResistDebugCode(int code)
        {
            if (this.IsDebug) return;
            if(this.debugCodeIndex < this.debugCode.Length)
            {
                int value = this.debugCode[this.debugCodeIndex];
                if(code == value)
                {
                    this.debugCodeIndex++;
                    if(this.debugCodeIndex == this.debugCode.Length)
                    {
                        this.IsDebug = true;
                        this.debugObject.DebugMode();
                    }
                }
                else
                {
                    this.debugCodeIndex = 0;
                }
            }
        }

        /// <summary>
        /// 少人数スタート
        /// </summary>
        /// <returns></returns>
        public bool IsStartPlayer()
        {
            bool result = false;
            if(this.debugObject != null)
            {
                result = this.debugObject.IsStartPlayer();
            }
            return result;
        }

        /// <summary>
        /// 制限時間
        /// </summary>
        /// <returns></returns>
        public int GameTime()
        {
            int result = GameConst.GameTime;
            if (this.debugObject != null)
            {
                result = this.debugObject.GameTime();
            }
            return result;
        }
    }
}
