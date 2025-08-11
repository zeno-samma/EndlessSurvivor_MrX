using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro; // Cần để dùng TextMeshPro

namespace MrX.EndlessSurvivor
{
    public class StageUI : MonoBehaviour
    {
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private TextMeshProUGUI timeWave;      // Text hiển thị %
        void Start()
        {
            MessageBroker.Default // Một cách dùng cho trường hợp phụ thuộc ngược(Thay vì dùng eventbus)
                .Receive<WaveCountdownTickMessage>() // Nhận dòng chảy tin nhắn loại này
                .Subscribe(message =>
                { // Đăng ký lắng nghe
                    timeWave.text = Mathf.CeilToInt(message.RemainingTime).ToString();
                })
                .AddTo(this);
        }
    }
}

