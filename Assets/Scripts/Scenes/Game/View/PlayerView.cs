using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerView : MonoBehaviour
    {
        public float value = 0.1f;

        private void Update()
        {
            Vector3 pos = gameObject.transform.localPosition;
            if (Input.GetKey(KeyCode.W))
            {
                pos.y += value;
            }

            if (Input.GetKey(KeyCode.A))
            {
                pos.x -= value;
            }

            if (Input.GetKey(KeyCode.S))
            {
                pos.y -= value;
            }

            if (Input.GetKey(KeyCode.D))
            {
                pos.x += value;
            }
            gameObject.transform.localPosition = pos;
        }
    }
}
