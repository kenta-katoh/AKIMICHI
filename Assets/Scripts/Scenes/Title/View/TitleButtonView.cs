using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akimichi.Game;

namespace Akimichi.Title
{
    public class TitleButtonView : ViewBase
    {
        TitleButtonLogic GetLogic()
        {
            return (TitleButtonLogic)this.logic;
        }
        protected override void OnAwake()
        {
            base.OnAwake();
            this.logic = new TitleButtonLogic(this);
        }

        public void ChangeScene()
        {
            GetLogic().CreateData();
            GetLogic().ChangeScene();
        }
    }
}


