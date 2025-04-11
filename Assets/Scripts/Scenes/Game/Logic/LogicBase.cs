using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class LogicBase
    {
        protected ViewBase view = null;
        
        public LogicBase(ViewBase viewBase)
        { 
            this.view = viewBase;
        }

        public virtual void OnManagedUpdate() { }

        public Transform GetTransform()
        {
            return this.view.transform;
        }
    }
}
