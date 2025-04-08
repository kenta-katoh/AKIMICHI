using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class ViewBase : MonoBehaviour
    {
        protected LogicBase logic = null;
        public LogicBase Logic { get { return logic; } }

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
