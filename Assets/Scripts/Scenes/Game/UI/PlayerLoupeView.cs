using UnityEngine;

namespace Akimichi.Game
{
    public class PlayerLoupeView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform canvas = null;

        [SerializeField]
        private PlayerLoupe player1st = null;

        [SerializeField]
        private PlayerLoupe player2nd = null;

        [SerializeField]
        private PlayerLoupe player3rd = null;

        [SerializeField]
        private PlayerLoupe player4th = null;

        private void Awake()
        {
            this.player1st.SetCanvas(this.canvas.sizeDelta);
            this.player2nd.SetCanvas(this.canvas.sizeDelta);
            this.player3rd.SetCanvas(this.canvas.sizeDelta);
            this.player4th.SetCanvas(this.canvas.sizeDelta);
        }

        public void UpdateLoupe(GameConst.PlayerIndex index, float length, float rot)
        {
            switch(index)
            {
                case GameConst.PlayerIndex.First:
                    this.player1st.UpdateLoupe(length, rot);
                    break;
                case GameConst.PlayerIndex.Second:
                    this.player2nd.UpdateLoupe(length, rot);
                    break;
                case GameConst.PlayerIndex.Third:
                    this.player3rd.UpdateLoupe(length, rot);
                    break;
                case GameConst.PlayerIndex.Fourth:
                    this.player4th.UpdateLoupe(length, rot);
                    break;
            }
        }
    }
}
