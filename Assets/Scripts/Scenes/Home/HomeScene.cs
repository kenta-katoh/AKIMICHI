using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Akimichi
{
    public class HomeScene : MonoBehaviour
    {
        public void ChangeLobby()
        {
            SceneManager.LoadScene("LobbyScene");
        }

        public void ChangeTutorial()
        {
        }
    }
}
