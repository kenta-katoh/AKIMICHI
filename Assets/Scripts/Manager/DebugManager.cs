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
        private bool isInGame = false;
        
        /// <summary>
        /// オブジェクト設定
        /// </summary>
        /// <param name="debugObject"></param>
        public void SetObject(DebugObject debugObject)
        { 
            this.debugObject = debugObject;
            this.debugCodeIndex = 0;
            this.IsDebug = false;
            this.isInGame = false;
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
        /// インゲーム設定
        /// </summary>
        /// <param name="isDebug"></param>
        public void InGame(bool isDebug)
        {
            this.isInGame = isDebug;
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

        /// <summary>
        /// 体重増加
        /// </summary>
        /// <param name="weight"></param>
        public bool AddWeight(int weight)
        {
            if (this.debugObject == null) return false;
            if (this.isInGame)
            {
                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
                send.Datas[1] = weight;
                send.Datas[2] = (byte)1;
                NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, send);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 体重減少
        /// </summary>
        /// <param name="weight"></param>
        public bool SubtractWeight(int weight)
        {
            if (this.debugObject == null) return false;
            if (this.isInGame)
            {
                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
                send.Datas[1] = weight;
                NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, send);
                return true;
            }
            return false;
        }

        public int OverwriteEvent()
        {
            int result = -1;
            if(this.debugObject != null && this.IsDebug)
            {
                result = this.debugObject.OverwriteEvent();
            }
            return result;
        }

        public int OverwriteDice()
        {
            int result = -1;
            if (this.debugObject != null && this.IsDebug)
            {
                result = this.debugObject.OverwriteDice();
            }
            return result;
        }

        /// <summary>
        /// 疲労状態の切り替え
        /// </summary>
        public bool SwitchFatigue(bool flag)
        {
            if (this.debugObject == null) return false;
            if (this.isInGame)
            {
                var send = DataObjectManager.Instance().Get();
                send.Datas[0] = (byte)PlayerManager.Instance().PlayerIndex;
                if(flag) NetworkManager.Instance().SendEvent(EventConst.Event.HoldFatigue, send);
                else NetworkManager.Instance().SendEvent(EventConst.Event.ReleaseFatigue, send);
                return true;
            }
            return false;
        }
    }
}
