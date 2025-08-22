using UnityEngine;
using TMPro; // Thêm dòng này để sử dụng TextMeshPro

namespace MrX.EndlessSurvivor
{
    public class FPSCounter : MonoBehaviour
    {
        // Kéo đối tượng Text (TMP) từ Hierarchy vào đây trong Inspector
        [SerializeField]
        private TMP_Text fpsText;

        // Khoảng thời gian cập nhật FPS (ví dụ: 1 giây một lần)
        [SerializeField]
        private float updateInterval = 1.0f;

        private float accum = 0.0f; // Tích lũy thời gian qua các frame
        private int frames = 0; // Đếm số frame trong khoảng thời gian updateInterval
        private float timeleft; // Thời gian còn lại

        void Start()
        {
            // Khởi tạo giá trị ban đầu
            timeleft = updateInterval;
        }

        void Update()
        {
            // Trừ thời gian đã trôi qua của frame này
            timeleft -= Time.deltaTime;
            // Tích lũy thời gian
            accum += Time.timeScale / Time.deltaTime;
            // Tăng bộ đếm frame
            ++frames;

            // Nếu đã hết khoảng thời gian cập nhật
            if (timeleft <= 0.0)
            {
                // Tính FPS trung bình
                float fps = accum / frames;
                // Format chuỗi hiển thị
                string format = System.String.Format("FPS:{0:F0}", fps); // F0 để làm tròn thành số nguyên
                fpsText.text = format;

                // Đặt lại các biến đếm cho chu kỳ tiếp theo
                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }
        }
    }
}
