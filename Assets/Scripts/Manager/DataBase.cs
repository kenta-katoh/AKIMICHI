using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Data
{
    public class DataBase
    {
        private Dictionary<string, string> valueList = new Dictionary<string, string>();
        public DataBase()
        {
        }

        ~DataBase()
        {
        }

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key,string value)
        {
            this.valueList[key] = value;
        }

        /// <summary>
        /// 値の取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (this.valueList.ContainsKey(key)) return this.valueList[key];
            return string.Empty;
        }
    }


    public static class DataConst
    {
        // データキー
        public static string TestDataBaseKey = "TestDataBase";

        // Valueキー
        public static string TestStringValueKey = "TestStringValueKeyb";
    }
}


