using Akimichi.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Akimichi
{
    public class ResultDataContents : MonoBehaviour
    {
        [SerializeField]
        private Image image = null;

        [SerializeField]
        private TextMeshProUGUI weight = null;

        [SerializeField]
        private TextMeshProUGUI dice = null;

        [SerializeField]
        private TextMeshProUGUI plus = null;

        [SerializeField]
        private TextMeshProUGUI minus = null;

        [SerializeField]
        private TextMeshProUGUI eventCount = null;

        [SerializeField]
        private TextMeshProUGUI move = null;

        [SerializeField]
        private TextMeshProUGUI practice = null;

        public void SetData(GameConst.PlayerIndex index, PlayerData data)
        {
            this.image.sprite = PlayerSpriteManager.Instance().GetUISprite(index, data.GetLevel());
            this.weight.text = data.Weight.ToString();
            this.dice.text = data.DiceCount.ToString();
            this.plus.text = data.PlusCount.ToString();
            this.minus.text = data.MinusCount.ToString();
            this.eventCount.text = data.EventCount.ToString();
            this.move.text = data.MoveValue.ToString();
            this.practice.text = data.PracticeCount.ToString();
        }
    }
}
