using System;
using System.Collections.Generic;
using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Ins { get; private set; }
        public List<Enemy> activeEnemies = new List<Enemy>();

        // Thuộc tính để WaveSpawner có thể kiểm tra xem còn bao nhiêu địch
        public int ActiveEnemyCount => activeEnemies.Count;
        [SerializeField]private Transform playerTransform; // Kéo đối tượng Player vào đây
        void Awake()
        {
            // Singleton Pattern
            if (Ins != null && Ins != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Ins = this;
            }
        }
        void OnEnable()
        {
            // Đăng ký lắng nghe
            EventBus.Subscribe<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        void OnDisable()
        {
            // Hủy đăng ký để tránh lỗi
            EventBus.Unsubscribe<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent value)
        {
            // Nhận Transform từ sự kiện và lưu lại
            playerTransform = value.PlayerTransform;
            // Debug.Log("EnemyManager đã nhận được vị trí của Player!");
        }

        void Start()
        {

        }
        void Update()
        {
            // Đảm bảo Player tồn tại trước khi ra lệnh
            if (playerTransform == null) return;
            // Vòng lặp chỉ huy
            foreach (Enemy enemy in activeEnemies)
            {
                // 1. Manager tính toán hướng đi cho mỗi con Enemy
                Vector3 direction = (playerTransform.position - enemy.transform.position).normalized;

                // 2. Manager ra lệnh cho Enemy di chuyển theo hướng đó
                // Truy cập component Movement thông qua hub Enemy.cs
                enemy.Movement.Move(direction);
            }
        }
        // Thay đổi kiểu tham số của hàm
        public void RegisterEnemy(Enemy enemy)
        {
            if (!activeEnemies.Contains(enemy))
            {
                activeEnemies.Add(enemy);
            }
        }

        // Thay đổi kiểu tham số của hàm
        public void UnregisterEnemy(Enemy enemy)
        {
            if (activeEnemies.Contains(enemy))
            {
                activeEnemies.Remove(enemy);
            }
        }
    }

}