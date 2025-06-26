using TMPro;

namespace Akimichi.Game
{
    public class GameStateManagerData : ManagerData
    {
        public GameProgressManager ProgressManager { get; set; }
        public TextMeshProUGUI Timer {  get; set; }
    }
}
