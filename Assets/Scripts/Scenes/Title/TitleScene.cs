using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Akimichi
{
    public class TitleScene : MonoBehaviour
    {
        public void ChangeScene()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}
