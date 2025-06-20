
namespace Akimichi
{
    public class ManagerBase<T> where T : class, new()
    {
        private static T instance = new T();
        protected object[] datas = new object[10];

        public static T Instance()
        {
            return instance;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ManagerBase()
        {
            instance = null;
            Dispose();
        }

        protected void ClearSendData()
        {
            for (int i = 0; i < this.datas.Length; ++i)
            {
                this.datas[i] = null;
            }
        }

        public virtual void Dispose()
        { 
            ClearSendData();
        }
        public virtual void DataTransfer(ManagerData data) { }
        public virtual void Initialize() { }
        public virtual void CreateData() { }
        public virtual void ManagedUpdate() { }
    }
}