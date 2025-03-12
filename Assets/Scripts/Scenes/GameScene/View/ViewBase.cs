using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class ViewBase : MonoBehaviour
    {
        protected LogicBase logic = null;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
