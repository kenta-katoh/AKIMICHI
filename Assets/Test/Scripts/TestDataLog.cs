using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akimichi.Data;

public class TestDataLog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var data = (TestDataBase)DataManager.Instance().GetData(DataConst.TestDataBaseKey);
        Debug.Log("#####");
        Debug.Log(data.test.ToString());
        Debug.Log(data.GetValue(DataConst.TestStringValueKey));
        Debug.Log("#####");
    }

}
