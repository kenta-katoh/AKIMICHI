using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerView : ViewBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new PlayerLogic(this);
        }
    }
}
