using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class GameObjectManager : MonoBehaviour
    {
        private static GameObjectManager instance = null;

        public static GameObjectManager Instance()
        {
            if (instance == null) instance = new GameObjectManager();
            return instance;
        }
    }
}
