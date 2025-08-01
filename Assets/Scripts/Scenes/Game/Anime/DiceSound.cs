using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class DiceSound : MonoBehaviour
    {
        public void DiceDicede()
        {
            AudioManager.Instance().PlaySE(SoundConst.GAME.DiceDecide);
        }
    }
}
