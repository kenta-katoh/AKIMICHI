using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneManager : MonoBehaviour
    {
        List<string> sceneList = new List<string>();
        private static SceneManager instance = null;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            SceneManager.Instance().DeleteScene("StartScene");
            SceneManager.Instance().ChangeScene("TitleScene");
        }

        private SceneManager()
        {
        }

        public static SceneManager Instance()
        {
            if (instance == null) instance = new SceneManager();
            return instance;
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="name"></param>
        public void ChangeScene(string name)
        {
            if (this.sceneList.Count > 0)
            {
                string deleteSceneName = "";
                deleteSceneName = this.sceneList[0];
                this.sceneList.RemoveAt(0);
                DeleteScene(name);
            }
            this.sceneList.Add(name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }

        /// <summary>
        /// シーン削除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteScene(string name)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
        }
    }
}

