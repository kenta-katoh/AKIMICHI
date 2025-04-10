using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class DiceManager : ManagerBase<DiceManager>
    {
        public int DiceValue { get; private set; } = 0;
        private System.Random rand = new System.Random();

        /// <summary>
        /// ダイス振り
        /// </summary>
        public bool DiceRoll()
        {
            if (PlayerManager.Instance().State == PlayerConst.State.WaitingInput)
            {
                this.DiceValue = this.rand.Next(1, 7);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 1進んだ
        /// </summary>
        public void DiceDecrement()
        {
            if (this.DiceValue > 0)
            {
                this.DiceValue--;
            }
        }

        /// <summary>
        /// ダイス目が残っているかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsDiceRest()
        {
            return this.DiceValue > 0;
        }
    }
}
