using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource; // Kênh phát nhạc nền
        [SerializeField] private AudioSource sfxSource;   // Kênh phát hiệu ứng

        [Header("Audio Clips")]
        public AudioClip loadingMusic;
        public AudioClip mainMenuMusic;
        public AudioClip gameplayMusic;
        public AudioClip shootSFX;
        public AudioClip bulletHitSFX;
        // Thêm các clip cho SFX nếu cần, ví dụ: public AudioClip shootSFX;

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
                // DontDestroyOnLoad(gameObject);
            }
        }

        // Hàm này sẽ được gọi từ các Manager khác để thay đổi nhạc nền
        public void PlayMusic(AudioClip musicClip)
        {
            // Kiểm tra xem có cần đổi nhạc không để tránh phát lại từ đầu một cách không cần thiết
            if (musicSource.clip == musicClip) return;

            musicSource.clip = musicClip;
            musicSource.loop = true; // Nhạc nền thường lặp lại
            musicSource.Play();
        }

        // Hàm để phát một hiệu ứng âm thanh
        public void PlaySFX(AudioClip sfxClip)
        {
            // PlayOneShot cho phép phát nhiều hiệu ứng chồng lên nhau mà không cắt ngang
            sfxSource.PlayOneShot(sfxClip);
        }
    }
}