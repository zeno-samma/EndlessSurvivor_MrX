using System;
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
        public int currentXp;
        public int xpToNextLevel;
        public int Level;
        void OnEnable()
        {
            Debug.Log($"currentXp: {currentXp}");
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
            currentXp += amount;
            Debug.Log($"currentXp: {currentXp}");
            if (currentXp >= xpToNextLevel)
            {
                // Trừ đi lượng XP cần thiết để lên cấp
                currentXp -= xpToNextLevel;
                // Tăng mốc XP cho cấp tiếp theo (ví dụ tăng 50%)
                xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
                Level += 1;
                Debug.Log($"Level: {Level}");
                // BÂY GIỜ MỚI LÀ LÚC PHÁT SỰ KIỆN LÊN CẤP
                EventBus.Publish(new PlayerLeveledUpEvent()); // Event này không cần mang data

                // Tạm dừng game và gọi bảng nâng cấp
                EventBus.Publish(new StateUpdatedEvent { CurState = GameState.UPGRADEPHASE }); //
                Time.timeScale = 0;
            }
            // ... kiểm tra logic level up ở đây ...
        }
        // Test asmdef;
    }
}

