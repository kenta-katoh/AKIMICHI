using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventDataBase
    {
        private int mainMessageIndex = 0;
        public bool IsMainMessageLast { get; private set; } = false;
        protected List<string> mainMessageList = new List<string>();
        
        private int yesMessageIndex = 0;
        public bool IsYesMessageLast { get; private set; } = false;
        protected List<string> yesMessageList = new List<string>();
        
        private int noMessageIndex = 0;
        public bool IsNoMessageLast { get; private set; } = false;
        protected List<string> noMessageList = new List<string>();
        public bool IsSelectEvent {  get; protected set; } = false; // セレクト方式かどうか
        protected System.Random random = new System.Random();
        protected object[] datas = new object[10];

        /// <summary>
        /// 文章取得
        /// </summary>
        /// <returns></returns>
        public string Message(EventConst.EventMessageType type)
        {
            string result = "";
            switch(type)
            {
                case EventConst.EventMessageType.Main:
                    result = MainMessage();
                    break;
                case EventConst.EventMessageType.Yes:
                    result = YesMessage();
                    break;
                case EventConst.EventMessageType.No:
                    result = NoMessage();
                    break;
            }
            result += EventConst.MessageLastChar;
            return result;
        }

        private string MainMessage()
        {
            string result = "";
            if (this.mainMessageIndex < this.mainMessageList.Count)
            {
                result = this.mainMessageList[this.mainMessageIndex];
                this.mainMessageIndex++;
                this.IsMainMessageLast = (this.mainMessageIndex == this.mainMessageList.Count);
            }
            else
            {
                this.IsMainMessageLast = true;
            }
            return result;
        }

        private string YesMessage()
        {
            string result = "";
            if (this.yesMessageIndex < this.yesMessageList.Count)
            {
                result = this.yesMessageList[this.yesMessageIndex];
                this.yesMessageIndex++;
                this.IsYesMessageLast = (this.yesMessageIndex == this.yesMessageList.Count);
            }
            else
            {
                this.IsYesMessageLast = true;
            }
            return result;
        }

        private string NoMessage()
        {
            string result = "";
            if (this.noMessageIndex < this.noMessageList.Count)
            {
                result = this.noMessageList[this.noMessageIndex];
                this.noMessageIndex++;
                this.IsNoMessageLast = (this.noMessageIndex == this.noMessageList.Count);
            }
            else
            {
                this.IsNoMessageLast = true;
            }
            return result;
        }

        public virtual void YesAction() { }
        public virtual void NoAction() { }
        public virtual void OnFinished() { }
    }
}
