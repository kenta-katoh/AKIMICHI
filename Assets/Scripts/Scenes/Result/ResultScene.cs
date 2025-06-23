using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class ResultScene : MonoBehaviour
    {
        private void Awake()
        {
            NetworkManager.Instance().DeleteCallBack();
            NetworkManager.Instance().LeaveRoom();
            NetworkManager.Instance().Disconnect();
        }
    }
}
