using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource = null;

        [Header("BGM")]
        [SerializeField]
        private List<AudioClip> bgmClips = null;

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="bgm"></param>
        public void PlayBGM(SoundConst.BGM bgm)
        {
            this.audioSource.Stop();
            this.audioSource.clip = this.bgmClips[((int)bgm) + 1];
            this.audioSource.Play();
        }
    }
}
