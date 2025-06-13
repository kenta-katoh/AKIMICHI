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

        public void PlayAnime(string boolTag, bool flag, string transTag, Action finish)
        {
            if (this.animator != null && !this.IsPlaying)
            {
                this.animeHash = Animator.StringToHash(transTag);
                this.animator.SetBool(boolTag, flag);
                this.IsPlaying = true;
                this.onFinished = null;
                this.onFinished = finish;
            }
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

        virtual protected void OnFinished() { }
    }
}
