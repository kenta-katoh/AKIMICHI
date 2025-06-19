using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class EventData01 : EventDataBase
    {
        public EventData01()
        {
            this.mainMessageList.Add("ラッキー！\n豪華な食事にありつけた。");
            this.mainMessageList.Add("食べますか？");
        }
    }
}
