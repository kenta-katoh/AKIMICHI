using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSceneManager : MonoBehaviour
{
    public void ConnectScene()
    {
        TestSceneManager.Instance().ChangeScene("LobbyScene");
    }
}
