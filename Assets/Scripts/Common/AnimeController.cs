using System;
using UnityEngine;

namespace Akimichi
{
    [RequireComponent(typeof(Animator))]
    public class AnimeController : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;
        private Action onFinished = null;
        private int animeHash = -1;
        public bool IsPlaying { get; private set; } = false;

        public bool PlayAnime(string boolTag, bool flag, string transTag, Action finish)
        {
            bool result = false;
            if (this.animator != null && !this.IsPlaying)
            {
                result = true;
                this.animeHash = Animator.StringToHash(transTag);
                this.animator.SetBool(boolTag, flag);
                this.IsPlaying = true;
                this.onFinished = null;
                this.onFinished = finish;
            }
            return result;
        }

        private void Update()
        {
            if(this.IsPlaying && this.animator != null)
            {
                if (this.animator.GetCurrentAnimatorStateInfo(0).tagHash == this.animeHash &&
                    this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    this.animeHash = -1;
                    this.IsPlaying = false;
                    this.onFinished?.Invoke();
                    OnFinished();
                }
            }
        }

        /// <summary>
        /// フラグ制御
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="flag"></param>
        public void SetBool(string tag, bool flag)
        {
            this.animator.SetBool(tag, flag);
        }

        /// <summary>
        /// int設定
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public void SetValue(string tag, int value)
        {
            this.animator.SetInteger(tag, value);
        }

        /// <summary>
        /// 強制再生
        /// </summary>
        /// <param name="name"></param>
        public void ForcePlay(string name)
        {
            this.animator.Play(name, 0, 0);
            this.IsPlaying = false;
        }

        /// <summary>
        /// action削除
        /// </summary>
        public void DeleteAction()
        {
            this.onFinished = null;
        }

        virtual protected void OnFinished() { }
    }
}
