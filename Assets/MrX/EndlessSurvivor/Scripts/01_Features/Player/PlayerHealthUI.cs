using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MrX.EndlessSurvivor
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthUI : MonoBehaviour
    {
        private Image healthBarImage;
        private PlayerHealth playerHealth; // Tham chiếu đến script quản lý máu
        void Awake()
        {
            healthBarImage = GetComponent<Image>();
        }
        void Start()
        {
            // Tìm đến component PlayerHealth (giả sử nó nằm trên đối tượng cha hoặc cùng cấp)
            playerHealth = GetComponentInParent<PlayerHealth>();

            // Nếu không tìm thấy, có thể dùng Player.Instance nếu bạn có
            // if (playerHealth == null && Player.Instance != null)
            // {
            //     playerHealth = Player.Instance.Health;
            // }

            if (playerHealth == null)
            {
                Debug.LogError("Không tìm thấy PlayerHealth component!");
                return;
            }

            // === ĐÂY LÀ PHẦN MA THUẬT CỦA UNIRX ===
            // Lắng nghe dòng chảy CurrentHealth
            playerHealth.CurrentHealth
                .Subscribe(newHealthValue =>
                {
                    // Mỗi khi CurrentHealth.Value thay đổi, code bên trong này sẽ tự động chạy
                    // Tính toán tỉ lệ máu
                    float fillPercentage = newHealthValue / playerHealth.MaxHealth;

                    // Cập nhật thanh máu
                    healthBarImage.fillAmount = fillPercentage;
                })
                .AddTo(this); // Rất quan trọng: Tự động hủy lắng nghe khi GameObject này bị destroy
        }
    }

}
