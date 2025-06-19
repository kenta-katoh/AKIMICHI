using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventData01 : EventDataBase
    {
        public EventData01()
        {
            this.IsSelectEvent = true;
            this.mainMessageList.Add("ラッキー！\n豪華な食事にありつけた。");
            this.mainMessageList.Add("食べますか？");
            this.yesMessageList.Add("豪華な食事を完食し、体重アップ！\n〇〇kg増えた！");
            this.noMessageList.Add("食事をあきらめた。");
        }
    }
}
