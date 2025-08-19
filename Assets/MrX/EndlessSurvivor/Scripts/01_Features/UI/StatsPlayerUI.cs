using UnityEngine;
using TMPro; // Cần để dùng TextMeshPro
using UniRx; // Thêm để dùng Subscribe với lambda

namespace MrX.EndlessSurvivor
{
    public class StatsPlayerUI : MonoBehaviour
    {

        public PlayerInfo playerInfo;               // Kéo Player vào đây
        [SerializeField] private TextMeshProUGUI valueDamage;      // Text hiển thị %
        [SerializeField] private TextMeshProUGUI valueHealth;      // Text hiển thị %
        [SerializeField] private TextMeshProUGUI valueDef;      // Text hiển thị %

        // [SerializeField] private CanvasGroup canvasGroup;

        void Start()
        {
            // Lắng nghe sự thay đổi của Level từ Player
            playerInfo.CurrentDamage
                .Subscribe(value => // Đổi tên thành newDamageValue cho rõ nghĩa
                {
                    // Sử dụng đúng tên biến đã khai báo ở trên
                    valueDamage.text = value.ToString();
                })
                .AddTo(this);
            // Lắng nghe sự thay đổi của Level từ Player
            playerInfo.CurrentHealth
                .Subscribe(value => // Đổi tên thành newDamageValue cho rõ nghĩa
                {
                    // Sử dụng đúng tên biến đã khai báo ở trên
                    valueHealth.text = value.ToString();
                })
                .AddTo(this);
            // Lắng nghe sự thay đổi của Level từ Player
            playerInfo.CurrentDef
                .Subscribe(value => // Đổi tên thành newDamageValue cho rõ nghĩa
                {
                    // Sử dụng đúng tên biến đã khai báo ở trên
                    valueDef.text = value.ToString();
                })
                .AddTo(this);
        }
    }
}

