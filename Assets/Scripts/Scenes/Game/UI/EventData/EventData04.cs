
using System;

namespace Akimichi.Game
{
    public class EventData04 : EventDataBase
    {
        private int value = 0;
        private bool result = false;

        public EventData04()
        {
            this.value = this.random.Next(20, 51);
            this.result = Convert.ToBoolean(new Random().Next(0, 2));
            this.IsSelectEvent = true;
            this.mainMessageList.Add("ラッキー！\n豪華な食事にありつけた。");
            this.mainMessageList.Add("料理を平らげますか？");
            if(this.result)
            {
                this.yesMessageList.Add(string.Format("豪華な食事を完食し、体重アップ！\n{0}kg増えた！", this.value));
            }
            else
            {
                this.yesMessageList.Add("豪華な食事を完食し、体重アップ！？");
                this.yesMessageList.Add(string.Format("食べ過ぎて胃もたれになってしまった…\n{0}kg減った", this.value));
            }
            this.noMessageList.Add("食事をあきらめた。");
        }

        public override void OnFinished()
        {
            base.OnFinished();
            if (this.resultType != EventConst.EventMessageType.Yes) return;
            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            send.Datas[1] = this.value;
            if (this.result)
            {
                NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, send);
            }
            else
            {
                NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, send);
            }
        }
    }
}


