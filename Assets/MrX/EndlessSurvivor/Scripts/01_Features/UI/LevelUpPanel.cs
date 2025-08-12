using UnityEngine;
using UniRx;
using TMPro;

namespace MrX.EndlessSurvivor
{
    public class LevelUpPanel : MonoBehaviour
    {
        public TextMeshProUGUI levelUpText; // Kéo Text hiển thị số 3 vào đây
        public Player player;               // Kéo Player vào đây

        void Start()
        {
            // Lắng nghe sự thay đổi của Level từ Player
            player.CurrentLevel
                .Subscribe(newLevel =>
                {
                    // Cập nhật Text với level mới
                    levelUpText.text = "" + newLevel;
                })
                .AddTo(this);
        }
    }
}