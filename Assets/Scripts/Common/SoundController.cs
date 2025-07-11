using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource bgmAudioSource = null;

        [SerializeField]
        private AudioSource seAudioSource = null;

        [Header("BGM")]
        [SerializeField]
        private List<AudioClip> bgmClips = null;

        [Header("Common")]
        [SerializeField]
        private List<AudioClip> commonClips = null;

        private bool isScheduled = false;
        private AudioClip scheduledClip = null;
        private float frame = 0.0f;
        private float currentVolume = 0.0f;

        private void Awake()
        {
            this.currentVolume = this.bgmAudioSource.volume;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="bgm"></param>
        public void PlayBGM(SoundConst.BGM bgm)
        {
            if(this.bgmAudioSource.clip == null)
            {
                this.isScheduled = false;
                this.bgmAudioSource.clip = this.bgmClips[((int)bgm) - 1];
                this.bgmAudioSource.time = 0.0f;
                this.bgmAudioSource.Play();
            }
            else
            {
                this.isScheduled = true;
                this.scheduledClip = this.bgmClips[((int)bgm) - 1];
                this.frame = 0.5f;
            }
        }

        private void Update()
        {
            if (this.isScheduled)
            {
                this.frame -= Time.deltaTime;
                this.bgmAudioSource.volume = this.currentVolume * (this.frame / 0.5f);
                if(this.frame < 0.0f)
                {
                    this.bgmAudioSource.Stop();
                    this.bgmAudioSource.volume = this.currentVolume;
                    this.bgmAudioSource.clip = this.scheduledClip;
                    this.bgmAudioSource.time = 0.0f;
                    this.bgmAudioSource.Play();

                    this.isScheduled = false;
                }
            }
        }

        /// <summary>
        /// 共通
        /// </summary>
        /// <param name="se"></param>
        public void PlaySE(SoundConst.SE se)
        {
            this.seAudioSource.PlayOneShot(this.commonClips[(int)se]);
        }
    }
}
