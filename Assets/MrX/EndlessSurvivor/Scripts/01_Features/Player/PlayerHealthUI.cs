using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MrX.EndlessSurvivor
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthUI : MonoBehaviour
    {
        private Image healthBarImage;
        public PlayerInfo playerInfo;
        void Awake()
        {
            healthBarImage = GetComponent<Image>();
            playerInfo = GetComponentInParent<PlayerInfo>();
        }
        void Start()
        {
            if (playerInfo == null)
            {
                Debug.LogError("Không tìm thấy PlayerHealth component!");
                return;
            }

            // === ĐÂY LÀ PHẦN MA THUẬT CỦA UNIRX ===
            // Lắng nghe dòng chảy CurrentHealth
            playerInfo.CurrentHealth
                .Subscribe(newHealthValue =>
                {
                    // Mỗi khi CurrentHealth.Value thay đổi, code bên trong này sẽ tự động chạy
                    // Tính toán tỉ lệ máu
                    float fillPercentage = newHealthValue / playerInfo.MaxHealth;

                    // Cập nhật thanh máu
                    healthBarImage.fillAmount = fillPercentage;
                })
                .AddTo(this); // Rất quan trọng: Tự động hủy lắng nghe khi GameObject này bị destroy
        }
    }

}
