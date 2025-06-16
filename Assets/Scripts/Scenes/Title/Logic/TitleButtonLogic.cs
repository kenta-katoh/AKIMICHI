using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akimichi.Game;
using Manager;
using Akimichi.Data;

namespace Akimichi.Title
{
    public class TitleButtonLogic : LogicBase
    {
        public TitleButtonLogic(ViewBase view) : base(view)
        {
        }

        public void ChangeScene()
        {
            // テストでリザルトへ
            SceneManager.Instance().ChangeScene("LobbyScene");
        }

        public void CreateData()
        {
            // テストデータ作成
            //var data = new TestDataBase();
            //data.test = 15;
            //data.SetValue(DataConst.TestStringValueKey,"111");
            //DataManager.Instance().SetData(DataConst.TestDataBaseKey,data);
        }
    }
}

