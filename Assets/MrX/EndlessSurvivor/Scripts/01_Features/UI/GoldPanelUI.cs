using TMPro;
using UnityEngine;
using UniRx; // Thêm để dùng Subscribe với lambda

namespace MrX.EndlessSurvivor
{
    public class GoldPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI goldText; // Kéo Text hiển thị số 3 vào đây
        public PlayerInfo playerInfo;               // Kéo Player vào đây
        void Start()
        {
            // Lắng nghe sự thay đổi của Level từ Player
            playerInfo.CurrentGold
                .Subscribe(newvalue =>
                {
                    // Cập nhật Text với level mới
                    goldText.text = "" + newvalue;
                })
                .AddTo(this);
        }
    }

}
