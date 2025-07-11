
namespace Akimichi.Game
{
    public class EventData02 : EventDataBase
    {
        private int value = 0;

        public EventData02()
        {
            this.value = this.random.Next(70, 101);
            this.IsSelectEvent = true;
            this.mainMessageList.Add("ラッキー！\n豪華な食事にありつけた。");
            this.mainMessageList.Add("料理を平らげますか？");
            this.yesMessageList.Add(string.Format("豪華な食事を完食し、体重アップ！\n{0}kg増えた！", this.value));
            this.noMessageList.Add("食事をあきらめた。");
        }

        public override void OnFinished()
        {
            base.OnFinished();
            if (this.resultType != EventConst.EventMessageType.Yes) return;
            var send = DataObjectManager.Instance().Get();
            send.Datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            send.Datas[1] = this.value;
            NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, send);
        }
    }
}

