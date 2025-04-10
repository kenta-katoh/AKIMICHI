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

        private void Update()
        {
            this.logic.OnManagedUpdate();
            OnManagedUpdate();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnManagedUpdate() { }
    }
}
