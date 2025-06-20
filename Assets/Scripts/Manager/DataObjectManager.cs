using System.Collections.Generic;

namespace Akimichi.Game
{
    public class DataObjectManager : ManagerBase<DataObjectManager>
    {
        private List<SendObjectData> sendObjectDatas = new List<SendObjectData>();

        public override void Initialize()
        {
            base.Initialize();
            this.sendObjectDatas.Clear();
            for (int i = 0; i < 20; ++i)
            {
                this.sendObjectDatas.Add(new SendObjectData());
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.sendObjectDatas.Clear();
        }

        /// <summary>
        /// 送信データ
        /// </summary>
        /// <returns></returns>
        public SendObjectData Get()
        {
            SendObjectData result = null;
            foreach (SendObjectData item in sendObjectDatas)
            {
                if(!item.IsUse)
                {
                    result = item;
                    break;
                }
            }

            // 送信データが足りてないので生成
            if(result == null)
            {
                result = new SendObjectData();
                this.sendObjectDatas.Add(result);
            }

            result.IsUse = true;
            result.ClearData();
            return result;
        }
    }
}
