using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Cần để dùng Slider
using TMPro; // Cần để dùng TextMeshPro

namespace MrX.EndlessSurvivor
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        [Header("Loading Screen UI")]
        [SerializeField] private GameObject loadingScreenPanel; // Panel chứa toàn bộ màn hình chờ
        [SerializeField] private GameObject panelBG;      // Bg
        [SerializeField] private Slider progressBar;            // Thanh Slider
        [SerializeField] private TextMeshProUGUI progressText;      // Text hiển thị %

        void Awake()
        {
            // Thiết lập Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Giữ SceneLoader tồn tại xuyên suốt game
            }
        }

        // Hàm public để các script khác gọi khi muốn tải scene
        public void LoadScene(string sceneName)
        {
            // Bắt đầu Coroutine để xử lý việc tải bất đồng bộ
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // 1. Kích hoạt màn hình chờ
            loadingScreenPanel.SetActive(true);

            // 2. Bắt đầu tải scene ở chế độ nền
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            // 3. Lặp lại cho đến khi scene tải gần xong
            while (!operation.isDone)
            {
                // operation.progress có giá trị từ 0.0 đến 0.9
                // Chúng ta cần chuyển nó về thang 0.0 đến 1.0
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

                // Cập nhật UI
                progressBar.value = progressValue;
                progressText.text = (progressValue * 100f).ToString("F0") + "%"; // Làm tròn đến số nguyên

                yield return null; // Chờ đến frame tiếp theo
            }

            // 4. (Tùy chọn) Sau khi tải xong, có thể đợi một chút để người chơi kịp đọc chữ
            yield return new WaitForSeconds(0.5f);

            // 5. Ẩn màn hình chờ đi sau khi scene mới đã được kích hoạt hoàn toàn
            loadingScreenPanel.SetActive(false);
            panelBG.SetActive(false);
        }
    }
}

