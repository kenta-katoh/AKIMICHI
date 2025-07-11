using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class GameSoundEvent : MonoBehaviour
    {
        public void PlaySECount1()
        {
            AudioManager.Instance().PlaySE(SoundConst.GAME.Count1);
        }

        public void PlaySECount2()
        {
            AudioManager.Instance().PlaySE(SoundConst.GAME.Count2);
        }

        public void PlaySECount3()
        {
            AudioManager.Instance().PlaySE(SoundConst.GAME.Count3);
        }
    }
}
