using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Akimichi.SoundConst;

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

        private bool isScheduled = false;
        private AudioClip scheduledClip = null;
        private float frame = 0.0f;

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="bgm"></param>
        public void PlayBGM(SoundConst.BGM bgm)
        {
            if(this.bgmAudioSource.clip == null)
            {
                this.isScheduled = false;
                this.bgmAudioSource.clip = this.bgmClips[((int)bgm) + 1];
                this.bgmAudioSource.time = 0.0f;
                this.bgmAudioSource.Play();
            }
            else
            {
                this.isScheduled = true;
                this.scheduledClip = this.bgmClips[((int)bgm) + 1];
                this.frame = 0.5f;
            }
        }

        private void Update()
        {
            if (this.isScheduled)
            {
                this.frame -= Time.deltaTime;
                this.bgmAudioSource.volume = this.frame * 2.0f;
                if(this.frame < 0.0f)
                {
                    this.bgmAudioSource.Stop();
                    this.bgmAudioSource.volume = 1.0f;
                    this.bgmAudioSource.clip = this.scheduledClip;
                    this.bgmAudioSource.time = 0.0f;
                    this.bgmAudioSource.Play();

                    this.isScheduled = false;
                }
            }
        }
    }
}
