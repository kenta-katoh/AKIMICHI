using Akimichi.Game;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Akimichi
{
    public class TransitionManager : ManagerBase<TransitionManager>
    {
        private TransitionObject transitionObject = null;
        private string sceneName = "";
        private Action onClose = null;
        private Action onOpen = null;
        private Dictionary<string, int> transedSceneDic = new Dictionary<string, int>();

        public override void Initialize()
        {
            base.Initialize();
            this.transedSceneDic.Clear();
        }

        /// <summary>
        /// オブジェクト設定
        /// </summary>
        /// <param name="obj"></param>
        public void SetObject(TransitionObject obj)
        {
            if(this.transitionObject == null)
            {
                this.transitionObject = obj;
            }
        }

        /// <summary>
        /// 遷移(閉めるだけ)
        /// </summary>
        /// <param name="name"></param>
        public void Transition(string name, Action action = null)
        {
            this.onClose = null;
            this.onClose = action;
            this.sceneName = name;
            Transition();
        }

        /// <summary>
        /// 遷移明け
        /// </summary>
        /// <param name="action"></param>
        public void Open(Action action = null)
        {
            this.onOpen = action;
            if (this.transitionObject != null)
            {
                this.transitionObject.Open(this.onOpen);
            }
            else
            {
                this.onOpen?.Invoke();
            }
        }

        /// <summary>
        /// 閉じるだけ
        /// </summary>
        /// <param name="action"></param>
        public void Close(Action action = null)
        {
            this.onClose = null;
            this.onClose = action;
            if (this.transitionObject != null)
            {
                this.transitionObject.Close(() =>
                {
                    this.onClose?.Invoke();
                    NetworkManager.Instance().DeleteCallBack();
                });
            }
            else
            {
                this.onClose?.Invoke();
            }
        }

        /// <summary>
        /// 同期遷移
        /// </summary>
        /// <param name="name"></param>
        public void SysncTransition(string name, Action action = null)
        {
            this.onClose = null;
            this.onClose = action;
            this.sceneName = name;
            SysncTransition();
        }

        private void SysncTransition()
        {
            if (this.transitionObject != null)
            {
                this.transitionObject.Close(() =>
                {
                    this.onClose?.Invoke();
                    NetworkManager.Instance().DeleteCallBack();
                    NetworkManager.Instance().SysncLoadScene(this.sceneName);
                });
            }
            else
            {
                this.onClose?.Invoke();
                NetworkManager.Instance().DeleteCallBack();
                NetworkManager.Instance().SysncLoadScene(this.sceneName);
            }
        }

        private void Transition()
        {
            if (this.transitionObject != null)
            {
                this.transitionObject.Close(() => 
                {
                    this.onClose?.Invoke();
                    NetworkManager.Instance().DeleteCallBack();
                    SceneManager.LoadScene(this.sceneName);
                });
            }
            else
            {
                this.onClose?.Invoke();
                NetworkManager.Instance().DeleteCallBack();
                SceneManager.LoadScene(this.sceneName);
            }
        }

        /// <summary>
        /// 遷移カウント追加
        /// </summary>
        /// <param name="sceneName"></param>
        public void AddScene(string sceneName)
        {
            if(this.transedSceneDic.ContainsKey(sceneName))
            {
                this.transedSceneDic[sceneName]++;
            }
            else
            {
                this.transedSceneDic.Add(sceneName, 1);
            }
        }

        /// <summary>
        /// 初回遷移か
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public bool IsFirstTransedScene(string sceneName)
        {
            if(!this.transedSceneDic.ContainsKey(sceneName))
            {
                return true;
            }
            else
            {
                return this.transedSceneDic[sceneName] == 1;
            }
        }
    }
}
