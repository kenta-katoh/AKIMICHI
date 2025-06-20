
namespace Akimichi.Game
{
    public class EventData05 : EventDataBase
    {
        private int value = 0;

        public EventData05()
        {
            this.value = this.random.Next(30, 51);
            this.IsSelectEvent = true;
            this.mainMessageList.Add("ラッキー！\n豪華な食事にありつけた。");
            this.mainMessageList.Add("料理を平らげますか？");
            this.yesMessageList.Add("豪華な食事を完食し、体重アップ！？");
            this.yesMessageList.Add(string.Format("全て食品サンプルだった…\nショックで{0}kg減った。", this.value));
            this.noMessageList.Add("食事をあきらめた。");
        }

        public override void OnFinished()
        {
            base.OnFinished();
            if (this.resultType != EventConst.EventMessageType.Yes) return;
            this.datas[0] = (int)PlayerManager.Instance().PlayerIndex;
            this.datas[1] = this.value;
            NetworkManager.Instance().SendEvent(EventConst.Event.SubtractWeight, this.datas);
        }
    }
}