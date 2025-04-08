using UnityEngine;
using UnityEngine.UI;

namespace Akimichi.Game
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private Button leftBrtn = null;

        [SerializeField]
        private Button rightBrtn = null;

        public void MoveLeft()
        {
            Input();
        }

        public void MoveRight()
        {
            Input();
        }

        private void Input()
        {
            if (PlayerManager.Instance().State == PlayerConst.State.WaitingInput)
            {
                PlayerManager.Instance().RollDice();
            }
        }
    }
}
