using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Data
{
    /// <summary>
    /// シーン間データ受け渡し想定
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        private Dictionary<string, DataBase> dataList = new Dictionary<string, DataBase>();
        private static DataManager instance = null;
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        public static DataManager Instance()
        {
            if (instance == null) instance = new DataManager();
            return instance;
        }

        private DataManager()
        {
        }

        public void SetData(string key, DataBase data)
        {
            this.dataList[key] = data;
        }

        public DataBase GetData(string key)
        {
            if (this.dataList.ContainsKey(key)) return this.dataList[key];
            return null;
        }

        public void Delete(string key)
        {
            if (this.dataList.ContainsKey(key)) this.dataList[key] = null;
        }

        public void AllDelete()
        {
            this.dataList.Clear();
        }
    }

}

