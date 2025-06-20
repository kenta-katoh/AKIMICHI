
namespace Akimichi.Game
{
    public class EventData01 : EventDataBase
    {
        private int value = 0;

        public EventData01()
        {
            this.value = this.random.Next(50, 71);
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
            this.datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            this.datas[1] = this.value;
            NetworkManager.Instance().SendEvent(EventConst.Event.AddWeight, this.datas);
        }
    }
}
