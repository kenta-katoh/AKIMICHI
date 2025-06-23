using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Akimichi
{
    public class HomeScene : MonoBehaviour
    {
        private void Awake()
        {
            NetworkManager.Instance().Disconnect();
        }

        public void ChangeLobby()
        {
            SceneManager.LoadScene(SceneConst.Lobby);
        }

        public void ChangeTutorial()
        {
        }
    }
}
