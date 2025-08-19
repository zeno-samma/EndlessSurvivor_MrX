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
        [SerializeField] private TextMeshProUGUI currentWaveNumber;      // Text hiển thị %
        [SerializeField] private TextMeshProUGUI currentNextWaveNumber;      // Text hiển thị %
        [SerializeField] private CanvasGroup canvasGroup;

        void OnEnable()
        {
            canvasGroup.alpha = 0;
            // Lắng nghe sự kiện cập nhật tiến trình
            EventBus.Subscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);
        }

        // Hàm xử lý khi nhận được sự kiện
        private void OnWaveProgressUpdated(WaveProgressUpdatedEvent eventData)
        {
            sliderProgress.value = eventData.progressPercentage;
            currentWaveNumber.text =$"{eventData.currentWaveNumber}";
            currentNextWaveNumber.text = $"{eventData.currentWaveNumber + 1}";
        }
        void Start()
        {
            MessageBroker.Default // Một cách dùng cho trường hợp phụ thuộc ngược(Thay vì dùng eventbus)
                .Receive<WaveCountdownTickMessage>() // Nhận dòng chảy tin nhắn loại này
                .Subscribe(message =>
                { // Đăng ký lắng nghe
                    canvasGroup.alpha = 1;
                    timeWave.text = Mathf.RoundToInt(message.RemainingTime).ToString();
                    if (Mathf.CeilToInt(message.RemainingTime) <= 1f)
                    {
                        // Debug.Log($"CurrentTime: {message.RemainingTime}");
                        canvasGroup.alpha = 0;
                    }
                })
                .AddTo(this);   
        }
    }
}

