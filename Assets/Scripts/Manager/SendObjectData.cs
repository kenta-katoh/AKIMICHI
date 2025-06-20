
namespace Akimichi.Game
{
    public class SendObjectData
    {
        public bool IsUse { get; set; } = false;
        public object[] Datas { get; set; } = new object[10];

        /// <summary>
        /// 送信データクリア
        /// </summary>
        public void ClearData()
        {
            for (int i = 0; i < this.Datas.Length; ++i)
            {
                this.Datas[i] = null;
            }
        }
    }
}
