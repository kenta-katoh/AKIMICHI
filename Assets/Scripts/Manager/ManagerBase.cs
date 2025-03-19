
namespace Akimichi
{
    public class ManagerBase<T> where T : class, new()
    {
        private static T instance = new T();

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

        public virtual void Dispose() { }
        public virtual void DataTransfer(ManagerData data) { }
        public virtual void Initialize() { }
        public virtual void CreateData() { }
    }
}