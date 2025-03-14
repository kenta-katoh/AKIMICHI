using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapSpaceViewBase : ViewBase
    {
        [SerializeField]
        private bool isStartingPosition = false;
        public bool IsStartingPosition {  get { return isStartingPosition; } }

        /// <summary>
        /// マスインデックス
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            ((MapSpaceLogicBase)this.logic).SetIndex(index);
        }
    }
}
