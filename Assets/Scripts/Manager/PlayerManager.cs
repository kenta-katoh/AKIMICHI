using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance = null;

        public static PlayerManager Instance()
        {
            if (instance == null) instance = new PlayerManager();
            return instance;
        }
    }
}
