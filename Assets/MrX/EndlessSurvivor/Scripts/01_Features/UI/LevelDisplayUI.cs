using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro; // Cần để dùng TextMeshPro

namespace MrX.EndlessSurvivor
{
    public class LevelDisplayUI : MonoBehaviour
    {
        [SerializeField] private Slider sliderXp;
        [SerializeField] private TextMeshProUGUI level;      // Text hiển thị %
        public Player player; // Kéo Player vào đây
        void Start()
        {
            // Lắng nghe sự thay đổi của Level để cập nhật Text
            player.CurrentLevel
                .Subscribe(newLevel =>
                {
                    level.text = "" + newLevel;
                })
                .AddTo(this);
            // Lắng nghe BẤT KỲ KHI NÀO CurrentXp HOẶC XpToNextLevel thay đổi
            // để tính toán lại tỉ lệ và cập nhật thanh tiến trình
            Observable.CombineLatest(player.CurrentXp, player.XpToNextLevel,
                (current, next) => (float)current / next)
                .Subscribe(fillAmount =>
                {
                    sliderXp.value = fillAmount;
                })
                .AddTo(this);
        }
    }

}
