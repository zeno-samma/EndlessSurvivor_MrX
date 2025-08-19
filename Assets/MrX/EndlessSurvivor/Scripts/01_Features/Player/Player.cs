using System;
using UniRx;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class Player : MonoBehaviour
    {
        // Có thể biến nó thành Singleton để dễ truy cập toàn cục
        public static Player Instance { get; private set; }
        // "Bộ não" sẽ giữ tham chiếu đến tất cả các bộ phận chuyên môn
        public PlayerMovement Movement { get; private set; }
        public PlayerHealth Health { get; private set; }
        public WeaponManager Weapon { get; private set; }
        public PlayerInfo playerInfo { get; private set; }
        // Thay thế các biến thông thường
        public ReactiveProperty<int> CurrentLevel { get; private set; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> CurrentXp { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> XpToNextLevel { get; private set; } = new ReactiveProperty<int>(100);
        // public int currentXp;
        // public int xpToNextLevel;
        // public int Level;
        void OnEnable()
        {
            // Debug.Log($"CurrentLevel: {CurrentLevel}");
            // Thông báo cho toàn bộ hệ thống: "Tôi đã xuất hiện! Đây là Transform của tôi."
            EventBus.Publish(new PlayerSpawnedEvent
            {
                playerObject = this.gameObject
            });
        }

        void OnDisable()
        {
            // Hủy đăng ký để tránh lỗi
        }

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
            }

            // Tự động lấy các "bộ phận" của mình
            Movement = GetComponent<PlayerMovement>();
            Health = GetComponent<PlayerHealth>();
            playerInfo = GetComponent<PlayerInfo>();
            Weapon = GetComponentInChildren<WeaponManager>(); // Ví dụ nếu Weapon là con
        }
        void Start()
        {

        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("XpGem"))
            {
                // Lấy script của viên ngọc
                XpGem gem = other.GetComponent<XpGem>();
                if (gem != null)
                {
                    // Player tự xử lý việc nhận XP
                    GainExperience(gem.xpPoint);

                    // Ra lệnh cho viên ngọc biến mất
                    other.gameObject.SetActive(false);
                }
            }
        }

        void GainExperience(int amount)
        {
            CurrentXp.Value += amount;
            if (CurrentXp.Value >= XpToNextLevel.Value)
            {
                // Trừ đi lượng XP cần thiết để lên cấp
                CurrentXp.Value -= XpToNextLevel.Value;
                // Tăng mốc XP cho cấp tiếp theo (ví dụ tăng 50%)
                XpToNextLevel.Value = Mathf.RoundToInt(XpToNextLevel.Value * 1.5f);
                CurrentLevel.Value += 1;
                // Cập nhật UI
                // progressBar.value = progressValue;
                // BÂY GIỜ MỚI LÀ LÚC PHÁT SỰ KIỆN LÊN CẤP
                // float Value = currentXp / xpToNextLevel;
                // Debug.Log($"Level: {Level} , Value: {Value}");
                EventBus.Publish(new PlayerLeveledUpEvent {});
                Debug.Log("1.Thăng Cấp");

            }
            // ... kiểm tra logic level up ở đây ...
        }
        // Test asmdef;
    }
}

